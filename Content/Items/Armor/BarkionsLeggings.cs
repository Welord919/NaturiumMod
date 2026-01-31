using NaturiumMod.Content.Items.Materials.PreHardmodeMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Armor;

[AutoloadEquip(EquipType.Legs)]
public class BarkionsLeggings : ModItem
{
    public override void SetStaticDefaults() { }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.value = 2000;
        Item.rare = ItemRarityID.Blue;
        Item.defense = 4;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetKnockback(DamageClass.Magic) += 0.1f;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, (ModContent.ItemType<BarkionsBark>(), 75), TileID.Anvils);
        recipe.Register();
    }
}
