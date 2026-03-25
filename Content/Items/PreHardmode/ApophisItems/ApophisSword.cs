using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.General.Projectiles;
using NaturiumMod.Content.Items.PreHardmode.MillenniumItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.ApophisItems;
public class ApophisSword : ModItem
{
  
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/ApophisSword";

    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;

        Item.damage = 16; 
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 4.5f;
        Item.crit = 4;

        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item20; 

        Item.shoot = ModContent.ProjectileType<ApophisProj>();
        Item.shootSpeed = 8f;

        Item.value = Item.buyPrice(0, 0, 80, 0);
        Item.rare = ItemRarityID.Blue;

        Item.noMelee = false;
        Item.noUseGraphic = false;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.GoldBroadsword, 1),
            new(ItemID.Amber, 15)
        ], TileID.Anvils);
        recipe.Register();
    }
}
