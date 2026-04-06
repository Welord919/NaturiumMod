using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.NPCDrop
{
    public class PlaguespreaderCard : BaseCardSuper
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/NPCDrop/PlaguespreaderCard";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(silver: 25);
            Item.buffType = ModContent.BuffType<PlagueInfusionBuff>();
            Item.buffTime = 60 * 60 * 6;
        }
    }
}
