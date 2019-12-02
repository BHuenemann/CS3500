//Authors: Ben Huenemann and Jonathan Wigderson

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    /// <summary>
    /// Class that represents a wall object. It contains info about the wall that can be serialized
    /// and sent to the server
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Wall
    {
        static int NextID = 0;

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

        public Wall(Vector2D p1, Vector2D p2)
        {
            ID = NextID;
            NextID++;

            endPoint1 = p1;
            endPoint2 = p2;
        }
    }
}
