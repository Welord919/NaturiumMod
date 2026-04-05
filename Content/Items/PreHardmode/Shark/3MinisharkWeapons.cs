using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Shark
{
    // -------------------------
    // SharkDrake (Damage Path)
    // -------------------------
    public class SharkDrake : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SharkDrake";

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 18;   // slower attack speed
            Item.useTime = 18;
            Item.autoReuse = true;
            Item.damage = 20;         // higher single-shot damage
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 7.5f;
            Item.crit = 6;
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.width = 50;
            Item.height = 16;
            Item.UseSound = SoundID.Item40;
            Item.value = Item.buyPrice(0, 12, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.10f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark, 1)
                .AddIngredient(ModContent.ItemType<NaturiumBar>(), 20)
                .AddIngredient(ModContent.ItemType<SharkFinBlades>(), 6)
                .AddIngredient(ItemID.IllegalGunParts, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
    // -------------------------
    // SpiderShark (Crit Path)
    // -------------------------
    public class SpiderShark : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SpiderShark";

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 12;   // moderate speed
            Item.useTime = 12;
            Item.autoReuse = true;
            Item.damage = 12;         // moderate damage
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 5.5f;
            Item.crit = 18;           // high crit focus
            Item.shootSpeed = 12f;
            Item.noMelee = true;
            Item.width = 44;
            Item.height = 14;
            Item.UseSound = SoundID.Item40;
            Item.value = Item.buyPrice(0, 10, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark, 1) // base material
                .AddIngredient(ModContent.ItemType<NaturiumBar>(), 20)
                .AddIngredient(ModContent.ItemType<SharkFinBlades>(), 3)
                .AddTile(TileID.Anvils)
                .Register();
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.20f;
        }
    }
    // -------------------------
    // AeroShark (Fire Rate Path)
    // -------------------------
    public class AeroShark : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/AeroShark";

        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 8;    // very fast
            Item.useTime = 8;
            Item.autoReuse = true;
            Item.damage = 8;          // lower per-shot damage
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 2.5f;
            Item.crit = 6;
            Item.shootSpeed = 11f;
            Item.noMelee = true;
            Item.width = 40;
            Item.height = 12;
            Item.UseSound = SoundID.Item11; // faster gun sound
            Item.value = Item.buyPrice(0, 9, 0, 0);
            Item.rare = ItemRarityID.Orange;
            Item.shoot = ProjectileID.Bullet;
            Item.useAmmo = AmmoID.Bullet;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextFloat() >= 0.40f;
        }
        public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Slight spread for high fire-rate feel
            Vector2 perturbed = velocity.RotatedByRandom(MathHelper.ToRadians(6));
            Projectile.NewProjectile(source, position, perturbed * Item.shootSpeed, Item.shoot, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Minishark, 1)
                .AddIngredient(ModContent.ItemType<NaturiumBar>(), 20)
                .AddIngredient(ModContent.ItemType<SharkFinBladesDamaged>(), 8)
                .AddIngredient(ItemID.Gel, 25)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}