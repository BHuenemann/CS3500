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
        private Vector2D origin;

        [JsonProperty(PropertyName = "dir")]
        private Vector2D orientation;

        [JsonProperty(PropertyName = "owner")]
        public int ownerID { get; private set; }


        public Beam()
        {
        }
    }
}
