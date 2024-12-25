using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.Materials;

namespace NaturiumMod.Content.Items.Weapons;

public class BarkionsBlaster : ModItem
{
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
        Item.value = Item.buyPrice(0, 20);
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
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<NaturiumBar>(), 25), (ItemID.LeadBar, 15)], TileID.Anvils);
        recipe.Register();

        Recipe recipe1 = CreateRecipe();
        recipe1 = RecipeUtils.GetNewRecipe(recipe1, [(ModContent.ItemType<NaturiumBar>(), 25), (ItemID.IronBar, 15)], TileID.Anvils);
        recipe1.Register();
    }
}
