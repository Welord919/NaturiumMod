using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Tools
{
    public class NaturiumPickaxe : ModItem
    {
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
            recipe.AddIngredient(ModContent.ItemType<Materials.NaturiumBar>(), 5);
            recipe.AddIngredient(ItemID.NightmarePickaxe, 1);
            recipe.AddIngredient(ItemID.GoldBar, 10);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        
            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<Materials.NaturiumBar>(), 5);
            recipe2.AddIngredient(ItemID.DeathbringerPickaxe, 1);
            recipe2.AddIngredient(ItemID.GoldBar, 10);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }
}