using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class PlagueChunk : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/PlagueChunk";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.rare = ItemRarityID.White;
        Item.maxStack = 999;
        Item.value = Item.buyPrice(0, 0, 0, 75);
    }
}
