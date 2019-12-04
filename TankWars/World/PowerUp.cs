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

        static int NextID = 0;

        //ID of the powerup
        [JsonProperty(PropertyName = "power")]
        public int ID { get; internal set; }

        //Location of the powerup
        [JsonProperty(PropertyName = "loc")]
        public Vector2D location { get; internal set; }

        //Tells whether the powerup has been picked up or not
        [JsonProperty(PropertyName = "died")]
        public bool died { get; internal set; } = false;



        public PowerUp()
        {
            ID = NextID;
            NextID++;
        }
    }
}
