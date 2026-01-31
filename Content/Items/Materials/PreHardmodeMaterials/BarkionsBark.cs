using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace NaturiumMod.Content.Items.Materials.PreHardmodeMaterials;

public class BarkionsBark : ModItem
{
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
    }

    public override void SetDefaults()
    {
        Item.width = 12;
        Item.height = 12;

        Item.damage = 6;
        Item.DamageType = DamageClass.Ranged;
        Item.knockBack = 0.25f;

        Item.maxStack = 9999;
        Item.consumable = true;

        Item.ammo = Item.type;
        Item.shoot = Mod.Find<ModProjectile>("BarkionsBarkProj").Type;
        Item.value = 5;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(15);
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ItemID.Wood, 25), (ItemID.Acorn, 3)], TileID.LivingLoom);
        recipe.Register();
    }
}
