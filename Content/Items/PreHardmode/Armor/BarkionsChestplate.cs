using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Armor;

[AutoloadEquip(EquipType.Body)]
public class BarkionsChestplate : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Armor/BarkionsChestplate";

    public override void SetStaticDefaults() { }

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.value = 2500;
        Item.rare = ItemRarityID.Blue;
        Item.defense = 6;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetKnockback(DamageClass.Melee) += 0.1f;
        player.GetKnockback(DamageClass.Summon) += 0.1f;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, (ModContent.ItemType<BarkionsBark>(), 125), TileID.Anvils);
        recipe.Register();
    }
}
