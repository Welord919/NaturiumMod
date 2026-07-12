using NaturiumMod.Content.Items.Cards;
using NaturiumMod.Content.Items.Cards.Fusion;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion
{
    public static class ExtractionRegistry
    {
        public static readonly Dictionary<int, (int essenceType, int amount)> ExtractionMap = new();

        public static void RegisterExtraction(int cardType, int essenceType, int amount)
        {
            ExtractionMap[cardType] = (essenceType, amount);
        }
    }

    public class EssenceExtractionRecipes : ModSystem
    {
        public override void PostAddRecipes()
        {
            ExtractionRegistry.ExtractionMap.Clear();

            foreach (var entry in CardPools.AllCards)
            {
                if (!entry.Category.Equals("Card", StringComparison.OrdinalIgnoreCase))
                    continue;

                string attribute = entry.CardAttributesList.FirstOrDefault();
                if (string.IsNullOrEmpty(attribute))
                    continue;

                int essenceType = EssenceTypeHelper.GetEssenceItem(attribute);
                int amount = EssenceYieldHelper.GetEssenceYield(entry.Rarity);

                if (essenceType == 0 || amount <= 0)
                    continue;

                ExtractionRegistry.RegisterExtraction(entry.ItemType, essenceType, amount);
            }
        }
    }

    public class ExtractionRecipeCreator : ModSystem
    {
        public override void PostAddRecipes()
        {
            foreach (var kv in ExtractionRegistry.ExtractionMap)
            {
                int cardType = kv.Key;
                int essenceType = kv.Value.essenceType;
                int amount = kv.Value.amount;

                Recipe recipe = Recipe.Create(essenceType, amount);
                recipe.AddIngredient(cardType);
                recipe.AddIngredient(ModContent.ItemType<EssenceExtractor>());
                recipe.AddTile(ModContent.TileType<FusionAltarTile>());
                recipe.Register();
            }
        }
    }

    public static class EssenceYieldHelper
    {
        public static int GetEssenceYield(Rarity rarity)
        {
            return rarity switch
            {
                Rarity.Common => 1,
                Rarity.Rare => 2,
                Rarity.ShortPrint => 3,
                Rarity.SuperRare => 3,
                Rarity.Exodia => 4,
                Rarity.SuperShortPrint => 4,
                Rarity.UltraRare => 5,
                Rarity.Crafted => 5,
                Rarity.Fusion => 7,
                _ => 1
            };
        }
    }

    public static class EssenceTypeHelper
    {
        public static int GetEssenceItem(string attribute)
        {
            return attribute switch
            {
                "Fire" => ModContent.ItemType<FireEssence>(),
                "Water" => ModContent.ItemType<WaterEssence>(),
                "Earth" => ModContent.ItemType<EarthEssence>(),
                "Wind" => ModContent.ItemType<WindEssence>(),
                "Light" => ModContent.ItemType<LightEssence>(),
                "Dark" => ModContent.ItemType<DarkEssence>(),
                "Spell" => ModContent.ItemType<SpellEssence>(),
                "Trap" => ModContent.ItemType<TrapEssence>(),
                "Dragon" => ModContent.ItemType<DragonScale>(),
                _ => 0
            };
        }
    }
}
