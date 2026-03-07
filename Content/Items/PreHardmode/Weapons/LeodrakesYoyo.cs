using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.General.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class LeodrakesYoyo : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/LeodrakesYoyo";
    private static readonly int[] _UnwantedPrefixes = [PrefixID.Terrible, PrefixID.Dull, PrefixID.Shameful, PrefixID.Annoying, PrefixID.Broken, PrefixID.Damaged, PrefixID.Shoddy];

    public override void SetStaticDefaults()
    {
        ItemID.Sets.Yoyo[Item.type] = true; // Used to increase the gamepad range when using Strings.
        ItemID.Sets.GamepadExtraRange[Item.type] = 16; // Increases the gamepad range. Some vanilla values: 4 (Wood), 10 (Valor), 13 (Yelets), 18 (The Eye of Cthulhu), 21 (Terrarian).
        ItemID.Sets.GamepadSmartQuickReach[Item.type] = true; // Unused, but weapons that require aiming on the screen are in this set.
    }

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;
        Item.rare = ItemRarityID.Green;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = 25; 
        Item.useAnimation = 25;
        Item.noMelee = true; 
        Item.noUseGraphic = true;
        Item.UseSound = SoundID.Item1; 

        Item.damage = 22; 
        Item.DamageType = DamageClass.MeleeNoSpeed; 
        Item.knockBack = 2.5f;
        Item.crit = 6; 
        Item.channel = true; // Set to true for items that require the attack button to be held out (e.g. yoyos and magic missile weapons)
        Item.value = Item.buyPrice(0, 2, 0, 0);

        Item.shoot = ModContent.ProjectileType<LeodrakesYoyoProj>();
        Item.shootSpeed = 11f;		
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 10),
            new(ModContent.ItemType<CameliaPetal>(), 10),
            new(ItemID.TissueSample, 10)
        ], TileID.LivingLoom);
        recipe.Register();

        recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumOre>(), 10),
            new(ModContent.ItemType<CameliaPetal>(), 10),
            new(ItemID.ShadowScale, 10)
        ], TileID.LivingLoom);
        recipe.Register();
    }

}

