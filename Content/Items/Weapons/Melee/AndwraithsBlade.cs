using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class AndwraithsBlade : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/AndwraithsBlade";

    public override void SetDefaults()
    {
        Item.width = 70;
        Item.height = 70;

        Item.damage = 32;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 6.5f;
        Item.crit = 4;

        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;

        Item.value = Item.buyPrice(0, 0, 90, 0);
        Item.rare = ItemRarityID.Orange;

        Item.noUseGraphic = false;
        Item.noMelee = false;
    }

    public override void HoldItem(Player player)
    {
        var mp = player.GetModPlayer<FabledBladePlayer>();
        mp.usingAndwraith = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.BladeofGrass);
        recipe.AddIngredient(ItemID.FieryGreatsword);
        recipe.AddIngredient(ItemID.Bone, 10);
        recipe.AddIngredient(ItemID.DemoniteBar, 15);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}
