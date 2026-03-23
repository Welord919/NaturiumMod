using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB
{
    public class DarkMagicianStaffWeapon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/MagiciansRod";

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

            Item.shoot = ModContent.ProjectileType<DarkMagicianStaffWeaponProj>();
            Item.shootSpeed = 0f;

            Item.DamageType = DamageClass.Magic;
            Item.damage = 25;
            Item.knockBack = 2f;

            Item.mana = 1; // mana cost per burst

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 1);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<DarkMagician>(), 1),
            new(ModContent.ItemType<DarkEssence>(), 15),
            new(ItemID.Hellstone, 7),
            new(ItemID.AquaScepter, 1)
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }

    // ============================================================
    // STAFF PROJECTILE (CHARGING + BURST FIRING)
    // ============================================================

    public class DarkMagicianStaffWeaponProj : ModProjectile
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
                // Mana cost per tier
                int manaCost = tier switch
                {
                    1 => 10,
                    2 => 20,
                    3 => 35,
                    _ => 10
                };

                // If not enough mana, downgrade tier
                while (tier > 1 && player.statMana < manaCost)
                {
                    tier--;
                    manaCost = tier switch
                    {
                        1 => 10,
                        2 => 20,
                        _ => 10
                    };
                }

                // If STILL not enough mana, cancel firing
                if (player.statMana < manaCost)
                {
                    // Optional: play fail sound
                    SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);
                    Projectile.Kill();
                    return;
                }

                // Consume mana
                player.statMana -= manaCost;
                player.manaRegenDelay = 60;

                // Fire the burst
                FireBurst(player);

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
}