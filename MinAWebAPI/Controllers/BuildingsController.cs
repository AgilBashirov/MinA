using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinAWebAPI.DTOs;
using MinAWebAPI.Helpers;
using MinAWebAPI.Models;
using NetTopologySuite;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Feature = NetTopologySuite.Features.Feature;
using Polygon = NetTopologySuite.Geometries.Polygon;
using NetTopologySuite.Geometries;
using Npgsql;
using System.IO;
using System.Linq;
using MinAWebAPI.Services;
using NetTopologySuite.Index.Strtree;
using Point = GeoJSON.Net.Geometry.Point;


namespace MinAWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuildingsController : ControllerBase
    {
        private readonly MinAContext _context;

        public BuildingsController(MinAContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var buildings = await _context.Binas.ToListAsync();

            List<Geometry> geometries = new();

            foreach (var build in buildings)
            {
                geometries.Add(build.WkbGeometry!);
            }

            // // Convert the geometry to GeoJSON
            var geoJson = NetTopologySuiteToGeoJsonConverter.ConvertToGeoJsonList(geometries);
            return Ok(geoJson);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var building = await _context.Binas.FirstOrDefaultAsync(x => x.Id == id);

            var coordinates = building.WkbGeometry.Coordinates;
            var polygon = new Polygon(new LinearRing(coordinates));

            // // Convert the geometry to GeoJSON
            var geoJson = NetTopologySuiteToGeoJsonConverter.ConvertToGeoJson(polygon);
            return Ok(geoJson);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (!file.FileName.EndsWith(".json") && !file.FileName.EndsWith(".geojson"))
            {
                return BadRequest("Invalid file format. Only GeoJSON files are allowed.");
            }

            var geojsonData = await GeoJsonService.FileToGeoJson(file);

            var geo = GeoJsonService.GeoJsonToGeometry(geojsonData);
            var newBina = new Bina()
            {
                WkbGeometry = geo
            };

            var result = await _context.Binas.AddAsync(newBina);

            await _context.SaveChangesAsync();

            return Ok(newBina.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, IFormFile file)
        {
            var existingBina = await _context.Binas.FindAsync(id);

            if (existingBina == null)
            {
                return NotFound();
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            if (!file.FileName.EndsWith(".json") && !file.FileName.EndsWith(".geojson"))
            {
                return BadRequest("Invalid file format. Only GeoJSON files are allowed.");
            }

            var geojsonData = await GeoJsonService.FileToGeoJson(file);
            var geo = GeoJsonService.GeoJsonToGeometry(geojsonData);

            existingBina.WkbGeometry = geo;

            await _context.SaveChangesAsync();

            return Ok("File updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var currentBina = await _context.Binas.FindAsync(id);
            if (currentBina is null) throw new ArgumentNullException(nameof(currentBina));
            _context.Remove(currentBina);
            return Ok("File deleted successfully.");
        }


        [HttpGet("GetPoi/{id}")]
        public async Task<IActionResult> GetPoi(int id)
        {
            var bina = await _context.Binas.FindAsync(id);
            var geometry = bina?.WkbGeometry;
            var pois = await _context.Pois.ToListAsync();
            var pointList = new List<NetTopologySuite.Geometries.Point>();

            foreach (var poi in pois)
            {
                pointList.Add(poi.WkbGeometry);
            }

            var points = pointList.Where(x => geometry.Contains(x)).ToList();
            

            var result = NetTopologySuiteToGeoJsonConverter.ConvertPointToJson(points);
            return Ok(result);
        }
    }
}