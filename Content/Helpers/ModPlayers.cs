using NaturiumMod.Content.Items.PostHardmode.Weapons;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Weapons;

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
    public class LeodrakePlayer : ModPlayer
    {
        public bool hasLeodrakeMedallion;

        public override void ResetEffects()
        {
            hasLeodrakeMedallion = false;
        }

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {
            if (hasLeodrakeMedallion && item.type == ModContent.ItemType<LeodrakesLeafstorm>())
            {
                reduce -= 0.25f;
            }
        }

    }
    public class TabletPlayer : ModPlayer
    {
        public bool apophisBoost;
        public override void ResetEffects()
        {
            apophisBoost = false;
        }
        public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
        {
            var tag = item.GetGlobalItem<WeaponTag>();

            if (apophisBoost && tag.isApophis)
                modifiers.SourceDamage *= 1.15f;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            var tag = proj.GetGlobalProjectile<WeaponTagProj>();

            if (apophisBoost && tag.isApophis)
                modifiers.SourceDamage *= 1.15f;
        }
    }
}