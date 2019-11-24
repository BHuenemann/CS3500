﻿//Authors: Ben Huenemann and Jonathan Wigderson

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    /// <summary>
    /// Class that represents a power up object. It contains info about the power up that can be serialized
    /// and sent to the server
    /// </summary>
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
    }
}
