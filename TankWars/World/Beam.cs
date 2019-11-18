using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Beam
    {
        [JsonProperty(PropertyName = "beam")]
        private int ID;

        [JsonProperty(PropertyName = "org")]
        public Vector2D origin { get; private set; }

        [JsonProperty(PropertyName = "dir")]
        public Vector2D orientation { get; private set; }

        [JsonProperty(PropertyName = "owner")]
        public int ownerID { get; private set; }


        public Beam()
        {
        }
    }
}
