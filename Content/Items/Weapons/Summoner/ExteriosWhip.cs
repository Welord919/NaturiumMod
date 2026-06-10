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
        Item.DefaultToWhip(ModContent.ProjectileType<ExteriosWhipProj>(), 60, 5f, 5);
        Item.shootSpeed = 4;
        Item.rare = ItemRarityID.LightRed;
        Item.channel = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<BarkionsBark>(), 25),
            new(ModContent.ItemType<ExteriosFang>(), 1),
            new(ModContent.ItemType<NaturiumBar>(), 15)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }

    public override bool MeleePrefix() => true;
}
