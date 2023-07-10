using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace MinAWebAPI.Models
{
    public partial class Bina
    {
        public int Id { get; set; }
        public Geometry? WkbGeometry { get; set; }
    }
}
