using NaturiumMod.Content.Items.Materials.PreHardmodeMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Consumables;

public class MktPotion : ModItem
{
    public override void SetDefaults()
    {
        Item.Size = new(20, 26);
        Item.useStyle = ItemUseStyleID.DrinkLiquid;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.UseSound = SoundID.Item3;
        Item.maxStack = Item.CommonMaxStack;
        Item.consumable = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.buffType = ModContent.BuffType<Buffs.MktPotionBuff>(); // Specify an existing buff to be applied when used.
        Item.buffTime = 14400; // The amount of time the buff declared in Item.buffType will last in ticks. 14400 / 60 = 240, so this buff will last 4 minutes.
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<NaturiumOre>(), 4), (ModContent.ItemType<CameliaPetal>(), 2), (ItemID.BottledWater, 1)], TileID.AlchemyTable);
        recipe.Register();
    }
}
