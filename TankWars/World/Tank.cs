//Authors: Ben Huenemann and Jonathan Wigderson

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {
        //ID of the tank
        [JsonProperty(PropertyName = "tank")]
        public int ID { get; private set; }

        //Location of the tank
        [JsonProperty(PropertyName = "loc")]
        public Vector2D location { get; private set; }

        //Orientation of the tank
        [JsonProperty(PropertyName = "bdir")]
        public Vector2D orientation { get; private set; }

        //Orientation of the turret
        [JsonProperty(PropertyName = "tdir")]
        public Vector2D aiming = new Vector2D(0, -1);

        //Tank's player name
        [JsonProperty(PropertyName = "name")]
        public string name { get; private set; }

        //Tank HP
        [JsonProperty(PropertyName = "hp")]
        public int hitPoints = Constants.MaxHP;

        //Tank Score
        [JsonProperty(PropertyName = "score")]
        public int score = 0;

        //Tells whether the Tank has died or niot
        [JsonProperty(PropertyName = "died")]
        public bool died = false;

        //Tells whether the Tank has disconnected or not
        [JsonProperty(PropertyName = "dc")]
        public bool disconnected = false;

        //Tells whether the tank has joined the game or not
        [JsonProperty(PropertyName = "join")]
        private bool joined = false;

        public Tank()
        {
        
        }

        public Tank(string tankName, int tankID)
        {
            name = tankName;
            ID = tankID;
        }

    }
}
