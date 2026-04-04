using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Shark;
public class SharkFinBlades : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SharkFinBlades";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.rare = ItemRarityID.White;
        Item.maxStack = 999;
        Item.value = Item.buyPrice(0, 0, 5, 0);
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.SharkFin, 1)
            .AddIngredient(ModContent.ItemType<SharkFinBladesDamaged>(), 5)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
public class SharkFinBladesDamaged : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SharkFinBladesDamaged";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.rare = ItemRarityID.White;
        Item.maxStack = 999;
        Item.value = Item.buyPrice(0, 0, 0, 80);
    }
}
