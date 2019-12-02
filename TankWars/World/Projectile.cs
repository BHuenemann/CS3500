//Authors: Ben Huenemann and Jonathan Wigderson

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    /// <summary>
    /// Class that represents a projectile object. It contains info about the projectile that can be serialized
    /// and sent to the server
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        static int NextID = 0;

        //Projectile ID
        [JsonProperty(PropertyName = "proj")]
        public int ID { get; internal set; }

        //Location of Projectile
        [JsonProperty(PropertyName = "loc")]
        public Vector2D location { get; internal set; }

        //Direction of the Projectile
        [JsonProperty(PropertyName = "dir")]
        public Vector2D orientation { get; internal set; }

        //Tells whether the Projectile has collied with something or not
        [JsonProperty(PropertyName = "died")]
        public bool died { get; internal set; } = false;

        //Owner tank ID of the Projectile
        [JsonProperty(PropertyName = "owner")]
        public int ownerID { get; internal set; }

        public Projectile(Vector2D currentLocation, Vector2D direction, int owner)
        {
            ID = NextID;
            NextID++;

            location = currentLocation;
            orientation = direction;
            ownerID = owner;

        }
    }
}
