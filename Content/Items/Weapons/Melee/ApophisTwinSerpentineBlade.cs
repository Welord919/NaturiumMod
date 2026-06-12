using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Projectiles.Melee;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons.Melee
{
    public class ApophisTwinSerpentineBlade : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Weapons/ApophisTwinSerpentineBlade";

        public override void SetDefaults()
        {
            Item.width = 54;
            Item.height = 54;

            Item.damage = 38;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 5.5f;
            Item.crit = 8;

            Item.useTime = 24;
            Item.useAnimation = 24;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.autoReuse = true;
            Item.UseSound = SoundID.Item20;

            // Use the shared projectile (ApophisProjPlus)
            Item.shoot = ModContent.ProjectileType<ApophisProjPlus>();
            Item.shootSpeed = 12f;

            Item.value = Item.buyPrice(0, 20, 0, 0);

            Item.rare = ItemRarityID.Pink;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<ApophisFangblade>(), 1),
            new(ItemID.MythrilBar, 18),
            new(ItemID.CursedFlame, 12)
            ], TileID.MythrilAnvil);
            recipe.Register();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
    Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // LEFT CLICK ONLY: spawn two ApophisProjPlus projectiles with slight spread
            if (player.altFunctionUse != 2)
            {
                int projType = ModContent.ProjectileType<ApophisProjPlus>();

                Vector2 v1 = velocity.RotatedBy(MathHelper.ToRadians(10));
                Vector2 v2 = velocity.RotatedBy(MathHelper.ToRadians(-10));

                // Pass ai0 = 1f to mark these projectiles as "from the twin blade"
                Projectile.NewProjectile(source, position, v1, projType, damage, knockback, player.whoAmI, 1f);
                Projectile.NewProjectile(source, position, v2, projType, damage, knockback, player.whoAmI, 1f);
            }

            return false; // prevent vanilla from spawning a default projectile
        }


        public override void ModifyItemScale(Player player, ref float scale)
        {
            scale = 0.5f;
        }
    }
}
