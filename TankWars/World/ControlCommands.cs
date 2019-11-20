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
        public string direction;

        [JsonProperty(PropertyName = "fire")]
        public string fire;

        [JsonProperty(PropertyName = "tdir")]
        public Vector2D aiming = new Vector2D(0, -1);

        public ControlCommands()
        {
        }
    }
}
