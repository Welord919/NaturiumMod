using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.ShortPrint;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static NaturiumMod.Content.Items.Cards.CardRarityHelper;

namespace NaturiumMod.Content.Items.Cards.Fusion
{
    // ---------------------------
    //  FUSION ALTAR
    // ---------------------------
    public class FusionAltar : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FusionAltar";
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<FusionAltarTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NaturiumBar>(), 25)
                .AddIngredient(ModContent.ItemType<Polymerization>(), 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }
    public class FusionAltarTile : ModTile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FusionAltar";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            // 2x2 tile setup
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Microsoft.Xna.Framework.Color(150, 50, 200), CreateMapEntryName());
        }
    }

    // ---------------------------
    // ITEMS
    // ---------------------------
    public class EssenceExtractor : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/EssenceExtractor";
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
    public abstract class BaseEssence : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = 50;
            Item.rare = ItemRarityID.White;
        }
    }
    public class FireEssence : BaseEssence
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FireEssence";
    }

    public class WaterEssence : BaseEssence
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/WaterEssence";
    }

    public class EarthEssence : BaseEssence
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/EarthEssence";
    }

    public class WindEssence : BaseEssence
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/WindEssence";
    }

    public class LightEssence : BaseEssence
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/LightEssence";
    }

    public class DarkEssence : BaseEssence
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/DarkEssence";
    }
    public class SpellEssence : BaseEssence
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/SpellEssence";
    }
    public class TrapEssence : BaseEssence
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/TrapEssence";
    }
    public static class ExtractionRegistry
    {
        // Maps card item type -> (essenceType, amount)
        public static readonly Dictionary<int, (int essenceType, int amount)> ExtractionMap = new();

        public static void RegisterExtraction(int cardType, int essenceType, int amount)
        {
            ExtractionMap[cardType] = (essenceType, amount);
        }

        public static bool TryGetExtraction(int cardType, out (int essenceType, int amount) value)
        {
            return ExtractionMap.TryGetValue(cardType, out value);
        }
    }

    public class EssenceExtractionRecipes : ModSystem
    {
        public override void OnModLoad()
        {
            ExtractionRegistry.ExtractionMap.Clear();

            foreach (var entry in CardPools.AllCards)
            {
                int type = entry.ItemType;

                // Must be a card
                if (entry.Tags == null || !entry.Tags.Any(t => t.Equals("Card", StringComparison.OrdinalIgnoreCase)))
                    continue;

                // Get attribute (Fire/Water/Earth/Wind/Light/Dark/Spell/Trap)
                string attribute = CardTagHelper.GetCardAttribute(type);
                if (attribute == null)
                    continue;

                // Use entry.Rarity (safe even if CardRarityRegistry isn't populated yet)
                var rarity = entry.Rarity;

                int essenceType = EssenceTypeHelper.GetEssenceItem(attribute);
                int amount = EssenceYieldHelper.GetEssenceYield(rarity);

                if (essenceType == 0 || amount <= 0)
                    continue;

                ExtractionRegistry.RegisterExtraction(type, essenceType, amount);
            }
        }
    }
    public class ExtractionRecipeCreator : ModSystem
    {
        public override void AddRecipes()
        {
            foreach (var kv in ExtractionRegistry.ExtractionMap)
            {
                int cardType = kv.Key;
                int essenceType = kv.Value.essenceType;
                int amount = kv.Value.amount;

                // Create the recipe: card + extractor -> essence
                Recipe recipe = Recipe.Create(essenceType, amount);
                recipe.AddIngredient(cardType); // card consumed
                recipe.AddIngredient(ModContent.ItemType<EssenceExtractor>()); // extractor consumed
                recipe.AddTile(ModContent.TileType<FusionAltarTile>());
                recipe.Register();
            }
        }
    }


    // ---------------------------
    //  ESSENCE YIELD HELPER
    // ---------------------------
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

    // ---------------------------
    //  ESSENCE TYPE LOOKUP
    // ---------------------------
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
                _ => 0
            };
        }
    }
    
}