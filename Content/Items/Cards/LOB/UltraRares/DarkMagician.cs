using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.UltraRares
{
    public class DarkMagician : MRUltra
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/darkmagi";

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true; // Hold like Diamond Staff
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.channel = true;
            Item.shoot = ModContent.ProjectileType<DarkMagicianStaff>();
            Item.damage = 50;
            Item.knockBack = 2f;
        }

        public override bool CanUseItem(Player player)
        {
            // Prevent use during summoning sickness
            return !player.HasBuff(ModContent.BuffType<SummoningSickness>());
        }

    }

    // ============================================================
    // STAFF PROJECTILE (CHARGING + BURST FIRING)
    // ============================================================

    public class DarkMagicianStaff : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/DMStaff";

        private int chargeTime = 0;
        private int tier = 0;

        private bool tier1Sound = false;
        private bool tier2Sound = false;
        private bool tier3Sound = false;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 999999;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            // Force held projectile pose
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // Aim direction
            Vector2 aim = (Main.MouseWorld - player.Center)
                .SafeNormalize(new Vector2(player.direction, 0f));

            float baseRot = aim.ToRotation();

            // Staff rotation
            Projectile.rotation = baseRot;

            // Attach staff to hand
            Projectile.Center = player.Center;

            // Flip player direction
            player.direction = Main.MouseWorld.X >= player.Center.X ? 1 : -1;

            // Arm rotation
            player.itemRotation = Projectile.rotation * player.direction;

            // Charge tiers
            chargeTime++;

            if (chargeTime < 40) tier = 1;
            else if (chargeTime < 80) tier = 2;
            else tier = 3;

            // Tier sounds
            if (tier == 1 && !tier1Sound)
            {
                tier1Sound = true;
                SoundEngine.PlaySound(SoundID.Item29, Projectile.Center);
            }
            if (tier == 2 && !tier2Sound)
            {
                tier2Sound = true;
                SoundEngine.PlaySound(SoundID.Item29, Projectile.Center);
            }
            if (tier == 3 && !tier3Sound)
            {
                tier3Sound = true;
                SoundEngine.PlaySound(SoundID.Item74, Projectile.Center);
            }

            // Visual charge effects
            if (tier == 1)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            else if (tier == 2)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
            else
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror);

            // Release
            if (!player.channel)
            {
                FireBurst(player);

                // Tier-based summoning sickness
                int sickness = tier switch
                {
                    1 => 10,  // base
                    2 => 30,  // mid
                    3 => 60,  // max
                    _ => 10
                };

                player.AddBuff(ModContent.BuffType<SummoningSickness>(), sickness);

                // Monster Reborn protection
                if (!CardUtils.TryApplyMonsterReborn(player, ModContent.ItemType<DarkMagician>()))
                {
                    // Consume 1 DM
                    if (player.HeldItem.type == ModContent.ItemType<DarkMagician>())
                    {
                        player.HeldItem.stack--;
                        if (player.HeldItem.stack <= 0)
                            player.HeldItem.TurnToAir();
                    }
                }

                Projectile.Kill();

            }

        }

        private void FireBurst(Player player)
        {
            int shots = 3;
            int delay = 6;

            for (int i = 0; i < shots; i++)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<DarkMagicDelayedShot>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    player.whoAmI,
                    tier,
                    i * delay
                );
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

            // HANDLE PIVOT (left side)
            Vector2 origin = new Vector2(20f, tex.Height / 2f);

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                1f,
                Projectile.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
                0
            );

            return false;
        }
    }

    // ============================================================
    // DELAYED SHOT (handles burst timing)
    // ============================================================

    public class DarkMagicDelayedShot : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.None;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.timeLeft = 200;
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }

        public override void AI()
        {
            int tier = (int)Projectile.ai[0];
            int delay = (int)Projectile.ai[1];

            if (Projectile.timeLeft == 200 - delay)
            {
                FireRealProjectile(tier);
                Projectile.Kill();
            }
        }

        private void FireRealProjectile(int tier)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 direction = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);

            float speed = 12f + tier * 2f;
            float scale = 1f + (tier - 1) * 0.5f;
            int damage = player.HeldItem.damage * tier;

            int proj = Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                player.Center,
                direction * speed,
                ModContent.ProjectileType<DarkMagicBolt>(),
                damage,
                player.HeldItem.knockBack,
                player.whoAmI,
                scale
            );

            Main.projectile[proj].scale = scale;
        }
    }

    // ============================================================
    // DARK MAGIC BOLT (scales automatically)
    // ============================================================

    public class DarkMagicBolt : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/DMagic";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();

            Projectile.penetrate = -1; // will be set on spawn
        }
        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            float s = Projectile.scale * 1.5f;

            // Center before resizing
            Vector2 center = hitbox.Center.ToVector2();

            hitbox.Width = (int)(Projectile.width * s);
            hitbox.Height = (int)(Projectile.height * s);

            // Re-center after resizing
            hitbox.X = (int)(center.X - hitbox.Width / 2f);
            hitbox.Y = (int)(center.Y - hitbox.Height / 2f);
        }
        public override void OnSpawn(IEntitySource source)
        {
            int tier = (int)Projectile.ai[0];

            Projectile.penetrate = tier switch
            {
                1 => 5,
                2 => 4,
                3 => 2,
                _ => 1
            };
        }

        public override void AI()
        {
            Projectile.rotation += 0.25f;

            if (Projectile.scale < 1.3f)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            else if (Projectile.scale < 1.8f)
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
            else
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.MagicMirror);
        }
    }
}