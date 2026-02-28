using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.General.Projectiles;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.ModPlayers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Tools;

public class StarsteelPickaxe : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Tools/StarsteelPickaxe";

    public override void SetDefaults()
    {
        Item.damage = 30;
        Item.DamageType = DamageClass.Melee;
        Item.Size = new(40, 40);
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.knockBack = 15;
        Item.value = Item.buyPrice(0, 10, 0, 0);
        Item.rare = ItemRarityID.Blue;
        Item.UseSound = SoundID.Item1;
        Item.autoReuse = true;
        Item.tileBoost = 3;
        Item.pick = 105;
        Item.attackSpeedOnlyAffectsWeaponAnimation = true;
        Item.shoot = ProjectileID.None;
        Item.shootSpeed = 0f;
    }

    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    public override bool CanUseItem(Player player)
    {
        var modPlayer = player.GetModPlayer<StarsteelPlayer>();

        // Always reset to pickaxe mode first
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.noMelee = false;
        Item.shoot = ProjectileID.None;
        Item.shootSpeed = 0f;

        // Right-click attempt
        if (player.altFunctionUse == 2)
        {
            if (modPlayer.starsteelCooldown > 0)
                return false; // still cooling down

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.noMelee = true;
            Item.shoot = ProjectileID.StarCannonStar;
            Item.shootSpeed = 12f;
        }

        return true;
    }


    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        if (player.altFunctionUse == 2)
        {
            Projectile.NewProjectile(
                source,
                position,
                velocity * 1.2f,
                ProjectileID.StarCannonStar,
                damage + 10,
                knockback + 2f,
                player.whoAmI
            );

            // Apply cooldown (60 ticks = 1 second)
            player.GetModPlayer<StarsteelPlayer>().starsteelCooldown = 60;

            // Reset to pickaxe mode immediately
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noMelee = false;
            Item.shoot = ProjectileID.None;
            Item.shootSpeed = 0f;
        }

        return false;
    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<Starsteel>(), 15),
            new(ItemID.MoltenPickaxe, 1)
        ], TileID.Anvils);
        recipe.Register();
    }
}
