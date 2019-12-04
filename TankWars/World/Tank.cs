﻿//Authors: Ben Huenemann and Jonathan Wigderson

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TankWars
{
    /// <summary>
    /// Class that represents a tank object. It contains info about the tank that can be serialized
    /// and sent to the server
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        public int PowerUps { get; internal set; } = 0;

        public int CooldownFrames { get; internal set; } = 0;

        public int RespawnFrames { get; internal set; } = 0;

        public Vector2D Velocity { get; internal set; } = new Vector2D(0, 0);

        //ID of the tank
        [JsonProperty(PropertyName = "tank")]
        public int ID { get; internal set; }

        //Location of the tank
        [JsonProperty(PropertyName = "loc")]
        public Vector2D Location { get; internal set; }

        //Orientation of the tank
        [JsonProperty(PropertyName = "bdir")]
        public Vector2D Orientation { get; internal set; } = new Vector2D(0, -1);

        //Orientation of the turret
        [JsonProperty(PropertyName = "tdir")]
        public Vector2D Aiming { get; internal set; } = new Vector2D(0, -1);

        //Tank's player name
        [JsonProperty(PropertyName = "name")]
        public string Name { get; internal set; }

        //Tank HP
        [JsonProperty(PropertyName = "hp")]
        public int HitPoints { get; internal set; } = Constants.MaxHP;

        //Tank Score
        [JsonProperty(PropertyName = "score")]
        public int Score { get; internal set; } = 0;

        //Tells whether the Tank has died or niot
        [JsonProperty(PropertyName = "died")]
        public bool Died { get; internal set; } = false;

        //Tells whether the Tank has disconnected or not
        [JsonProperty(PropertyName = "dc")]
        public bool Disconnected { get; internal set; } = false;

        //Tells whether the tank has joined the game or not
        [JsonProperty(PropertyName = "join")]
        public bool Joined { get; internal set; } = false;


        public Tank()
        {

        }


        public Tank(string name, int ID)
        {
            this.Name = name;
            this.ID = ID;
        }
    }
}
