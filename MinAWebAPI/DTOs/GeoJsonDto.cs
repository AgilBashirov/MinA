namespace MinAWebAPI.DTOs;

public class GeoJsonDto
{
    public string Type { get; set; }
    //public List<Feature> Features { get; set; }
}

// public class Feature
// {
//     public string Type { get; set; }
//     public Geometry Geometry { get; set; }
//     public Property Properties { get; set; }
//
// }

// public class Geometry
// {
//     public string Type { get; set; }
//     public List<List<List<double>>> Coordinates { get; set; }
// }

// public class Property
// {
//     public int Index { get; set; }
//     public string Geotype { get; set; }
// }