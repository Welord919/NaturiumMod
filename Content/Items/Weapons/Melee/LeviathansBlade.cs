using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class LeviathansBlade : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/LeviathansBlade";

    public override void SetDefaults()
    {
        Item.width = 90;
        Item.height = 90;

        Item.damage = 78;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 8.5f;
        Item.crit = 6;

        Item.useTime = 21;
        Item.useAnimation = 21;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item1;

        Item.value = Item.buyPrice(0, 8, 0, 0);
        Item.rare = ItemRarityID.LightPurple;

        Item.noUseGraphic = false;
        Item.noMelee = false;
    }

    public override void HoldItem(Player player)
    {
        var mp = player.GetModPlayer<FabledBladePlayer>();
        mp.usingLeviathan = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<AndwraithsBlade>());
        recipe.AddIngredient(ItemID.SoulofMight, 10);
        recipe.AddIngredient(ItemID.SoulofSight, 10);
        recipe.AddIngredient(ItemID.SoulofFright, 10);
        recipe.AddIngredient(ItemID.HallowedBar, 15);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.Register();
    }
}
