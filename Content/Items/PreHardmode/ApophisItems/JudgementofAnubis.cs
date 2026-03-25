using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.MillenniumItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.ApophisItems;
    public class JudgementofAnubis : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/JudgementofAnubis";

        public override void SetDefaults()
        {
            Item.damage = 20;
            Item.DamageType = DamageClass.Magic; // or Ranged if you prefer
            Item.mana = 10;
            Item.width = 32;
            Item.height = 32;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.noMelee = true;
            Item.knockBack = 2f;
            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<AnubisSentry>();
            Item.shootSpeed = 0f;
            Item.sentry = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Place the sentry at the mouse cursor
            player.UpdateMaxTurrets();
            Projectile.NewProjectile(
                source,
                Main.MouseWorld,
                Vector2.Zero,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            return false;
        }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<MillenniumPiece>(), 10),
            new(ItemID.SlimeStaff, 1),
            new(ItemID.Amber, 4)
        ], TileID.Anvils);
        recipe.Register();
    }
}
    public class AnubisSentry : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/AnubisSentry";

        public ref float ShootTimer => ref Projectile.ai[0];

        public bool JustSpawned
        {
            get => Projectile.localAI[0] == 0;
            set => Projectile.localAI[0] = value ? 0 : 1;
        }

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 4;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 35;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.sentry = true;
            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.tileCollide = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override void AI()
        {
            const int ShootFrequency = 45;
            const int TargetingRange = 50 * 16;
            const float FireVelocity = 10f;

            if (JustSpawned)
            {
                JustSpawned = false;
                ShootTimer = ShootFrequency * 1.5f;

                SoundEngine.PlaySound(SoundID.Item46, Projectile.position);    
            }

            Projectile.velocity.X = 0f;

            Projectile.velocity.Y += 0.4f;
            if (Projectile.velocity.Y > 10f)
                Projectile.velocity.Y = 10f;

            if (IsOnGround())
                Projectile.velocity.Y = 0f;

            float closestTargetDistance = TargetingRange;
            NPC targetNPC = null;

            if (Projectile.OwnerMinionAttackTargetNPC != null)
                TryTargeting(Projectile.OwnerMinionAttackTargetNPC, ref closestTargetDistance, ref targetNPC);

            if (targetNPC == null)
            {
                foreach (var npc in Main.ActiveNPCs)
                    TryTargeting(npc, ref closestTargetDistance, ref targetNPC);
            }

            if (targetNPC != null)
            {
                if (ShootTimer <= 0)
                {
                    ShootTimer = ShootFrequency;

                    SoundEngine.PlaySound(SoundID.Item102 with { Volume = 0.4f }, Projectile.Center);

                    if (Main.myPlayer == Projectile.owner)
                    {
                        Vector2 shootDirection = (targetNPC.Center - Projectile.Center).SafeNormalize(Vector2.UnitX);
                        Vector2 shootVelocity = shootDirection * FireVelocity;

                        int type = ModContent.ProjectileType<ApophisProj>();

                        Projectile.NewProjectile(
                            Projectile.GetSource_FromThis(),
                            new Vector2(Projectile.Center.X - 4f, Projectile.Center.Y),
                            shootVelocity,
                            type,
                            Projectile.damage,
                            3,
                            Projectile.owner
                        );
                    }
                }
            }

            ShootTimer--;

            if (ShootTimer > ShootFrequency)
            {
                Projectile.frame = 0;
            }
            else if (targetNPC == null)
            {
                if (++Projectile.frameCounter >= 60)
                    Projectile.frameCounter = 0;

                Projectile.frame = Projectile.frameCounter < 30 ? 1 : 2;
            }
            else
            {
                Projectile.frame = 3;
            }
        }

        private bool IsOnGround()
        {
            int tileX = (int)(Projectile.Center.X / 16f);
            int tileY = (int)((Projectile.Bottom.Y + 1f) / 16f);

            Tile tile = Framing.GetTileSafely(tileX, tileY);
            return tile.HasTile && Main.tileSolid[tile.TileType];
        }

        private void TryTargeting(NPC npc, ref float closestTargetDistance, ref NPC targetNPC)
        {
            if (npc.CanBeChasedBy(this))
            {
                float distanceToTargetNPC = Vector2.Distance(Projectile.Center, npc.Center);

                if (distanceToTargetNPC < closestTargetDistance)
                {
                    closestTargetDistance = distanceToTargetNPC;
                    targetNPC = npc;
                }
            }
        }
        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(Projectile.Center + speed * 50, DustID.BlueCrystalShard, speed * -5, Scale: 1.5f);
                d.noGravity = true;
            }
        }
    }
