using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Items.Materials;
using NaturiumMod.Content.Projectiles.Magic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

public class NibiruSepter : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/NibiruSepter";

    public override void SetDefaults()
    {
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useAnimation = 36;
        Item.useTime = 36;
        Item.crit += 25;
        Item.width = 44;
        Item.height = 14;
        Item.UseSound = SoundID.Item12;
        Item.damage = 60;
        Item.noMelee = true;
        Item.value = Item.buyPrice(0, 20, 0, 0);
        Item.knockBack = 8f;
        Item.rare = ItemRarityID.Orange;
        Item.DamageType = DamageClass.Magic;
        Item.shootSpeed = 15f;
        Item.shoot = ModContent.ProjectileType<NibiruStarProj>();
        Item.mana = 12;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        for (int i = 0; i < 5; i++)
        {
            Vector2 perturbed = velocity.RotatedByRandom(MathHelper.ToRadians(6));
            Projectile.NewProjectile(source, position, perturbed, ModContent.ProjectileType<NibiruStarProj>(), damage, knockback, player.whoAmI);
        }
        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<Starsteel>(), 25);
        recipe.AddIngredient(ItemID.SpaceGun, 1);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}


