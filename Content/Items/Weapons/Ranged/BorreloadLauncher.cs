using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons.Ranged
{
    public class BorrelRocketLauncher : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Weapons/BorreloadLauncher";

        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 26;

            // Basic stats
            Item.damage = 65;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 4f;
            Item.crit = 4;

            Item.useTime = 32;
            Item.useAnimation = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.autoReuse = true;

            Item.value = Item.buyPrice(gold: 10);
            Item.rare = ItemRarityID.LightRed;

            // Ammo & shooting
            Item.useAmmo = AmmoID.Rocket;
            Item.shoot = ProjectileID.RocketI;
            Item.shootSpeed = 10f;

            // Sound
            Item.UseSound = SoundID.Item11;
        }

        // Optional: slight Borrel-style damage boost for rocket projectiles
        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockBack)
        {
            // Slight Borrel-style boost
            damage = (int)(damage * 1.10f);
            velocity *= 1.05f;

            // Spawn the projectile manually
            Projectile.NewProjectile(source, position, velocity, type, damage, knockBack, player.whoAmI);
            return false; // prevent double-spawn
        }

        public override void AddRecipes()
        {
            // Upgrade from vanilla Rocket Launcher + Borrel-themed materials
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.RocketLauncher, 1);
            recipe.AddIngredient(ModContent.ItemType<Qliphite>(), 10);
            recipe.AddIngredient(ModContent.ItemType<InfusedNaturiumGunParts>(), 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}
