using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Materials;
using NaturiumMod.Content.Projectiles.Melee;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons.Melee;

public class ApophisFangblade : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/ApophisFangBlade";

    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 48;

        Item.damage = 26;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 5f;
        Item.crit = 6;

        Item.useTime = 26;
        Item.useAnimation = 26;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item20;

        Item.shoot = ModContent.ProjectileType<ApophisProjPlus>();
        Item.shootSpeed = 10f;

        Item.value = Item.buyPrice(0, 2, 50, 0);
        Item.rare = ItemRarityID.LightRed;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<ApophisSword>(), 1),
            new(ModContent.ItemType<PoisonBulb>(), 6),
            new(ItemID.SpiderFang, 12)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
    public override void ModifyItemScale(Player player, ref float scale)
    {
        scale = 0.5f;
    }
}

