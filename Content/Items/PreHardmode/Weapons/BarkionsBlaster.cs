using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class BarkionsBlaster : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/BarkionsBlaster";

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useAnimation = 36;
        Item.useTime = 36;
        Item.crit += 25;
        Item.width = 44;
        Item.height = 14;

        Item.UseSound = SoundID.Item40;
        Item.damage = 35;

        Item.noMelee = true;
        Item.value = Item.buyPrice(0, 20, 0, 0);
        Item.knockBack = 8f;
        Item.rare = ItemRarityID.Yellow;
        Item.DamageType = DamageClass.Ranged;

        //Gun Properties
        Item.shootSpeed = 15f;
        Item.shoot = ProjectileID.PurificationPowder;
        Item.useAmmo = Mod.Find<ModItem>("NaturiumClump").Type;
        Item.useAmmo = AmmoID.Bullet;

    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 25),
            new(ItemID.LeadBar, 15)
        ], TileID.Anvils);
        recipe.Register();

        recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 25),
            new(ItemID.IronBar, 15)
        ], TileID.Anvils);
        recipe.Register();
    }
}
