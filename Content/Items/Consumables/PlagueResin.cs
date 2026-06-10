using Microsoft.Xna.Framework;
using NaturiumMod.Content.BuffsDebuffs;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.NPCs.ManyGlobalNPC;

namespace NaturiumMod.Content.Items.Consumables; 

public class PlagueResin : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Consumables/PlagueResin";
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 999;
        Item.useStyle = ItemUseStyleID.EatFood;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.UseSound = SoundID.Item3;
        Item.consumable = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 5);

        Item.buffType = ModContent.BuffType<PlagueInfusionBuff>();
        Item.buffTime = 60 * 60 * 5;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<PlagueChunk>(), 5),
                new(ItemID.BottledWater, 1)
        ], TileID.ImbuingStation);
        recipe.Register();
    }
}




