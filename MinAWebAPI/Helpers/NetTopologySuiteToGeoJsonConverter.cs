using GeoJSON.Net;
using GeoJSON.Net.Geometry;

using Newtonsoft.Json;
using System.Collections.Generic;
using NetTopologySuite.Geometries;
using LineString = GeoJSON.Net.Geometry.LineString;
using Polygon = GeoJSON.Net.Geometry.Polygon;
using Position = GeoJSON.Net.Geometry.Position;
using Feature = GeoJSON.Net.Feature.Feature;
using FeatureCollection = GeoJSON.Net.Feature.FeatureCollection;
using Point = GeoJSON.Net.Geometry.Point;


namespace MinAWebAPI.Helpers;

public class NetTopologySuiteToGeoJsonConverter
{
    public static string ConvertToGeoJson(Geometry geometry)
    {
        var coordinates = new List<IPosition>();
        foreach (var coordinate in geometry.Coordinates)
        {
            coordinates.Add(new Position(coordinate.Y, coordinate.X));
        }

        var polygon = new Polygon(new List<LineString>
        {
            new LineString(coordinates)
        });

        var feature = new Feature(polygon, new Dictionary<string, object>
        {
            { "index", 0 },
            { "geotype", geometry.GeometryType }
        });
        var featureCollection = new FeatureCollection();
        featureCollection.Features.Add(feature);
        
        return JsonConvert.SerializeObject(featureCollection);
    }
    
    public static string ConvertToGeoJsonList(List<Geometry> geometries)
    {
        var features = new List<Feature>();

        foreach (var geometry in geometries)
        {
            var coordinates = new List<IPosition>();
            foreach (var coordinate in geometry.Coordinates)
            {
                coordinates.Add(new Position(coordinate.Y, coordinate.X));
            }

            var polygon = new Polygon(new List<LineString>
            {
                new LineString(coordinates)
            });

            var feature = new Feature(polygon, new Dictionary<string, object>
            {
                { "index", features.Count },
                { "geotype", geometry.GeometryType }
            });

            features.Add(feature);
        }

        var featureCollection = new FeatureCollection(features);

        return JsonConvert.SerializeObject(featureCollection);
    }

    public static string ConvertPointToJson(List<NetTopologySuite.Geometries.Point> points)
    {
        var features = new List<GeoJSON.Net.Feature.Feature>();
        foreach (var point in points)
        {
            var p = new Point(new Position(point.X, point.Y));
            var feature = new Feature(p);
            if (feature != null) features.Add(feature);
        }
        var featureCollection = new FeatureCollection(features);
        
        return JsonConvert.SerializeObject(featureCollection);
    }
    
}