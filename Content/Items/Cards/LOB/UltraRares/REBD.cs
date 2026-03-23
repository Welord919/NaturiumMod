using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using StructureHelper.Content.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.ID;
using Terraria.ModLoader;
using static AssGen.Assets;

namespace NaturiumMod.Content.Items.Cards.LOB.SuperRares
{
    public class REBD : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/rebd";
        private int burstsFired = 0;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
            Item.damage = 50;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.knockBack = 3.5f;
            Item.shoot = ProjectileID.None;
            Item.shootSpeed = 10f;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 1);
            Item.autoReuse = false;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
                return false;

            return true;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);

            burstsFired++;
            if (burstsFired >= 3)
            {
                burstsFired = 0;
                Item.stack--;
                if (Item.stack <= 0)
                    Item.TurnToAir();
            }

            // Spawn 3 fireballs
            for (int i = 0; i < 3; i++)
            {
                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<RedEyesFireball>(),
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI,
                    i // index for spread
                );
            }

            return true;
        }
    }
    public class REBDTag : GlobalProjectile
    {
        public bool isREBD;

        public override bool InstancePerEntity => true;
    }

    public class RedEyesFireball : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/RedEyesFireball";

        private Vector2 targetPos;
        private bool initialized = false;

        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<REBDProjTag>().isREBD = true;

            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 180;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
            Projectile.ownerHitCheck = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];

            if (!initialized)
            {
                initialized = true;

                targetPos = Main.MouseWorld;

                int index = (int)Projectile.ai[0]; // 0,1,2
                float spread = MathHelper.ToRadians(40f);

                // Flip arc based on facing direction
                float angle = MathHelper.Lerp(-spread, spread, index / 2f) * owner.direction;

                // Spawn offset on facing side
                Vector2 offset = new Vector2(120 * owner.direction, 0).RotatedBy(angle);
                Projectile.Center = owner.Center + offset;

                // Initial outward velocity
                Projectile.velocity = offset.SafeNormalize(Vector2.UnitX * owner.direction) * 6f;
            }

            // Curve toward cursor
            Vector2 toTarget = (targetPos - Projectile.Center).SafeNormalize(Vector2.Zero);
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, toTarget * 14f, 0.05f);

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
                Main.dust[dust].velocity *= 0.3f;
            }

            if (Vector2.Distance(Projectile.Center, targetPos) < 20f)
                Explode();
        }

        private void Explode()
        {
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.RedTorch);
                Main.dust[dust].velocity *= 2f;
            }

            Projectile.NewProjectile(
                Projectile.GetSource_Death(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<RedEyesExplosion>(),
                Projectile.damage *2,
                Projectile.knockBack,
                Projectile.owner
            );

            Projectile.Kill();
        }

    }

    public class RedEyesExplosion : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.InfernoFriendlyBlast;

        public override void SetDefaults()
        {
            Projectile.GetGlobalProjectile<REBDProjTag>().isREBD = true;

            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.ownerHitCheck = false;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            if (Projectile.timeLeft == 2)
            {
                SoundEngine.PlaySound(SoundID.Item14, Projectile.Center);
            }
        }
    }

}
