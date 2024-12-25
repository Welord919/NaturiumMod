using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.Projectiles;
using NaturiumMod.Content.Items.Materials;

namespace NaturiumMod.Content.Items.Weapons;

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
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<BarkionsBark>(), 25), (ModContent.ItemType<ExteriosFang>(), 1), (ModContent.ItemType<NaturiumBar>(), 15)], TileID.MythrilAnvil);
        recipe.Register();
    }

    public override bool MeleePrefix()
    {
        return true;
    }
}
