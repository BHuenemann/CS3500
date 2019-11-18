using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Tank
    {

        [JsonProperty(PropertyName = "tank")]
        public int ID { get; private set; }
        
        [JsonProperty(PropertyName = "loc")]
        public Vector2D location { get; private set; }

        [JsonProperty(PropertyName = "bdir")]
        public Vector2D orientation { get; private set; }

        [JsonProperty(PropertyName = "tdir")]
        private Vector2D aiming = new Vector2D(0, -1);

        [JsonProperty(PropertyName = "name")]
        private string name;

        [JsonProperty(PropertyName = "hp")]
        private int hitPoints = Constants.MaxHP;

        [JsonProperty(PropertyName = "score")]
        private int score = 0;

        [JsonProperty(PropertyName = "died")]
        private bool died = false;

        [JsonProperty(PropertyName = "dc")]
        private bool disconnected = false;

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
