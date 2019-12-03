//Authors: Ben Huenemann and Jonathan Wigderson

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
        public int cooldownFrames { get; internal set; } = 0;

        public Vector2D velocity { get; internal set; } = new Vector2D(0, 0);

        //ID of the tank
        [JsonProperty(PropertyName = "tank")]
        public int ID { get; internal set; }

        //Location of the tank
        [JsonProperty(PropertyName = "loc")]
        public Vector2D location { get; internal set; }

        //Orientation of the tank
        [JsonProperty(PropertyName = "bdir")]
        public Vector2D orientation { get; internal set; } = new Vector2D(0, -1);

        //Orientation of the turret
        [JsonProperty(PropertyName = "tdir")]
        public Vector2D aiming { get; internal set; } = new Vector2D(0, -1);

        //Tank's player name
        [JsonProperty(PropertyName = "name")]
        public string name { get; internal set; }

        //Tank HP
        [JsonProperty(PropertyName = "hp")]
        public int hitPoints { get; internal set; } = Constants.MaxHP;

        //Tank Score
        [JsonProperty(PropertyName = "score")]
        public int score { get; internal set; } = 0;

        //Tells whether the Tank has died or niot
        [JsonProperty(PropertyName = "died")]
        public bool died { get; internal set; } = false;

        //Tells whether the Tank has disconnected or not
        [JsonProperty(PropertyName = "dc")]
        public bool disconnected { get; internal set; } = false;

        //Tells whether the tank has joined the game or not
        [JsonProperty(PropertyName = "join")]
        public bool joined { get; internal set; } = false;


        public Tank()
        {

        }


        public Tank(string name, int ID)
        {
            this.name = name;
            this.ID = ID;
        }
    }
}
