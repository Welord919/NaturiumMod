using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;
    public class ReapersArm : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/ReapersArm";
        public override void SetDefaults()
        {
        Item.damage = 30;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 5;
        Item.width = 26;
        Item.height = 26;
        Item.useTime = 60;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Thrust;
        Item.noMelee = true;
        Item.channel = true; //Channel so that you can held the weapon [Important]
        Item.knockBack = 4;
        Item.value = Item.sellPrice(silver: 90);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item9;
        Item.shoot = ProjectileType<General.Projectiles.SpiritScytheProj>();
        Item.shootSpeed = 12f;
        }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<PlagueChunk>(), 10),
            new(ItemID.ZombieArm, 1),
            new(ItemID.Bone, 20)
        ], TileID.Anvils);
        recipe.Register();
    }
}
