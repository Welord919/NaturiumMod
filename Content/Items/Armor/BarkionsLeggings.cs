using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Materials;

namespace NaturiumMod.Content.Items.Armor;

[AutoloadEquip(EquipType.Legs)]
public class BarkionsLeggings : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Armor/BarkionsLeggings";

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
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
        recipe = RecipeHelper.GetNewRecipe(recipe, [new(ModContent.ItemType<BarkionsBark>(), 75)], TileID.Anvils);
        recipe.Register();
    }
}
