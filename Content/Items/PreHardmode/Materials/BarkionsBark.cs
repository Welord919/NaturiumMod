using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class BarkionsBark : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/BarkionsBark";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);

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
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.Wood, 25),
            new(ItemID.Acorn, 3)
        ], TileID.LivingLoom);
        recipe.Register();
    }
}
