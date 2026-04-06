using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards
{
    public abstract class MRUltra : ModItem
    {
        public override void SetDefaults()
        {
            Item.UseSound = SoundID.Item4;
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 999;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = 2000;
            Item.DamageType = ModContent.GetInstance<CardDamage>();

            Item.consumable = false;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
            {
                return false;
            }
            return base.CanUseItem(player);
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