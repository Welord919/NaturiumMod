using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion
{
    public class DarkfireDragon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/DarkfireDragon";

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 20;
            Item.useTime = 20;

            Item.noUseGraphic = false;
            Item.noMelee = true;

            Item.DamageType = ModContent.GetInstance<CardDamage>();
            Item.damage = 100;
            Item.knockBack = 3f;

            // Item shoots ONLY the dragon head
            Item.shoot = ModContent.ProjectileType<DarkFireDragonHead>();
            Item.shootSpeed = 8f;

            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(silver: 80);

            Item.consumable = true;
            Item.maxStack = 999;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(ModContent.BuffType<SummoningSickness>());
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Consume card
            Item.stack--;

            // Apply short cooldown
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 30);

            return true; // allow default shooting (dragon head)
        }
    }


    // ============================================================
    // ORB — Spawns above the dragon, fires PetiteDragonShot
    // ============================================================

    public class DarkFireOrb : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/DarkfireOrb";

        public override void SetDefaults()
        {
            Projectile.width = 100;   // ⭐ 25% larger hitbox
            Projectile.height = 100;
            Projectile.friendly = true; // ⭐ orb can hit enemies
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            Projectile.tileCollide = false;
            Projectile.timeLeft = 180;
            Projectile.alpha = 0;
        }

        public override void AI()
        {
            Projectile.rotation += 0.35f;
            Projectile.velocity *= 0.9f;

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
            d.noGravity = true;
            d.scale = 1.2f;

            if (Projectile.timeLeft % 20 == 0)
            {
                int count = 8;
                float speed = 10f;

                for (int i = 0; i < count; i++)
                {
                    float angle = MathHelper.TwoPi * (i / (float)count);
                    Vector2 dir = angle.ToRotationVector2();

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        dir * speed,
                        ModContent.ProjectileType<PetiteDragonShot>(),
                        Projectile.damage / 2,
                        0f,
                        Projectile.owner
                    );
                }
            }

            if (Projectile.timeLeft <= 60)
            {
                float fade = 1f - (Projectile.timeLeft / 60f);
                Projectile.alpha = (int)(fade * 255f);
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = new Vector2(tex.Width / 2f, tex.Height / 2f);

            Main.EntitySpriteDraw(
                tex,
                drawPos,
                null,
                lightColor * (1f - Projectile.alpha / 255f),
                Projectile.rotation,
                origin,
                1.25f, // ⭐ 25% scale
                SpriteEffects.None,
                0
            );

            return false;
        }
    }


    // ============================================================
    // DRAGON HEAD — Spawns the orb on FIRST FRAME
    // ============================================================

    public class DarkFireDragonHead : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/DarkfireDragonHead";

        private float waveCounter;
        public List<Vector2> oldPositions = new List<Vector2>();

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.damage = 100;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
            Projectile.timeLeft = 200;
        }

        public override void OnSpawn(IEntitySource source)
        {
            // Spawn segments (your existing code unchanged)
            int headID = Projectile.whoAmI;
            int segments = 20;
            int spacing = 4;

            for (int i = 0; i < segments; i++)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<DarkFireDragonBody>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner,
                    headID,
                    (i + 1) * spacing
                );
            }

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<DarkFireDragonTail>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner,
                headID,
                (segments + 1) * spacing
            );

            int requiredHistory = (segments + 2) * spacing + 10;
            oldPositions.Clear();
            for (int i = 0; i < requiredHistory; i++)
                oldPositions.Add(Projectile.Center);
        }

        public override void AI()
        {
            // ⭐ Spawn orb on FIRST FRAME
            if (Projectile.localAI[0] == 0f)
            {
                Projectile.localAI[0] = 1f;

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<DarkFireOrb>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    Projectile.owner
                );
            }

            // Store head position history
            oldPositions.Insert(0, Projectile.Center);
            if (oldPositions.Count > 600)
                oldPositions.RemoveAt(oldPositions.Count - 1);

            // Serpentine sway
            waveCounter += 0.12f;
            float amplitude = 70f;
            float strength = 0.18f;

            Vector2 forward = Projectile.velocity.SafeNormalize(Vector2.UnitX);
            Vector2 perp = forward.RotatedBy(MathHelper.PiOver2);

            Projectile.position += perp * (float)Math.Sin(waveCounter) * amplitude * strength;

            if (Projectile.velocity.LengthSquared() > 0.001f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // Fire dust
            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
            d.noGravity = true;
            d.scale = 1.2f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 180);
        }
    }


    // ============================================================
    // BODY SEGMENT
    // ============================================================

    public class DarkFireDragonBody : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/DarkfireDragonBody";

        public override void SetDefaults()
        {
            Projectile.width = 30;
            Projectile.height = 30;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
            Projectile.timeLeft = 200;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void AI()
        {
            int headID = (int)Projectile.ai[0];
            int histIndex = (int)Projectile.ai[1];

            if (headID < 0 || headID >= Main.maxProjectiles)
            {
                Projectile.Kill();
                return;
            }

            Projectile headProj = Main.projectile[headID];
            if (!headProj.active)
            {
                Projectile.Kill();
                return;
            }

            DarkFireDragonHead head = headProj.ModProjectile as DarkFireDragonHead;
            if (head == null)
            {
                Projectile.Kill();
                return;
            }

            // If the head hasn't accumulated enough history yet, place the segment at the head's center
            // and skip rotation until history is available. Do NOT kill the segment.
            if (head.oldPositions.Count <= histIndex)
            {
                Projectile.Center = headProj.Center;
                if (head.oldPositions.Count > 0)
                {
                    Vector2 nextPos = head.oldPositions[Math.Max(head.oldPositions.Count - 1, 0)];
                    Vector2 toNext = nextPos - Projectile.Center;
                    if (toNext.LengthSquared() > 0.001f)
                        Projectile.rotation = toNext.ToRotation() + MathHelper.PiOver2;
                }
                return;
            }

            // Follow the exact recorded path of the head
            Projectile.Center = head.oldPositions[histIndex];

            // Rotate toward the next point in the history for smooth orientation
            int nextIndex = Math.Max(histIndex - 1, 0);
            Vector2 nextPos2 = head.oldPositions[nextIndex];
            Vector2 toNext2 = nextPos2 - Projectile.Center;
            if (toNext2.LengthSquared() > 0.001f)
                Projectile.rotation = toNext2.ToRotation() + MathHelper.PiOver2;
        }
    }

    // ============================================================
    // TAIL SEGMENT
    // ============================================================

    public class DarkFireDragonTail : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/DarkfireDragonTail";

        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
            Projectile.timeLeft = 200;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 300);
        }

        public override void AI()
        {
            int headID = (int)Projectile.ai[0];
            int histIndex = (int)Projectile.ai[1];

            if (headID < 0 || headID >= Main.maxProjectiles)
            {
                Projectile.Kill();
                return;
            }

            Projectile headProj = Main.projectile[headID];
            if (!headProj.active)
            {
                Projectile.Kill();
                return;
            }

            DarkFireDragonHead head = headProj.ModProjectile as DarkFireDragonHead;
            if (head == null)
            {
                Projectile.Kill();
                return;
            }

            if (head.oldPositions.Count <= histIndex)
            {
                Projectile.Center = headProj.Center;
                if (head.oldPositions.Count > 0)
                {
                    Vector2 nextPos = head.oldPositions[Math.Max(head.oldPositions.Count - 1, 0)];
                    Vector2 toNext = nextPos - Projectile.Center;
                    if (toNext.LengthSquared() > 0.001f)
                        Projectile.rotation = toNext.ToRotation() + MathHelper.PiOver2;
                }
                return;
            }

            Projectile.Center = head.oldPositions[histIndex];

            int nextIndex = Math.Max(histIndex - 1, 0);
            Vector2 nextPos2 = head.oldPositions[nextIndex];
            Vector2 toNext2 = nextPos2 - Projectile.Center;
            if (toNext2.LengthSquared() > 0.001f)
                Projectile.rotation = toNext2.ToRotation() + MathHelper.PiOver2;
        }
    }
}