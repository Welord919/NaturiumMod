using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.SuperRares
{
    public class Exodia : BaseCardSuper
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Exodia";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = 1500;
        }

        private bool Has(Player p, int type)
        {
            for (int i = 0; i < p.inventory.Length; i++)
                if (p.inventory[i].type == type)
                    return true;
            return false;
        }

        private void Consume(Player p, int type)
        {
            for (int i = 0; i < p.inventory.Length; i++)
            {
                if (p.inventory[i].type == type)
                {
                    p.inventory[i].stack--;
                    if (p.inventory[i].stack <= 0)
                        p.inventory[i].TurnToAir();
                    return;
                }
            }
        }

        public override bool? UseItem(Player player)
        {
            int la = ModContent.ItemType<LeftArm>();
            int ra = ModContent.ItemType<RightArm>();
            int ll = ModContent.ItemType<LeftLeg>();
            int rl = ModContent.ItemType<RightLeg>();

            bool ok =
                Has(player, la) &&
                Has(player, ra) &&
                Has(player, ll) &&
                Has(player, rl);

            if (!ok)
                return false;

            Consume(player, la);
            Consume(player, ra);
            Consume(player, ll);
            Consume(player, rl);

            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 60 * 20);

            Projectile.NewProjectile(
                Item.GetSource_FromThis(),
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<ExodiaSummonSequence>(),
                0,
                0.33f,
                player.whoAmI
            );

            return true;
        }

    }
    public class ExodiaSummonSequence : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/Blank";
        private int beamDelay = 0;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 300;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        private bool spawnedLeftArm;
        private bool spawnedRightArm;
        private bool spawnedLeftLeg;
        private bool spawnedRightLeg;
        private bool spawnedHead;

        private Vector2 lastLeftArmOffset;
        private Vector2 lastRightArmOffset;

        public override void AI()
        {
            Player p = Main.player[Projectile.owner];
            float t = 300 - Projectile.timeLeft;

            if (t == 1)
                SoundEngine.PlaySound(SoundID.DD2_EtherianPortalOpen, p.Center);

            if (t == 30 && !spawnedLeftArm)
            {
                SpawnPiece(p, "LeftArm");
                spawnedLeftArm = true;
            }

            if (t == 60 && !spawnedRightArm)
            {
                SpawnPiece(p, "RightArm");
                spawnedRightArm = true;
            }

            if (t == 90 && !spawnedLeftLeg)
            {
                SpawnPiece(p, "LeftLeg");
                spawnedLeftLeg = true;
            }

            if (t == 120 && !spawnedRightLeg)
            {
                SpawnPiece(p, "RightLeg");
                spawnedRightLeg = true;
            }

            if (t == 150 && !spawnedHead)
            {
                SpawnPiece(p, "Head");
                spawnedHead = true;
            }

            if (t == 200)
                SoundEngine.PlaySound(SoundID.Roar, p.Center);

            if (t == 220)
            {
                
                KillAllPieces();
                SpawnFinalHands(p);
                beamDelay = 60; // delay in frames (20 = 1/3 second)

            }
            if (beamDelay > 0)
            {
                beamDelay--;

                // When delay finishes, fire beams
                if (beamDelay == 0)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        p.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<ExodiaFinalBlast>(),
                        Projectile.damage,
                        10f,
                        p.whoAmI
                    );
                }
            }

        }

        private void SpawnPiece(Player p, string piece)
        {
            Vector2 offset = Vector2.Zero;
            int texIndex = 0;

            if (piece == "LeftArm") { offset = new Vector2(-80, -60); texIndex = 1; lastLeftArmOffset = offset; }
            if (piece == "LeftLeg") { offset = new Vector2(-60, 40); texIndex = 2; }
            if (piece == "RightArm") { offset = new Vector2(80, -60); texIndex = 3; lastRightArmOffset = offset; }
            if (piece == "RightLeg") { offset = new Vector2(60, 40); texIndex = 4; }
            if (piece == "Head") { offset = new Vector2(0, -80); texIndex = 0; }

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                p.Center,
                Vector2.Zero,
                ModContent.ProjectileType<ExodiaPieceVisual>(),
                0,
                0f,
                p.whoAmI,
                offset.X,
                offset.Y,
                texIndex
            );
        }

        private void KillAllPieces()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile pr = Main.projectile[i];
                if (pr.active && pr.type == ModContent.ProjectileType<ExodiaPieceVisual>())
                    pr.Kill();
            }
        }

        private void SpawnFinalHands(Player p)
        {
            // Bottom hand (starts at old LeftArm world position)
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                p.Center + lastLeftArmOffset, // FIXED
                Vector2.Zero,
                ModContent.ProjectileType<ExodiaFinalHand>(),
                0,
                0f,
                p.whoAmI,
                lastLeftArmOffset.X,
                lastLeftArmOffset.Y,
                0 // bottom
            );

            // Top hand (starts at old RightArm world position)
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                p.Center + lastRightArmOffset, // FIXED
                Vector2.Zero,
                ModContent.ProjectileType<ExodiaFinalHand>(),
                0,
                0.33f,
                p.whoAmI,
                lastRightArmOffset.X,
                lastRightArmOffset.Y,
                1 // top
            );
        }

    }
    public class ExodiaPieceVisual : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/ExodiaHeadProj";

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.timeLeft = 9999;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player p = Main.player[Projectile.owner];

            Vector2 offset = new Vector2(Projectile.ai[0], Projectile.ai[1]);
            Projectile.Center = p.Center + offset + new Vector2(0, (float)Math.Sin(Main.GameUpdateCount / 20f) * 4f);

            Projectile.alpha -= 10;
            if (Projectile.alpha < 0) Projectile.alpha = 0;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            string texName = "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/ExodiaHeadProj";
            SpriteEffects fx = SpriteEffects.None;

            if (Projectile.ai[2] == 1)
                texName = "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftArmProj";

            if (Projectile.ai[2] == 2)
                texName = "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftLegProj";

            if (Projectile.ai[2] == 3)
            {
                texName = "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftArmProj";
                fx = SpriteEffects.FlipHorizontally;
            }

            if (Projectile.ai[2] == 4)
            {
                texName = "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftLegProj";
                fx = SpriteEffects.FlipHorizontally;
            }

            Texture2D tex = ModContent.Request<Texture2D>(texName).Value;

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White * (1f - Projectile.alpha / 255f),
                0f,
                tex.Size() / 2f,
                0.3f,
                fx,
                0
            );

            return false;
        }
    }
    public class ExodiaFinalHand : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/FinalArm";

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.timeLeft = 60; // give extra time so rotation can finish
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.alpha = 0;
        }


        public override void OnSpawn(IEntitySource source)
        {
            // Store the world start position ONCE
            Projectile.localAI[0] = Projectile.Center.X;
            Projectile.localAI[1] = Projectile.Center.Y;
        }

        public override void AI()
        {
            Player p = Main.player[Projectile.owner];

            Vector2 startPos = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);

            bool isTop = Projectile.ai[2] == 1;
            Vector2 targetPos = p.Center + (isTop ? new Vector2(0, -60) : new Vector2(0, 60));

            // Rotation/movement still takes 50 frames
            float progress = MathHelper.Clamp((60f - Projectile.timeLeft) / 50f, 0f, 1f);

            Projectile.Center = Vector2.Lerp(startPos, targetPos, progress);

            if (!isTop)
                Projectile.rotation = MathHelper.Lerp(0f, -MathHelper.Pi, progress);
            else
                Projectile.rotation = 0f;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;

            SpriteEffects fx = Projectile.ai[2] == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                tex.Size() / 2f,
                0.3f,
                fx,
                0
            );

            return false;
        }

    }
    public class ExodiaFinalBlast : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/Blank";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 10;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player p = Main.player[Projectile.owner];

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                p.Center,
                Vector2.Zero,
                ModContent.ProjectileType<ExodiaBlast>(),
                300,
                0.3f,
                p.whoAmI,
                -MathHelper.PiOver2
            );

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                p.Center,
                Vector2.Zero,
                ModContent.ProjectileType<ExodiaBlast>(),
                300,
                0.3f,
                p.whoAmI,
                MathHelper.PiOver2
            );

            Projectile.Kill();
        }
    }


    public class ExodiaBlast : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/ExodiaBlast";

        public override void SetDefaults()
        {
            Projectile.width = 2000;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 180; // 3 seconds total

            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player p = Main.player[Projectile.owner];

            float t = 180 - Projectile.timeLeft;

            float rot;

            if (t < 30)
            {
                rot = Projectile.ai[0];
            }
            else if (t < 150)
            {
                float sweepProgress = (t - 30f) / 120f;
                rot = Projectile.ai[0] + MathHelper.TwoPi * sweepProgress;
            }
            else
            {
                rot = Projectile.ai[0] + MathHelper.TwoPi;
            }

            Vector2 dir = rot.ToRotationVector2();

            Projectile.Center = p.Center + dir * 40f;

            Projectile.velocity = dir;

            Projectile.rotation = dir.ToRotation();

            Lighting.AddLight(Projectile.Center, 1f, 0.9f, 0.3f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(0, tex.Height / 2f);

            Main.EntitySpriteDraw(
                tex,
                Projectile.Center - Main.screenPosition,
                null,
                Color.White,
                Projectile.rotation,
                origin,
                0.3f,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float p = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Projectile.Center,
                Projectile.Center + Projectile.velocity * 3000f,
                80f,
                ref p
            );
        }
    }
}
