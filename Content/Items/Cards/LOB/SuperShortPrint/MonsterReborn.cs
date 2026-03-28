using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Cards.LOB.Rares.CurseofDragon;

namespace NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint
{

    public class MonsterReborn : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/MonsterReborn";

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Orange;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.consumable = false;
            Item.value = Item.buyPrice(silver: 50);
            Item.buffType = ModContent.BuffType<RebornBuff>();
            Item.buffTime = 60 * 60 * 10;
        }

        public override bool? UseItem(Player player)
        {
            CombatText.NewText(player.getRect(), Color.Cyan, "Your next card is protected!");
            return true;
        }

        /// <summary>
        /// Call this from your card consumption logic.
        /// Returns true if the card should be consumed.
        /// Returns false if RebornBuff prevented consumption.
        /// </summary>
        public static bool TryPreventCardConsumption(Player player)
        {
            int rebornBuff = ModContent.BuffType<RebornBuff>();

            if (player.HasBuff(rebornBuff))
            {
                // Remove the buff
                player.ClearBuff(rebornBuff);

                CombatText.NewText(player.getRect(), Color.LightBlue, "Monster Reborn protected your card!");

                // Do NOT consume the card
                return false;
            }

            // No buff → consume normally
            return true;
        }
    }
    public class RebornBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/RebornBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true; // stays until manually removed
            Main.debuff[Type] = false;
        }
    }

}