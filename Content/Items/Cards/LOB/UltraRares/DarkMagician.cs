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
    public class DarkMagician : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/darkmagi";

        public override void SetStaticDefaults()
        {
            Item.staff[Item.type] = true; // Hold like Diamond Staff
        }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true;

            Item.shoot = ModContent.ProjectileType<DarkMagicianStaff>();
            Item.shootSpeed = 0f;

            Item.DamageType = ModContent.GetInstance<CardDamage>();
            Item.damage = 50;
            Item.knockBack = 2f;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 1);

            Item.consumable = false;
            Item.maxStack = 999;
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

                // Apply summoning sickness (30 frames)
                player.AddBuff(ModContent.BuffType<SummoningSickness>(), 30);

                // Consume 1 card
                if (player.HeldItem.stack > 0)
                {
                    player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
                    player.HeldItem.stack--;
                    if (player.HeldItem.stack <= 0)
                        player.HeldItem.TurnToAir();
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
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
        }

        public override void ModifyDamageHitbox(ref Rectangle hitbox)
        {
            float s = Projectile.scale;

            int newWidth = (int)(hitbox.Width * s);
            int newHeight = (int)(hitbox.Height * s);

            hitbox = new Rectangle(
                (int)(Projectile.Center.X - newWidth / 2),
                (int)(Projectile.Center.Y - newHeight / 2),
                newWidth,
                newHeight
            );
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