using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.General.Projectiles;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class StarryNight : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/StarryNight";

    public override void SetDefaults()
    {
        Item.DefaultToWhip(ModContent.ProjectileType<StarryNightProj>(), 30, 3.5f, 5);
        Item.shootSpeed = 4;
        Item.rare = ItemRarityID.Green;
        Item.channel = true;

        Item.DamageType = DamageClass.Summon;
        Item.width = 40;
        Item.height = 40;

        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.rare = ItemRarityID.Orange;

        Item.damage = 32;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<Starsteel>(), 25)
        ], TileID.Anvils);
        recipe.Register();
    }

    public override bool MeleePrefix() => true;
}
