using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class RocketSword : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/RocketSword";

    public override void SetDefaults()
    {
        Item.DamageType = DamageClass.Melee;
        Item.damage = 30;
        Item.width = 30;
        Item.height = 50;
        Item.useTime = 22;
        Item.useAnimation = 22;
        Item.shootSpeed = 12f;
        Item.knockBack = 6f;
        Item.crit = 8;
        Item.UseSound = SoundID.Item1;

        Item.value = Item.buyPrice(0, 3, 0, 0);
        Item.rare = ItemRarityID.Green;
        Item.autoReuse = true;

        Item.useStyle = ItemUseStyleID.Swing;

        Item.shoot = ProjectileID.RocketI;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.IronBar, 25),
            new(ItemID.Wire, 10),
            new(ItemID.ExplosivePowder, 15)
        ], TileID.Anvils);
        recipe.Register();
    }
}
