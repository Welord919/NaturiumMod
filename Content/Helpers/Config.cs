using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.Config;

namespace NaturiumMod.Content.Helpers
{
    public class NaturiumConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [DefaultValue(true)]
        public bool Structures { get; set; }

        [DefaultValue(true)]
        public bool CardDrops { get; set; }
    }

}


