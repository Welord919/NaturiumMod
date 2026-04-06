using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.NPCDrop
{
    public class SpiritReaperCard : BaseCardSuper
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/NPCDrop/SpiritReaperCard";

        public override void SetDefaults()
        {
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(silver: 25);
            Item.buffType = ModContent.BuffType<SpiritReaperBuff>();
            Item.buffTime = 60 * 60 * 4;
        }
    }
    public class SpiritReaperBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/NPCDrop/SpiritReaperBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.noKnockback = true;
            player.endurance += 0.05f;
        }
    }
}
