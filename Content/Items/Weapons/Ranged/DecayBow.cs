using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Consumables;
using NaturiumMod.Content.Projectiles.Ranged;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons.Ranged;
    public class DecayBow : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Weapons/DecayBow";

        private static int fogShotCounter = 0;

        public override void SetDefaults()
        {
            Item.damage = 18;
            Item.DamageType = DamageClass.Ranged;
            Item.width = 24;
            Item.height = 48;
            Item.useTime = 26;
            Item.useAnimation = 26;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 3f;
            Item.value = Item.sellPrice(silver: 80);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item5;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 9f;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            type = ModContent.ProjectileType<DecayArrowProj>();

            fogShotCounter++;

            bool spawnFog = fogShotCounter >= 5;

            if (spawnFog)
                fogShotCounter = 0;

            int proj = Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);

            // Mark the arrow
            Main.projectile[proj].ai[0] = spawnFog ? 1f : 0f;

            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ItemID.TendonBow, 1),
                new(ModContent.ItemType<PlagueResin>(), 8)
            ], TileID.Anvils);
            recipe.Register();
            recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ItemID.DemonBow, 1),
                new(ModContent.ItemType<PlagueResin>(), 8)
            ]);
            recipe.Register();
        }
    }
