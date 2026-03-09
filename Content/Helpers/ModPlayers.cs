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
    public class LeodrakesManePlayer : ModPlayer
    {
        public bool leodrakeManeEquipped;
        private int leodrakeCooldown = 0;

        public override void ResetEffects()
        {
            leodrakeManeEquipped = false;
        }

        public override void PostUpdate()
        {
            if (leodrakeCooldown > 0)
                leodrakeCooldown--;
        }

        public override void OnHurt(Player.HurtInfo hurtInfo)
        {
            if (!leodrakeManeEquipped)
                return;

            if (leodrakeCooldown > 0)
                return;

            leodrakeCooldown = 60; // 1 second cooldown

            // Sound
            SoundEngine.PlaySound(SoundID.Roar with { Volume = 0.8f, Pitch = 0.3f }, Player.Center);

            // Fire 5 projectiles in a circle
            int amount = 5;
            float speed = 8f;

            for (int i = 0; i < amount; i++)
            {
                float angle = MathHelper.TwoPi * i / amount;
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;

                Projectile.NewProjectile(
                    Player.GetSource_OnHurt(Player),
                    Player.Center,
                    velocity,
                    ModContent.ProjectileType<LeodrakesManeProj>(),
                    20,       // damage
                    2f,       // knockback
                    Player.whoAmI
                );
            }
        }
    }
    public class MillenniumNecklacePlayer : ModPlayer
    {
        public bool MillenniumNecklaceEquipped;

        public override void ResetEffects()
        {
            MillenniumNecklaceEquipped = false;
        }

    }
}
