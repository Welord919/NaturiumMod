using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using NaturiumMod.Content.Items.Materials;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace NaturiumMod.Content.Items.Weapons;

public class LeodrakesLeafstorm : ModItem
{
    public override void SetDefaults()
    {
        Item.damage = 18;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 10;

        Item.width = 30;
        Item.height = 40;
        Item.useTime = 40;
        Item.useAnimation = 40;

        Item.shootSpeed = 12f;
        Item.knockBack = 0.5f;
        Item.crit = 8;
        Item.UseSound = SoundID.Item8;

        Item.value = Item.buyPrice(silver: 95);
        Item.rare = ItemRarityID.Green;
        Item.autoReuse = true;
        Item.noMelee = true;

        Item.useStyle = ItemUseStyleID.RaiseLamp;

        Item.shoot = Mod.Find<ModProjectile>("LeodrakesManeProj").Type;
        Item.shootSpeed = 9f;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<NaturiumBar>(), 8), (ItemID.Ruby, 24), (ItemID.IronBar, 10)], TileID.Anvils);
        recipe.Register();
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        float numberProjectiles = 3 + Main.rand.Next(5);
        float rotation = MathHelper.TwoPi;

        position += Vector2.Normalize(velocity) * 45f;

        for (int i = 0; i < numberProjectiles; i++)
        {
            Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / numberProjectiles));
            Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
        }

        return false; // return false to stop vanilla from calling Projectile.NewProjectile.
    }
}
