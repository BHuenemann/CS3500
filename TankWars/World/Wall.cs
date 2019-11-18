using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {
        [JsonProperty(PropertyName = "wall")]
        private int ID;

        [JsonProperty(PropertyName = "p1")]
        private Vector2D endPoint1;

        [JsonProperty(PropertyName = "p2")]
        private Vector2D endPoint2;

        public Wall()
        {
        }
    }
}
