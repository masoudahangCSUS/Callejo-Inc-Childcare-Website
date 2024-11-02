using Org.BouncyCastle.Asn1.Ocsp;
using System;

namespace BlazorApp.Models
{
    public class Guardian
    {
        public Guid FkParent { get; set; }
        public long FkChildren { get; set; }

        // Navigation Properties
        public CallejoIncUser Parent { get; set; }
        public Child Child { get; set; }
    }
}
