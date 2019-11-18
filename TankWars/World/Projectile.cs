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
        private int ID;

        [JsonProperty(PropertyName = "loc")]
        private Vector2D location;

        [JsonProperty(PropertyName = "dir")]
        private Vector2D orientation;

        [JsonProperty(PropertyName = "died")]
        private bool died = false;

        [JsonProperty(PropertyName = "owner")]
        public int ownerID { get; private set; }


        public Projectile()
        {

        }
    }
}
