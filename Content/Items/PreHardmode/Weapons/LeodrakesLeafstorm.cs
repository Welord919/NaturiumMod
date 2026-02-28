using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Helpers;
using System;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class LeodrakesLeafstorm : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/LeodrakesLeafstorm";

    public override void SetDefaults()
    {
        Item.damage = 18;
        Item.DamageType = DamageClass.Magic;
        Item.mana = 10;

        Item.width = 1;
        Item.height = 1;
        Item.useTime = 40;
        Item.useAnimation = 40;
        Item.useTurn = true;

        Item.shootSpeed = 12f;
        Item.knockBack = 0.5f;
        Item.crit = 8;
        Item.UseSound = SoundID.Item8;

        Item.value = Item.buyPrice(0, 0, 95, 0);
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
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 8),
            new(ModContent.ItemType<CameliaPetal>(), 12),
            new(ItemID.IronBar, 10)
        ], TileID.Anvils);
        recipe.Register();
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Vector2 origin = player.RotatedRelativePoint(player.MountedCenter);
        Vector2 aimDir = (Main.MouseWorld - origin).SafeNormalize(Vector2.UnitX * player.direction);
        Vector2 spawnPosition = origin + aimDir * 18f + new Vector2(0f, -6f * player.gravDir);

        float numberProjectiles = Main.rand.Next(3, 8);

        float baseRotation = Main.rand.NextFloat(MathHelper.TwoPi);

        for (int i = 0; i < numberProjectiles; i++)
        {
            float angle = baseRotation + MathHelper.TwoPi * (i / (float)numberProjectiles);

            Vector2 direction = new(MathF.Cos(angle), MathF.Sin(angle));

            float speedScale = Main.rand.NextFloat(0.9f, 1.1f);
            Vector2 speed = direction * Item.shootSpeed * speedScale;

            Vector2 jitter = Vector2.Zero;

            Projectile.NewProjectile(source, spawnPosition + jitter, speed, type, damage, knockback, player.whoAmI);
        }

        return false; // return false to stop vanilla from calling Projectile.NewProjectile.
    }
}
