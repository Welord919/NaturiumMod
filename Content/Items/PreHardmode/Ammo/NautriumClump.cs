using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Ammo;

public class NaturiumClump : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Ammo/NaturiumClump";

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 99;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);

        Item.damage = 6;
        Item.DamageType = DamageClass.Ranged;
        Item.knockBack = 0.25f;

        Item.maxStack = 9999;
        Item.consumable = true;

        Item.value = 50;
        Item.shoot = Mod.Find<ModProjectile>("NaturiumClumpProj").Type;

        Item.shootSpeed = 14f;
        Item.ammo = AmmoID.Bullet;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(15);
        recipe = RecipeHelper.GetNewRecipe(recipe, [new(ModContent.ItemType<NaturiumBar>(), 1)], TileID.Anvils);
        recipe.Register();
    }
}
