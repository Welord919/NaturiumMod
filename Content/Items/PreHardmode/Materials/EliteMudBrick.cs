using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class EliteMudBrick : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/EliteMudBrick";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.maxStack = 999;
        Item.consumable = true;
        Item.value = Item.buyPrice(0, 0, 0, 0);

        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useTurn = true;
        Item.autoReuse = true;

        Item.createTile = ModContent.TileType<Tiles.EliteMudBrickTile>();
    }
}
