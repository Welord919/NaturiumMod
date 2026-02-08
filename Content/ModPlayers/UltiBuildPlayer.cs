using NaturiumMod.Content.Items.PostHardmode.Weapons;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.PreHardmode.Consumables;

namespace NaturiumMod.Content.ModPlayers
{
    public class UltiBuildPlayer : ModPlayer
    {
            public bool hasUltiBuild;

            public override void ResetEffects()
            {
                hasUltiBuild = false;
            }
    }
}
