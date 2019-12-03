//Authors: Ben Huenemann and Jonathan Wigderson

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    /// <summary>
    /// Class that represents a beam object. It contains info about the beam that can be serialized
    /// and sent to the server
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Beam
    {
        static int NextID = 0;

        //ID of the beam
        [JsonProperty(PropertyName = "beam")]
        public int ID { get; internal set; }

        //Location for the beam to be drawn from
        [JsonProperty(PropertyName = "org")]
        public Vector2D origin { get; internal set; }

        //Direction for the beam to be drawn
        [JsonProperty(PropertyName = "dir")]
        public Vector2D orientation { get; internal set; }

        //Owner of the beam
        [JsonProperty(PropertyName = "owner")]
        public int ownerID { get; internal set; }

        //Dictionary containing the particles around the beam and the frames those particles have been out
        public Dictionary<int, Vector2D> beamParticles = new Dictionary<int, Vector2D>();
        public int beamFrames = 0;


        Beam()
        {

        }


        public Beam(Vector2D originPoint, Vector2D beamOrientation, int owner)
        {
            ID = NextID;
            NextID++;

            origin = originPoint;
            orientation = beamOrientation;
            ownerID = owner;
        }
    }


}
