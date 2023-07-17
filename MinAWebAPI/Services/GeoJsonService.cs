
using GeoAPI.Geometries;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using GeoJSON.Net.Feature;

namespace MinAWebAPI.Services;

public class GeoJsonService
{
    public static NetTopologySuite.Geometries.Geometry GeoJsonToGeometry(string geoJson)
    {
        var reader = new GeoJsonReader();
        var feature = reader.Read<NetTopologySuite.Features.Feature>(geoJson);
        var geometry = feature.Geometry as Geometry;

        return geometry;
    }

    public static async Task<string> FileToGeoJson(IFormFile file)
    {
        using var streamReader = new StreamReader(file.OpenReadStream());
        if (streamReader == null) throw new ArgumentNullException(nameof(streamReader));
        return await streamReader.ReadToEndAsync();
    }

  
}