using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class NaturiumOre : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/NaturiumOre";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 50;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.maxStack = 999;
        Item.consumable = true;
        Item.value = Item.buyPrice(0, 0, 0, 50);

        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useTurn = true;
        Item.autoReuse = true;

        Item.createTile = ModContent.TileType<Tiles.NaturiumOreTile>();
    }
}
