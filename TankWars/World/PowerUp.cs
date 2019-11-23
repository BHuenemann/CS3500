//Authors: Ben Huenemann and Jonathan Wigderson

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PowerUp
    {
        //ID of the powerup
        [JsonProperty(PropertyName = "power")]
        public int ID { get; private set; }

        //Location of the powerup
        [JsonProperty(PropertyName = "loc")]
        public Vector2D location { get; private set; }

        //Tells whether the powerup has been picked up or not
        [JsonProperty(PropertyName = "died")]
        public bool died = false;


        public PowerUp()
        {
        }
    }
}
