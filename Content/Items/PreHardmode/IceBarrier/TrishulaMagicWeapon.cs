using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.IceBarrier
{
    public class TrishulaWeapon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/TrishulaWeapon";

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.autoReuse = true;
            Item.staff[Item.type] = true;
            Item.UseSound = SoundID.Item8;

            Item.DamageType = DamageClass.Magic;
            Item.damage = 33;
            Item.knockBack = 3f;
            Item.mana = 12;

            Item.shoot = ModContent.ProjectileType<TrishulaProj>();
            Item.shootSpeed = 16f;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 3);
        }

        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 baseDir = velocity.SafeNormalize(Vector2.UnitX);

            // Three beams: left, center, right
            float spread = MathHelper.ToRadians(12f);

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
            recipe.AddIngredient(ModContent.ItemType<IceBarrierCore>(), 20);
            recipe.AddIngredient(ModContent.ItemType<WaterEssence>(), 8);
            recipe.AddIngredient(ItemID.Sapphire, 9);
            recipe.AddIngredient(ModContent.ItemType<TripleWOF>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
    public class TrishulaProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/TrishulaProj";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;

            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 3;
            Projectile.tileCollide = true;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.extraUpdates = 1;
        }

        public override void AI()
        {
            // Frosty trail
            Dust.NewDustPerfect(
                Projectile.Center,
                DustID.IceTorch,
                Projectile.velocity * -0.2f,
                150,
                Color.Cyan,
                1.2f
            );

            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 180);

            // If target is frozen → shatter explosion
            if (target.HasBuff(BuffID.Frozen))
            {
                for (int i = 0; i < 12; i++)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(4f, 4f);
                    Dust.NewDustPerfect(target.Center, DustID.IceRod, vel, 150, Color.White, 1.4f);
                }

                // Bonus damage
                target.SimpleStrikeNPC(damageDone / 2, hit.HitDirection);
            }
        }
    }

}