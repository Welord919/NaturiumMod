using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class Firegrass : BaseCardCommon
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/Firegrass";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = BuffID.ObsidianSkin;
            Item.buffTime = 60 * 40;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
}
