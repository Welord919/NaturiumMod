using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Text;
using NaturiumMod.Content.Items.PostHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PostHardmode.Materials;

    public class InfusedNaturiumGunParts : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Materials/InfusedNaturiumGunParts";

        public override void SetStaticDefaults()
        {
            // Optional: animated texture (remove if not needed)
            // Main.RegisterItemAnimation(Type, new DrawAnimationVertical(5, 4));

            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;

            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.LightRed; // Hardmode-tier rarity

            Item.material = true; // Helps with recipe grouping
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<InfusedNaturiumBar>(), 3),
                new(ModContent.ItemType<NaturesEssence>(), 2),
        ], TileID.MythrilAnvil);
            recipe.Register();
        }

    }

