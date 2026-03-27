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
    public class SpiritReaperCard : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/NPCDrop/SpiritReaperCard";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Lime;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(silver: 10);
            Item.buffType = ModContent.BuffType<SpiritReaperBuff>();
            Item.buffTime = 60 * 60 * 3;
        }
    }
    public class SpiritReaperBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/NPCDrop/SpiritReaperBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false; // it's a beneficial buff
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Immune to knockback
            player.noKnockback = true;

            // Reduce damage taken by 5%
            player.endurance += 0.05f;
        }

    }

}
