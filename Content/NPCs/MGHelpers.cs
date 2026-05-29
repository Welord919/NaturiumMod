using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Cards.PSA;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.NPCs.MysteriousGrandpa;

namespace NaturiumMod.Content.NPCs;
public static class MGHelpers
{
    
    public static void RewardPack(Player player)
    {
        var weighted = new List<(int type, int weight)>
        {
            (ModContent.ItemType<PackLOB_Common>(), 45),
            (ModContent.ItemType<PackLOB_Rare>(),   25),
            (ModContent.ItemType<PackLOB_Super>(),  20),
            (ModContent.ItemType<PackLOB_Ultra>(),  10)
        };

        int totalWeight = weighted.Sum(w => w.weight);
        int roll = Main.rand.Next(totalWeight);

        foreach (var entry in weighted)
        {
            if (roll < entry.weight)
            {
                player.QuickSpawnItem(
                    player.GetSource_GiftOrReward(),
                    entry.type
                );

                SoundEngine.PlaySound(SoundID.Item37);
                return;
            }

            roll -= entry.weight;
        }
    }

    public static int CountTaggedCards(Player player, string tag)
    {
        int count = 0;

        foreach (var item in player.inventory)
        {
            if (WeaponTag.ItemTags.TryGetValue(item.type, out var tags) &&
                tags.Contains(tag))
            {
                count += item.stack;
            }
        }

        return count;
    }

    public static bool HasAnyPSA10Card(Player player)
    {
        foreach (var item in player.inventory)
        {
            if (item?.ModItem is BaseGradedCard card && card.grade >= 10f)
                return true;
        }

        return false;
    }

    public static void ConsumeTaggedCards(Player player, string tag, int amount)
    {
        for (int i = 0; i < player.inventory.Length && amount > 0; i++)
        {
            Item item = player.inventory[i];

            if (WeaponTag.ItemTags.TryGetValue(item.type, out var tags) &&
                tags.Contains(tag))
            {
                int take = System.Math.Min(item.stack, amount);
                item.stack -= take;
                amount -= take;

                if (item.stack <= 0)
                    item.TurnToAir();
            }
        }
    }

    public static void ConsumeItem(Player player, int type, int amount)
    {
        for (int i = 0; i < player.inventory.Length && amount > 0; i++)
        {
            if (player.inventory[i].type == type)
            {
                int take = System.Math.Min(player.inventory[i].stack, amount);
                player.inventory[i].stack -= take;
                amount -= take;

                if (player.inventory[i].stack <= 0)
                    player.inventory[i].TurnToAir();
            }
        }
    }
    public static class CardQuery
    {
        public static int CountCardsWithSubtype(Player player, string subtype)
        {
            int count = 0;

            foreach (var item in player.inventory)
            {
                if (item == null || item.IsAir)
                    continue;

                if (!CardPools.TryGetEntry(item.type, out var entry))
                    continue;

                if (entry.Subtype == subtype)
                    count += item.stack;
            }

            return count;
        }

        public static int CountCardsWithAttribute(Player player, string attribute)
        {
            int count = 0;

            foreach (var item in player.inventory)
            {
                if (item == null || item.IsAir)
                    continue;

                if (!CardPools.TryGetEntry(item.type, out var entry))
                    continue;

                if (entry.CardAttributesList.Contains(attribute))
                    count += item.stack;
            }

            return count;
        }

        public static bool HasCardWithRarity(Player player, Rarity rarity)
        {
            foreach (var item in player.inventory)
            {
                if (item == null || item.IsAir)
                    continue;

                if (!CardPools.TryGetEntry(item.type, out var entry))
                    continue;

                if (entry.Rarity == rarity)
                    return true;
            }

            return false;
        }
        public static void ConsumeCardsWithSubtype(Player player, string subtype, int amount)
        {
            for (int i = 0; i < player.inventory.Length && amount > 0; i++)
            {
                var item = player.inventory[i];
                if (item == null || item.IsAir) continue;

                if (!CardPools.TryGetEntry(item.type, out var entry)) continue;
                if (entry.Subtype != subtype) continue;

                int take = Math.Min(item.stack, amount);
                item.stack -= take;
                amount -= take;

                if (item.stack <= 0)
                    item.TurnToAir();
            }
        }

        public static void ConsumeCardsWithAttribute(Player player, string attribute, int amount)
        {
            for (int i = 0; i < player.inventory.Length && amount > 0; i++)
            {
                var item = player.inventory[i];
                if (item == null || item.IsAir) continue;

                if (!CardPools.TryGetEntry(item.type, out var entry)) continue;
                if (!entry.CardAttributesList.Contains(attribute)) continue;

                int take = Math.Min(item.stack, amount);
                item.stack -= take;
                amount -= take;

                if (item.stack <= 0)
                    item.TurnToAir();
            }
        }

        public static void ConsumeCardWithRarity(Player player, Rarity rarity, int amount)
        {
            for (int i = 0; i < player.inventory.Length && amount > 0; i++)
            {
                var item = player.inventory[i];
                if (item == null || item.IsAir) continue;

                if (!CardPools.TryGetEntry(item.type, out var entry)) continue;
                if (entry.Rarity != rarity) continue;

                int take = Math.Min(item.stack, amount);
                item.stack -= take;
                amount -= take;

                if (item.stack <= 0)
                    item.TurnToAir();
            }
        }
    }

}