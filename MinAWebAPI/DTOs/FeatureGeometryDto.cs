namespace MinAWebAPI.DTOs;

public class FeatureGeometryDto
{
    public string Type { get; set; }
    public Polygon Geometry { get; set; }
    public FeatureProperties Properties { get; set; }
    public string Id { get; set; }
}
public class Polygon
{
    public string Type { get; set; }
    public List<List<List<double>>> Coordinates { get; set; }
}
public class FeatureProperties
{
    public string AddrCity { get; set; }
    public string AddrCountry { get; set; }
    public string AddrHouseNumber { get; set; }
    public string AddrPostcode { get; set; }
    public string AddrStreet { get; set; }
    public string Building { get; set; }
    public string BuildingLevels { get; set; }
    public string Name { get; set; }
    public string NameAz { get; set; }
    public string NameEn { get; set; }
    public string NameRu { get; set; }
    public string Geotype { get; set; }
    public int Index { get; set; }
}