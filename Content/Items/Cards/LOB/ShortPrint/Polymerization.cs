using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.ShortPrint;
public class Polymerization : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/Polymerization";

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.maxStack = 999;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(silver: 25);
    }
}