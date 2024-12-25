using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.Projectiles;
using NaturiumMod.Content.Items.Materials;

namespace NaturiumMod.Content.Items.Weapons
{
    public class ExteriosWhip : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<ExteriosWhipProj>(), 60, 5f, 5);
            Item.shootSpeed = 4;
            Item.rare = ItemRarityID.Purple;
            Item.channel = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BarkionsBark>(), 25);
            recipe.AddIngredient(ModContent.ItemType<ExteriosFang>(), 1);
            recipe.AddIngredient(ModContent.ItemType<NaturiumBar>(), 15);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }

        public override bool MeleePrefix()
        {  
            return true; 
        }
    }
}