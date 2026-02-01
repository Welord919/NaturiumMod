using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PostHardmode.Weapons;

public class EmpressFS : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Weapons/EmpressFS";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        Item.staff[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.Size = new(20, 30);
        Item.damage = 165;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 8;
        Item.useTime = 5;
        Item.useAnimation = 5;
        Item.shootSpeed = 18f;
        Item.knockBack = 6f;
        Item.crit = 10;
        Item.UseSound = SoundID.Item9;
        Item.value = Item.buyPrice(platinum: 5);
        Item.rare = ItemRarityID.Purple;
        Item.autoReuse = true;
        Item.noMelee = true;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.shoot = ProjectileID.FairyQueenMagicItemShot;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.FairyQueenMagicItem, 1),
            new(ItemID.PiercingStarlight, 1),
            new(ItemID.RainbowRod, 1)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
}
