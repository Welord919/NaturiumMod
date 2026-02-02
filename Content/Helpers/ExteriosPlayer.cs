using NaturiumMod.Content.Items.PostHardmode.Weapons;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace NaturiumMod.Content.Helpers
{
    public class ExteriosPlayer : ModPlayer
    {
        public bool hasExteriosMedallion;

        public override void ResetEffects()
        {
            hasExteriosMedallion = false;
        }
    }

}
