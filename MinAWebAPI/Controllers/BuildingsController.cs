using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MinAWebAPI.Helpers;
using MinAWebAPI.Models;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Feature = NetTopologySuite.Features.Feature;
using Polygon = NetTopologySuite.Geometries.Polygon;


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
        
    }
}
