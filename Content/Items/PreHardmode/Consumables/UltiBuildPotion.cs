using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Consumables;

public class UltiBuildPotion : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Consumables/UltiBuildPotion";

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
        Item.buffType = ModContent.BuffType<Buffs.UltiBuildPotionBuff>(); // Specify an existing buff to be applied when used.
        Item.buffTime = 14400; // The amount of time the buff declared in Item.buffType will last in ticks. 14400 / 60 = 240, so this buff will last 4 minutes.
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumOre>(), 4),
            new(ModContent.ItemType<CameliaPetal>(), 2),
            new(ItemID.BottledWater, 1),
            new(ItemID.Blinkroot, 1)
        ], TileID.AlchemyTable);
        recipe.Register();
    }
}
