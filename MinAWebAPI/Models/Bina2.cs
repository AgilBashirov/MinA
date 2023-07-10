using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace MinAWebAPI.Models
{
    public partial class Bina2
    {
        public int OgcFid { get; set; }
        public string? Id { get; set; }
        public string? AddrCity { get; set; }
        public string? AddrHousenumber { get; set; }
        public string? AddrPostcode { get; set; }
        public string? AddrStreet { get; set; }
        public string? Amenity { get; set; }
        public string? Building { get; set; }
        public string? BuildingLevels { get; set; }
        public string? Country { get; set; }
        public string? Description { get; set; }
        public string? Diplomatic { get; set; }
        public string? Email { get; set; }
        public string? Embassy { get; set; }
        public string? Facebook { get; set; }
        public string? Fax { get; set; }
        public string? Government { get; set; }
        public string? Image { get; set; }
        public string? Leisure { get; set; }
        public string? Name { get; set; }
        public string? NameAz { get; set; }
        public string? NameEn { get; set; }
        public string? NameFr { get; set; }
        public string? NameRu { get; set; }
        public string? Name1 { get; set; }
        public string? Name2 { get; set; }
        public string? Office { get; set; }
        public string? OpeningHours { get; set; }
        public string? OpeningHoursCovid19 { get; set; }
        public string? Operator { get; set; }
        public string? Phone { get; set; }
        public string? Religion { get; set; }
        public string? SourceRef { get; set; }
        public string? Target { get; set; }
        public string? Tourism { get; set; }
        public string? Website { get; set; }
        public string? Wikidata { get; set; }
        public string? Wikipedia { get; set; }
        public string? Geotype { get; set; }
        public int? Index { get; set; }
        public Polygon? WkbGeometry { get; set; }
    }
}
