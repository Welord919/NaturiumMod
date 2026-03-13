using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.General.Projectiles;
using NaturiumMod.Content.Items.PostHardmode.Weapons;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Helpers
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
