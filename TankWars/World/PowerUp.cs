using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PowerUp
    {
        [JsonProperty(PropertyName = "power")]
        private int ID;

        [JsonProperty(PropertyName = "loc")]
        public Vector2D location { get; private set; }

        [JsonProperty(PropertyName = "died")]
        private bool died = false;


        public PowerUp()
        {
        }
    }
}
