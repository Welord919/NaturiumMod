using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace NaturiumMod.Content.Items.Weapons.PostHardmode.Late;

public class EmpressFS : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/PostHardmode/Late/EmpressFS";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        Item.staff[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 165;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 8;
        Item.width = 20;
        Item.height = 30;
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
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ItemID.FairyQueenMagicItem, 1), (ItemID.PiercingStarlight, 1), (ItemID.RainbowRod, 1)], TileID.MythrilAnvil);
        recipe.Register();
    }
}
