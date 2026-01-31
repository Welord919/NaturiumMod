using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PostHardmode.Weapons;

public class ExteriosCannon : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Weapons/ExteriosCannon";

    public override void SetDefaults()
    {
        Item.width = 62;
        Item.height = 32;
        Item.scale = 1.0f;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(gold: 8);

        // Use Properties
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item36;

        // Weapon Properties
        Item.DamageType = DamageClass.Ranged;
        Item.damage = 40;
        Item.knockBack = 6.5f;
        Item.noMelee = true;

        // Gun Properties
        Item.shoot = ProjectileID.PurificationPowder;
        Item.shoot = Mod.Find<ModProjectile>("ExteriosFangProj").Type;
        Item.shootSpeed = 55f;
        Item.useAmmo = Mod.Find<ModItem>("ExteriosFang").Type;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<NaturiumBar>(), 50), (ItemID.HallowedBar, 15)], TileID.Anvils);
        recipe.Register();
    }
}
