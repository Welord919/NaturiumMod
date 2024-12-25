using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Consumables;

public class MktPotion : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 26;
        Item.useStyle = ItemUseStyleID.DrinkLiquid;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.UseSound = SoundID.Item3;
        Item.maxStack = Item.CommonMaxStack;
        Item.consumable = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 1);
        Item.buffType = ModContent.BuffType<Buffs.MktPotionBuff>(); // Specify an existing buff to be applied when used.
        Item.buffTime = 5400; // The amount of time the buff declared in Item.buffType will last in ticks. 5400 / 60 is 90, so this buff will last 90 seconds.
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<NaturiumOre>(), 4), (ModContent.ItemType<CameliaPetal>(), 2), (ItemID.BottledWater, 1)], TileID.Anvils);
        recipe.Register();
    }
}
