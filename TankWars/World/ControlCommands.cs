using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankWars
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ControlCommands
    {
        [JsonProperty(PropertyName = "moving")]
        private string direction;

        [JsonProperty(PropertyName = "fire")]
        private string fire;

        [JsonProperty(PropertyName = "tdir")]
        private Vector2D aiming = new Vector2D(0, -1);

        public ControlCommands()
        {
        }
    }
}
