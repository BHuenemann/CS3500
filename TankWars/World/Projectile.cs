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
        //Projectile ID
        [JsonProperty(PropertyName = "proj")]
        public int ID { get; private set; }

        //Location of Projectile
        [JsonProperty(PropertyName = "loc")]
        public Vector2D location { get; private set; }

        //Direction of the Projectile
        [JsonProperty(PropertyName = "dir")]
        public Vector2D orientation { get; private set; }

        //Tells whether the Projectile has collied with something or not
        [JsonProperty(PropertyName = "died")]
        public bool died = false;

        //Owner tank ID of the Projectile
        [JsonProperty(PropertyName = "owner")]
        public int ownerID { get; private set; }
    }
}
