//Authors: Ben Huenemann and Jonathan Wigderson

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {
        //Wall ID
        [JsonProperty(PropertyName = "wall")]
        public int ID { get; private set; }

        //Endpoint 1 of the wall
        [JsonProperty(PropertyName = "p1")]
        public Vector2D endPoint1 { get; private set; }

        //Endpoint 2 of the wall
        [JsonProperty(PropertyName = "p2")]
        public Vector2D endPoint2 { get; private set; }

        public Wall()
        {
        }
    }
}
