using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {
        [JsonProperty(PropertyName = "proj")]
        public int ID { get; private set; }

        [JsonProperty(PropertyName = "loc")]
        public Vector2D location { get; private set; }

        [JsonProperty(PropertyName = "dir")]
        public Vector2D orientation { get; private set; }

        [JsonProperty(PropertyName = "died")]
        public bool died = false;

        [JsonProperty(PropertyName = "owner")]
        public int ownerID { get; private set; }


        public Projectile()
        {

        }
    }
}
