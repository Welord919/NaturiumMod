using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class BarkionsSS : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/BarkionsSS";

    public override void SetDefaults()
    {
        Item.width = 62;
        Item.height = 32;
        Item.scale = 0.75f;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(silver: 20);

        // Use Properties
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item11;

        // Weapon Properties
        Item.DamageType = DamageClass.Ranged;
        Item.damage = 12;
        Item.knockBack = 1f;
        Item.noMelee = true;

        // Gun Properties
        Item.shoot = ProjectileID.PurificationPowder;
        Item.shoot = Mod.Find<ModProjectile>("BarkionsBarkProj").Type;
        Item.shootSpeed = 9f;
        Item.useAmmo = Mod.Find<ModItem>("BarkionsBark").Type;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [(ModContent.ItemType<BarkionsBark>(), 50), (ItemID.IronBar, 5)], TileID.Anvils);
        recipe.Register();

        Recipe recipe1 = CreateRecipe();
        recipe1 = RecipeHelper.GetNewRecipe(recipe1, [(ModContent.ItemType<BarkionsBark>(), 50), (ItemID.LeadBar, 5)], TileID.Anvils);
        recipe1.Register();
    }
}

