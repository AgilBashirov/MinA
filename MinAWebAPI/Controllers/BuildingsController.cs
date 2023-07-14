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
        public async Task<ActionResult> Get()
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
        public async Task<ActionResult> Get(int id)
        {
            var building = await _context.Binas.FirstOrDefaultAsync(x=>x.Id == id);

            var coordinates = building.WkbGeometry.Coordinates;
            var polygon = new Polygon(new LinearRing(coordinates));
            
            // // Convert the geometry to GeoJSON
            var geoJson = NetTopologySuiteToGeoJsonConverter.ConvertToGeoJson(polygon);
            return Ok(geoJson);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, IFormFile geometryFile)
        {
            var existingBina = await _context.Binas.FindAsync(id);

            if (existingBina == null)
            {
                return NotFound();
            }

            // Dosyadan veriyi okuyun
            using (var memoryStream = new MemoryStream())
            {
                await geometryFile.CopyToAsync(memoryStream);
                var geometryBytes = memoryStream.ToArray();

                // Dosyadaki geometri verisini Geometry nesnesine dönüştürün
                GeometryFactory geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                var reader = new WKBReader(geometryFactory.GeometryServices);
                var geometry = reader.Read(geometryBytes);

                // Güncelleme işlemlerinde Geometry nesnesini kullanın
                existingBina.WkbGeometry = geometry;
            }
            // var coordinates = building.WkbGeometry.Coordinates;
            // var polygon = new Polygon(new LinearRing(coordinates));
            
            // // Convert the geometry to GeoJSON
            // var geoJson = NetTopologySuiteToGeoJsonConverter.ConvertToGeoJson(geometry);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadGeoJSON(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Ensure the file has a .json or .geojson extension
            if (!file.FileName.EndsWith(".json") && !file.FileName.EndsWith(".geojson"))
            {
                return BadRequest("Invalid file format. Only GeoJSON files are allowed.");
            }

            // Process the GeoJSON file
            // Here, you can perform any necessary operations on the uploaded file
            // such as reading, parsing, and validating the GeoJSON data

            // Example: Read the GeoJSON data
            using var streamReader = new StreamReader(file.OpenReadStream());
            var geojsonData = await streamReader.ReadToEndAsync();

            // Example: Print the GeoJSON data
            System.Console.WriteLine(geojsonData);
            
            GeoJsonService.GeoJsonToGeometry(geojsonData);

            return Ok("File uploaded successfully.");
            
            
        }
        

    }
}
