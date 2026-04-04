using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NaturiumMod.Content.Items.PreHardmode.Shark
{
    using global::NaturiumMod.Content.Helpers;
    using global::NaturiumMod.Content.Items.PreHardmode.Materials;
    using Microsoft.Xna.Framework;
    using Terraria;
    using Terraria.Audio;
    using Terraria.ID;
    using Terraria.ModLoader;

    namespace NaturiumMod.Content.Items.PreHardmode.Shark
    {
        public class WindUpShark : ModItem
        {
            public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/WindUpShark";

            public override void SetDefaults()
            {
                Item.width = 42;
                Item.height = 22;

                Item.useStyle = ItemUseStyleID.Shoot;
                Item.useAnimation = 30;
                Item.useTime = 30;
                Item.autoReuse = false;

                Item.channel = true;

                Item.DamageType = DamageClass.Ranged;
                Item.damage = 20;
                Item.knockBack = 2f;

                Item.shootSpeed = 12f;
                Item.shoot = ModContent.ProjectileType<SharkFinProj>();

                Item.noMelee = true;
                Item.useAmmo = AmmoID.None;

                Item.value = Item.buyPrice(silver: 70);
                Item.rare = ItemRarityID.Green;
            }
            public override void AddRecipes()
            {
                Recipe recipe = CreateRecipe();
                recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<NaturiumBar>(), 10),
                new(ModContent.ItemType<SharkFinBladesDamaged>(), 9),
                new(ItemID.SharkFin, 3)
                ], TileID.Anvils);
                recipe.Register();
            }
            private float lastValidRotation = 0f;

            public override bool CanUseItem(Player player)
            {
                return true; // allow charging
            }  
            public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
            {
                return false;
            }


            public override void HoldItem(Player player)
            {
                var modPlayer = player.GetModPlayer<WindUpSharkPlayer>();

                // Rotate weapon toward mouse
                Vector2 dir = Main.MouseWorld - player.Center;
                player.itemRotation = dir.ToRotation();

                if (player.channel)
                {
                    // Keep item held
                    player.itemTime = 2;
                    player.itemAnimation = 2;

                    // Charging
                    modPlayer.charge++;

                    if (!modPlayer.fullyCharged && modPlayer.charge >= WindUpSharkPlayer.ChargeTime)
                    {
                        modPlayer.fullyCharged = true;
                        SoundEngine.PlaySound(SoundID.Unlock, player.Center);
                    }
                }

                else
                {
                    // Released
                    if (modPlayer.fullyCharged)
                    {
                        // Fully charged → fire 3-shot burst
                        Projectile.NewProjectile(
                            Item.GetSource_FromThis(),
                            player.Center,
                            Vector2.Zero,
                            ModContent.ProjectileType<WindUpSharkBurst>(),
                            Item.damage,
                            Item.knockBack,
                            player.whoAmI
                        );
                    }
                    else if (modPlayer.charge > 10) // prevent accidental taps
                    {
                        // Released early → fire 1 projectile

                        Projectile.NewProjectile(
                            Item.GetSource_FromThis(),
                            player.Center,
                            dir * Item.shootSpeed,
                            ModContent.ProjectileType<SharkFinProj>(),
                            Item.damage,
                            Item.knockBack,
                            player.whoAmI
                        );

                        // Play firing sound
                        SoundEngine.PlaySound(SoundID.Item40, player.Center);
                    }

                    modPlayer.ResetCharge();
                }

                // If mouse is off-screen, freeze rotation
                if (!Main.gameMenu && !Main.instance.IsActive)
                {
                    // Keep last rotation
                    player.itemRotation = lastValidRotation;
                }
                else
                {
                    // Normal rotation
                    float rot = dir.ToRotation();
                    player.itemRotation = rot;
                    lastValidRotation = rot;
                }

                // Force correct facing direction
                player.direction = (dir.X > 0f) ? 1 : -1;

                // Fix sprite flipping when facing left
                if (player.direction == -1)
                {
                    player.itemRotation += MathHelper.Pi;
                }


            }

        }
        public class WindUpSharkPlayer : ModPlayer
        {
            public const int ChargeTime = 60; // 1 second

            public int charge = 0;
            public bool fullyCharged = false;

            public void ResetCharge()
            {
                charge = 0;
                fullyCharged = false;
            }
        }
        public class WindUpSharkBurst : ModProjectile
        {
            public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/Blank";

            private int shotCount = 0;

            public override void SetDefaults()
            {
                Projectile.width = 1;
                Projectile.height = 1;
                Projectile.timeLeft = 40;
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.hide = true;
            }

            public override void AI()
            {
                Player player = Main.player[Projectile.owner];

                if (Projectile.ai[0] % 10 == 0 && shotCount < 3)
                {
                    Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center);
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        player.Center,
                        dir * 12f,
                        ModContent.ProjectileType<SharkFinProj>(),
                        Projectile.damage,
                        Projectile.knockBack,
                        player.whoAmI
                    );
                    // 🔊 Play firing sound for each shot
                    SoundEngine.PlaySound(SoundID.Item40, player.Center);
                    shotCount++;
                }

                Projectile.ai[0]++;
            }
        }

        public class SharkFinProj : ModProjectile
        {
            public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SharkFinProj";

            public override void SetDefaults()
            {
                Projectile.width = 10;
                Projectile.height = 10;

                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.penetrate = 2; // pierces 2 enemies
                Projectile.DamageType = DamageClass.Ranged;

                Projectile.aiStyle = ProjAIStyleID.Arrow;
                AIType = ProjectileID.Bullet;

                Projectile.ignoreWater = false;
                Projectile.tileCollide = true;
            }

            public override void AI()
            {
                // Small water trail
                if (Main.rand.NextBool(4))
                {
                    Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Water);
                    d.velocity *= 0.2f;
                }
            }
        }

    }
}
