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

            List<Geometry> geometries  = new();

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
            var building = await _context.Binas.FirstOrDefaultAsync(x=>x.Id == id);

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

            var result  = await _context.Binas.AddAsync(new Bina()
            {
                WkbGeometry = geo
            });
            
            await _context.SaveChangesAsync();

            return Ok("File uploaded successfully.");
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
        public async Task Delete(int id)
        {
            var currentBina = await _context.Binas.FindAsync(id);
            if(currentBina is null) throw new ArgumentNullException(nameof(currentBina));
            _context.Remove(currentBina);
        }


        // [HttpGet("GetPoi/{id}")]
        // public async Task GetPoi(int id)
        // {
        //      var poi = await _context.Pois.ToListAsync();
        //
        //      var bina = await _context.Binas.FindAsync(id);
        //
        //      var overlappedPolygon =poi.WkbGeometry.Intersection(bina.WkbGeometry);
        //      
        //      var overlappedPoints = overlappedPolygon.Coordinates;
        //      foreach (var point in overlappedPoints)
        //      {
        //          Console.WriteLine(point); // Prints the overlapped points
        //      }
        // }
        
    }
}
