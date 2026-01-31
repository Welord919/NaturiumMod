using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using NaturiumMod.Content.Items.PreHardmode.Materials;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class TripleWOF : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/TripleWOF";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        Item.staff[Item.type] = true;
    }

    public override void SetDefaults()
    {
        Item.damage = 15;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 11;

        Item.width = 30;
        Item.height = 40;
        Item.useTime = 24;
        Item.useAnimation = 24;
        Item.shootSpeed = 12f;
        Item.knockBack = 6f;
        Item.crit = 8;
        Item.UseSound = SoundID.Item8;

        Item.value = Item.buyPrice(silver: 95);
        Item.rare = ItemRarityID.Green;
        Item.autoReuse = true;
        Item.noMelee = true;

        Item.useStyle = ItemUseStyleID.Shoot;

        Item.shoot = ProjectileID.WandOfFrostingFrost;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        float numberProjectiles = 3;
        float rotation = MathHelper.ToRadians(25);

        position += Vector2.Normalize(velocity) * 35f;

        for (int i = 0; i < numberProjectiles; i++)
        {
            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
            Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
        }

        return false; // return false to stop vanilla from calling Projectile.NewProjectile.
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ItemID.WandofFrosting, 3), (ModContent.ItemType<BarkionsBark>(), 15), (ItemID.Topaz, 3)], TileID.Anvils);
        recipe.AddRecipeGroup("Wood", 30);
        recipe.Register();

        Recipe recipe2 = CreateRecipe();
        recipe2 = RecipeUtils.GetNewRecipe(recipe2, [(ItemID.IceTorch, 297), (ModContent.ItemType<TripleWOS>(), 1)]);
        recipe2.Register();
    }
}
