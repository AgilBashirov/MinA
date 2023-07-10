using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace MinAWebAPI.Models
{
    public partial class Marshrut
    {
        public int Id { get; set; }
        public Geometry? WkbGeometry { get; set; }
    }
}
