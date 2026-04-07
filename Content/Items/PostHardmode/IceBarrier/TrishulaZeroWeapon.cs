using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.PreHardmode.IceBarrier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PostHardmode.IceBarrier
{
    public class TrishulaZeroWeapon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/IceBarrier/TrishulaZeroWeapon";

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 18;
            Item.useAnimation = 18;
            Item.autoReuse = true;
            Item.staff[Item.type] = true;
            Item.UseSound = SoundID.Item20;

            Item.DamageType = DamageClass.Magic;
            Item.damage = 52;
            Item.knockBack = 3.5f;
            Item.mana = 10;

            Item.shoot = ModContent.ProjectileType<TrishulaZeroProj>();
            Item.shootSpeed = 20f;

            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 6);
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 baseDir = velocity.SafeNormalize(Vector2.UnitX);
            float spread = MathHelper.ToRadians(10f);

            for (int i = -1; i <= 1; i++)
            {
                Vector2 perturbed = baseDir.RotatedBy(spread * i);
                Projectile.NewProjectile(
                    source,
                    position,
                    perturbed * Item.shootSpeed,
                    type,
                    damage,
                    knockback,
                    player.whoAmI
                );
            }

            return false;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<TrishulaWeapon>(), 1);
            recipe.AddIngredient(ModContent.ItemType<IceBarrierCore>(), 30);
            recipe.AddIngredient(ItemID.SoulofNight, 12);
            recipe.AddIngredient(ItemID.Sapphire, 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
    public class TrishulaZeroProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/IceBarrier/TrishulaZeroProj";

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 4;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            Dust.NewDustPerfect(
                Projectile.Center,
                DustID.IceTorch,
                Projectile.velocity * -0.25f,
                150,
                Color.Cyan,
                1.4f
            );

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Frostburn
            target.AddBuff(BuffID.Frostburn2, 240);
            target.AddBuff(BuffID.OnFire3, 240);

            // ❄ Freeze shatter bonus
            if (target.HasBuff(BuffID.Frozen))
            {
                for (int i = 0; i < 14; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(5f, 5f);
                    Dust.NewDustPerfect(target.Center, DustID.IceRod, vel, 150, Color.White, 1.5f);
                }

                target.SimpleStrikeNPC(damageDone / 2, hit.HitDirection);
            }

            // ❤️ Lifesteal (2% of damage, capped at 6 HP per hit)
            Player owner = Main.player[Projectile.owner];
            int heal = (int)(damageDone * 0.02f);
            if (heal > 6) heal = 6;

            if (heal > 0 && owner.statLife < owner.statLifeMax2)
            {
                owner.statLife += heal;
                owner.HealEffect(heal, true);
            }
        }
    }
}
