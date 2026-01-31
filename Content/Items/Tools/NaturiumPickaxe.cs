using NaturiumMod.Content.Items.Materials.PreHardmode;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Tools;

public class NaturiumPickaxe : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Tools/NaturiumPickaxe";
    
    public override void SetDefaults()
    {
        Item.damage = 20;
        Item.DamageType = DamageClass.Melee;
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 9;
        Item.value = Item.buyPrice(gold: 1);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.tileBoost = 1;
        Item.pick = 100;
        Item.attackSpeedOnlyAffectsWeaponAnimation = true;
    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<NaturiumBar>(), 5), (ItemID.NightmarePickaxe, 1), (ItemID.GoldBar, 10)], TileID.Anvils);
        recipe.Register();

        Recipe recipe2 = CreateRecipe();
        recipe2 = RecipeUtils.GetNewRecipe(recipe2, [(ModContent.ItemType<NaturiumBar>(), 5), (ItemID.DeathbringerPickaxe, 1), (ItemID.GoldBar, 10)], TileID.Anvils);
        recipe2.Register();
    }
}
