using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Materials;
using NaturiumMod.Content.Projectiles.Summoner;

namespace NaturiumMod.Content.Items.Weapons.Summoner;

public class ExteriosWhip : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/ExteriosWhip";

    public override void SetDefaults()
    {
        Item.DefaultToWhip(ModContent.ProjectileType<ExteriosWhipProj>(), 75, 5f, 5);
        Item.shootSpeed = 4;
        Item.rare = ItemRarityID.LightRed;
        Item.channel = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<ExteriosFang>(), 8),
            new(ModContent.ItemType<Apoqliphite>(), 15)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }

    public override bool MeleePrefix() => true;
}
