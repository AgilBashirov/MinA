
using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using GeoJSON.Net.Feature;
using Feature = GeoJSON.Net.Feature.Feature;

namespace MinAWebAPI.Services;

public class GeoJsonService
{
    public static void GeoJsonToGeometry(string geoJson)
    {
        // Create a JSON reader
        var reader = new JsonTextReader(new System.IO.StringReader(geoJson));

// Create a GeoJsonSerializer
        var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

// Deserialize the GeoJSON data to a Feature
        var feature = serializer.Deserialize<Feature>(reader);

// Get the geometry from the feature
        var geometry = feature.Geometry;

// Convert the geometry to a NetTopologySuite Geometry object
        // var ntsGeometry = geometry.ToNTSGeometry();
    }
}