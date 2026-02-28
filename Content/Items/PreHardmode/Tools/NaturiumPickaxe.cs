using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Tools;

public class NaturiumPickaxe : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Tools/NaturiumPickaxe";
    
    public override void SetDefaults()
    {
        Item.damage = 20;
        Item.DamageType = DamageClass.Melee;
        Item.Size = new(40, 40);
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 10;
        Item.value = Item.buyPrice(0, 3, 0, 0);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.tileBoost = 2;
        Item.pick = 75;
        Item.attackSpeedOnlyAffectsWeaponAnimation = true;
    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 15),
            new(ItemID.NightmarePickaxe, 1)
        ], TileID.Anvils);
        recipe.Register();

        recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 15),
            new(ItemID.DeathbringerPickaxe, 1)
        ], TileID.Anvils);
        recipe.Register();
    }
}
