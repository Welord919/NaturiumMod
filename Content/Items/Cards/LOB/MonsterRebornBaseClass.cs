using NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint;
using System;
using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards
{
    public abstract class UltraRareCard : ModItem
    {
        public override void SetDefaults()
        {
            Item.consumable = false; // NEVER auto-consume
            Item.maxStack = 999;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        // ============================================================
        //  MONSTER REBORN INTERCEPT
        // ============================================================

        protected bool TryApplyMonsterReborn(Player player)
        {
            int rebornBuff = ModContent.BuffType<RebornBuff>();

            if (player.HasBuff(rebornBuff))
            {
                player.ClearBuff(rebornBuff);

                // Consume Monster Reborn instead of this card
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    if (player.inventory[i].type == ModContent.ItemType<MonsterReborn>())
                    {
                        player.inventory[i].stack--;
                        if (player.inventory[i].stack <= 0)
                            player.inventory[i].TurnToAir();
                        break;
                    }
                }

                return true;
            }

            return false;
        }

        // ============================================================
        //  MANUAL CONSUMPTION
        // ============================================================

        protected void ConsumeCard(Player player, int amount)
        {
            for (int i = 0; i < player.inventory.Length && amount > 0; i++)
            {
                if (player.inventory[i].type == Type)
                {
                    int take = Math.Min(player.inventory[i].stack, amount);
                    player.inventory[i].stack -= take;
                    amount -= take;

                    if (player.inventory[i].stack <= 0)
                        player.inventory[i].TurnToAir();
                }
            }
        }
    }
    public static class CardUtils
    {
        public static bool TryApplyMonsterReborn(Player player, int cardType)
        {
            int rebornBuff = ModContent.BuffType<RebornBuff>();

            if (player.HasBuff(rebornBuff))
            {
                player.ClearBuff(rebornBuff);

                // Consume Monster Reborn instead of the card
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    if (player.inventory[i].type == ModContent.ItemType<MonsterReborn>())
                    {
                        player.inventory[i].stack--;
                        if (player.inventory[i].stack <= 0)
                            player.inventory[i].TurnToAir();
                        break;
                    }
                }

                return true; // protected
            }

            return false;
        }
    }

}