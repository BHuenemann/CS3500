//Authors: Ben Huenemann and Jonathan Wigderson

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Beam
    {
        //ID of the beam
        [JsonProperty(PropertyName = "beam")]
        public int ID { get; private set; }

        //Location for the beam to be drawn from
        [JsonProperty(PropertyName = "org")]
        public Vector2D origin { get; private set; }

        //Direction for the beam to be drawn
        [JsonProperty(PropertyName = "dir")]
        public Vector2D orientation { get; private set; }

        //Owner of the beam
        [JsonProperty(PropertyName = "owner")]
        public int ownerID { get; private set; }

        public Dictionary<int, Vector2D> beamParticles = new Dictionary<int, Vector2D>();
        public int beamFrames = 0;



        public Beam()
        {
        }
    }
}
