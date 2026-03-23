using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion
{
    public class CharubinMace : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/CharubinMace";

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.knockBack = 6f;
            Item.damage = 40;
            Item.DamageType = DamageClass.MeleeNoSpeed; // REQUIRED FOR FLAILS
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.channel = true; // REQUIRED
            Item.shoot = ModContent.ProjectileType<CharubinMaceProj>();
            Item.shootSpeed = 12f; // flail extension speed
            Item.rare = ItemRarityID.Orange;
        }
    }

    public class CharubinMaceProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/CharubinMaceProj";

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.MeleeNoSpeed;
            Projectile.aiStyle = ProjAIStyleID.Flail; // REAL flail physics
            Projectile.tileCollide = true;
            Projectile.noDropItem = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // 10% chance to apply egg shell
            if (Main.rand.NextFloat() < 0.10f)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    target.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<EggShellStickProj>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }
        }

        // Draw chain
        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D chainTex = ModContent.Request<Texture2D>("Terraria/Images/Projectile_13").Value;

            Vector2 position = Projectile.Center;
            Vector2 mountedCenter = Main.player[Projectile.owner].MountedCenter;
            Vector2 distToProj = mountedCenter - position;
            float projRotation = distToProj.ToRotation() - MathHelper.PiOver2;
            float distance = distToProj.Length();

            while (distance > 20f)
            {
                distToProj.Normalize();
                distToProj *= 20f;
                position += distToProj;
                distToProj = mountedCenter - position;
                distance = distToProj.Length();

                Main.EntitySpriteDraw(chainTex, position - Main.screenPosition,
                    null, lightColor, projRotation, chainTex.Size() / 2f, 1f, SpriteEffects.None, 0);
            }

            return true;
        }
    }

    public class EggShellStickProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/EggShellStickProj";

        private NPC stuckTarget;
        private int stuckTimer = 0;
        private int damageCooldown = 0;

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.timeLeft = 180; // safety
        }

        public override void AI()
        {
            if (stuckTarget != null && stuckTarget.active)
            {
                stuckTimer++;

                // Stick to the enemy's center
                Projectile.Center = stuckTarget.Center;

                // Spin faster over time
                Projectile.rotation += 0.4f + stuckTimer * 0.01f;

                // Damage tick every 0.5 seconds
                damageCooldown++;
                if (damageCooldown >= 30)
                {
                    damageCooldown = 0;

                    stuckTarget.SimpleStrikeNPC(Projectile.damage, 0);

                }

                // After 2 seconds (120 frames), explode
                if (stuckTimer >= 120)
                {
                    Explode();
                }
            }
            else
            {
                Projectile.rotation += 0.3f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Stick to this NPC
            stuckTarget = target;
            Projectile.velocity = Vector2.Zero;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        private void Explode()
        {
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                ProjectileID.GrenadeIII,
                Projectile.damage,
                4f,
                Projectile.owner
            );

            Projectile.Kill();
        }
    }
}