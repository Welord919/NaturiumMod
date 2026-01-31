using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.General.Projectiles;
using NaturiumMod.Content.Items.PostHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PostHardmode.Weapons;

public class ExteriosWhip : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Weapons/ExteriosWhip";

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
        recipe = RecipeHelper.GetNewRecipe(recipe, [(ModContent.ItemType<BarkionsBark>(), 25), (ModContent.ItemType<ExteriosFang>(), 1), (ModContent.ItemType<NaturiumBar>(), 15)], TileID.MythrilAnvil);
        recipe.Register();
    }

    public override bool MeleePrefix()
    {
        return true;
    }
}
