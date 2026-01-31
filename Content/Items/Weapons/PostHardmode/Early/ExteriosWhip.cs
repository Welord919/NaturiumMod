using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.Projectiles;
using NaturiumMod.Content.Items.Materials.PreHardmode;
using NaturiumMod.Content.Items.Materials.PostHardmode;

namespace NaturiumMod.Content.Items.Weapons.PostHardmode.Early;

public class ExteriosWhip : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/PostHardmode/Early/ExteriosWhip";

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
