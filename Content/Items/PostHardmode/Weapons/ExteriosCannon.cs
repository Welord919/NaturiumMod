using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PostHardmode.Weapons;

public class ExteriosCannon : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Weapons/ExteriosCannon";

    public override void SetDefaults()
    {
        Item.Size = new(62, 32);
        Item.scale = 1.0f;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 8, 0, 0);

        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item36;

        Item.DamageType = DamageClass.Ranged;
        Item.damage = 40;
        Item.knockBack = 6.5f;
        Item.noMelee = true;

        Item.shoot = ProjectileID.PurificationPowder;
        Item.shoot = Mod.Find<ModProjectile>("ExteriosFangProj").Type;
        Item.shootSpeed = 55f;
        Item.useAmmo = Mod.Find<ModItem>("ExteriosFang").Type;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 50),
            new(ItemID.HallowedBar, 15)
        ], TileID.Anvils);
        recipe.Register();
    }
}
