using Terraria;
using Terraria.ID;

namespace NaturiumMod.Content.Items.Cards.LOB.ShortPrint;
public class Polymerization : BaseCardShortPrint
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/Polymerization";
    public override string CardSubtype => "Spell";
    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Green;
    }
}