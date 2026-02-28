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
    public class ModPlayers : ModPlayer
    {
        public bool hasUltiBuild;

        public override void ResetEffects()
        {
            hasUltiBuild = false;
        }
    }
    public class ExteriosPlayer : ModPlayer
    {
        public bool hasExteriosMedallion;

        public override void ResetEffects()
        {
            hasExteriosMedallion = false;
        }
    }
    public class StarsteelPlayer : ModPlayer
    {
        public int starsteelCooldown;

        public override void ResetEffects()
        {
            if (starsteelCooldown > 0)
                starsteelCooldown--;
        }
    }
    public class StarsteelGunPlayer : ModPlayer
    {
        public int starburstCooldown;
        public int burstShots;
        public int fireTimer;

        public override void ResetEffects()
        {
            if (starburstCooldown > 0) starburstCooldown--;
            if (fireTimer > 0) fireTimer--;
        }
    }

}