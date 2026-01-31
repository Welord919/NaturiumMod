using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Items.Materials.PreHardmodeMaterials;

namespace NaturiumMod.Content.Items.Materials.LateHardmodeMaterials;

public class ExteriosFang : ModItem
{
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 15;
    }

    public override void SetDefaults()
    {
        Item.width = 12;
        Item.height = 12;

        Item.damage = 12;
        Item.DamageType = DamageClass.Ranged;
        Item.knockBack = 1.25f;

        Item.maxStack = 9999;
        Item.consumable = true;

        Item.ammo = Item.type;
        Item.shoot = Mod.Find<ModProjectile>("ExteriosFangProj").Type;
        Item.value = 5000;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(20);
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<NaturiumBar>(), 10), (ItemID.SoulofLight, 1), (ItemID.SoulofNight, 1), (ItemID.SoulofMight, 1)], TileID.MythrilAnvil);
        recipe.Register();
    }
}
