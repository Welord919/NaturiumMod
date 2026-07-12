using NaturiumMod.Content.Items.Cards;
using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.Linq;
using System.Collections.Generic;

namespace NaturiumMod.Content.Items.Cards.Fusion
{
    // ---------------------------
    // DRAGON SCALE ITEM
    // ---------------------------
    public class DragonScale : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Materials/DragonScale";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(silver: 5);
            Item.rare = ItemRarityID.LightRed;
        }
    }

    // ---------------------------
    // GENOME EXTRACTOR ITEM
    // ---------------------------
    public class GenomeExtractor : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/GenomeExtractor";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 999;
            Item.value = 500;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ModContent.ItemType<NaturiumBar>(), 5)
                .AddTile(ModContent.TileType<FusionAltarTile>())
                .Register();
        }
    }

    // ---------------------------
    // DRAGON EXTRACTION REGISTRY
    // ---------------------------
    public static class DragonExtractionRegistry
    {
        public static readonly Dictionary<int, int> DragonScaleYield = new();

        public static void Register(int cardType, int amount)
        {
            DragonScaleYield[cardType] = amount;
        }
    }

    // ---------------------------
    // DRAGON EXTRACTION SCAN (robust)
    // ---------------------------
    public class DragonExtractionRegistrar : ModSystem
    {
        // Scan after content is loaded to ensure CardPools is populated and overrides are available
        public override void PostSetupContent()
        {
            DragonExtractionRegistry.DragonScaleYield.Clear();

            foreach (var entry in CardPools.AllCards)
            {
                // Only cards
                if (!entry.Category.Equals("Card", StringComparison.OrdinalIgnoreCase))
                    continue;

                // 1) Prefer the Subtype stored in CardEntry if present
                string subtype = entry.Subtype?.Trim();

                // 2) If CardEntry.Subtype is empty, try to read the actual ModItem override (BaseCard.CardSubtype)
                if (string.IsNullOrWhiteSpace(subtype))
                {
                    try
                    {
                        ModItem mi = ModContent.GetModItem(entry.ItemType);
                        if (mi is BaseCard bc)
                        {
                            subtype = bc.CardSubtype?.Trim();
                        }
                    }
                    catch
                    {
                        // ignore and continue to other checks
                    }
                }

                // 3) If still empty, check CardAttributesList for a "Dragon"-like attribute
                if (string.IsNullOrWhiteSpace(subtype) && entry.CardAttributesList != null)
                {
                    var attr = entry.CardAttributesList.FirstOrDefault(a => !string.IsNullOrWhiteSpace(a));
                    if (!string.IsNullOrWhiteSpace(attr) && attr.IndexOf("dragon", StringComparison.OrdinalIgnoreCase) >= 0)
                        subtype = "Dragon";
                }

                // 4) Final check: name heuristic (only if you want it; safe fallback)
                if (string.IsNullOrWhiteSpace(subtype))
                {
                    string itemName = Lang.GetItemNameValue(entry.ItemType) ?? "";
                    if (itemName.IndexOf("dragon", StringComparison.OrdinalIgnoreCase) >= 0
                        || (itemName.IndexOf("blue", StringComparison.OrdinalIgnoreCase) >= 0 && itemName.IndexOf("eyes", StringComparison.OrdinalIgnoreCase) >= 0))
                    {
                        subtype = "Dragon";
                    }
                }

                // If subtype resolved to Dragon, register yield
                if (!string.IsNullOrWhiteSpace(subtype) && subtype.Equals("Dragon", StringComparison.OrdinalIgnoreCase))
                {
                    int amount = EssenceYieldHelper.GetEssenceYield(entry.Rarity);
                    if (amount > 0)
                        DragonExtractionRegistry.Register(entry.ItemType, amount);
                }
            }
        }
    }

    // ---------------------------
    // DRAGON EXTRACTION RECIPES
    // ---------------------------
    public class DragonExtractionRecipeCreator : ModSystem
    {
        // Create recipes in PostAddRecipes so they appear correctly in the crafting system
        public override void PostAddRecipes()
        {
            foreach (var kv in DragonExtractionRegistry.DragonScaleYield)
            {
                int cardType = kv.Key;
                int amount = kv.Value;

                Recipe recipe = Recipe.Create(ModContent.ItemType<DragonScale>(), amount);
                recipe.AddIngredient(cardType);
                recipe.AddIngredient(ModContent.ItemType<GenomeExtractor>());
                recipe.AddTile(ModContent.TileType<FusionAltarTile>());
                recipe.Register();
            }
        }
    }
}
