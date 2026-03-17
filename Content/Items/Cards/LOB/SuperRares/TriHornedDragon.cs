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
using Terraria.ID;
using Terraria.ModLoader;
using static AssGen.Assets;

namespace NaturiumMod.Content.Items.Cards.LOB.SuperRares
{
    public class TriHornedDragon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/TriHornedDragon";
        private int burstsFired = 0;

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.noMelee = true;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
            Item.damage = 28;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.knockBack = 3.5f;
            Item.shoot = ModContent.ProjectileType<DragonHorn>();
            Item.shootSpeed = 10f;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 1);
            Item.autoReuse = false;
            Item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            // Also block if buff is active mid-cycle
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
                return false;

            return true;
        }

        public override bool? UseItem(Player player)
        {

            // Apply cooldown (20 frames)
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 20);

            // Count bursts
            burstsFired++;

            // After 3 bursts → consume 1 card
            if (burstsFired >= 3)
            {
                burstsFired = 0; // reset burst counter
                Item.stack--;    // consume exactly 1 stack

                if (Item.stack <= 0)
                    Item.TurnToAir();
            }

            // Fire 3 horns
            float baseSpeed = Item.shootSpeed == 0 ? 8f : Item.shootSpeed;
            float spread = MathHelper.ToRadians(10f);

            Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.Center);

            for (int i = 0; i < 3; i++)
            {
                float angle = MathHelper.Lerp(-spread, spread, i / 2f);
                Vector2 vel = direction.RotatedBy(angle) * baseSpeed;

                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    vel,
                    ModContent.ProjectileType<DragonHorn>(),
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI
                );
            }

            return true;
        }

    }
    public class BleedDebuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteDragonBuff";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;          // It's a debuff
            Main.buffNoSave[Type] = true;      // Doesn't persist on reload
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true; // Allows minions to benefit
        }
    }
    public class BleedDebuffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool hasBleed;

        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<BleedDebuff>()))
            {
                hasBleed = true;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (hasBleed)
            {
                // 1% max HP damage per second
                int percentDamage = (int)(npc.lifeMax * 0.01f);
                if (percentDamage < 1)
                    percentDamage = 1;

                // Terraria regen is per second * 2
                npc.lifeRegen -= percentDamage * 2;

                // Ensures the damage actually applies
                if (damage < percentDamage)
                    damage = percentDamage;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            // FIXED: check for BleedDebuff, not DecayDebuff
            if (!npc.HasBuff(ModContent.BuffType<BleedDebuff>()))
            {
                hasBleed = false;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (hasBleed)
            {
                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(
                        npc.position,
                        npc.width,
                        npc.height,
                        DustID.Blood,
                        0f,
                        1.5f,
                        100,
                        default,
                        1.1f
                    );

                    Main.dust[dust].velocity *= 0.3f;
                    Main.dust[dust].noGravity = false;
                }

                drawColor = Color.Lerp(drawColor, new Color(200, 40, 40), 0.45f);
            }
        }
    }
    public class DragonHorn : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/TriHornedDragonHorn";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
        }

        public override void AI()
        {
            // Simple rotation and dust
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Main.rand.NextBool(6))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            ApplyBleed(target);
        }
        private void ApplyBleed(NPC target)
        {
            target.AddBuff(ModContent.BuffType<BleedDebuff>(), 300);

            var g = target.GetGlobalNPC<BleedDebuffNPC>();
            g.hasBleed = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // small bounce or die
            Projectile.Kill();
            return false;
        }
    }
    

}
