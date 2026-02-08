using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons;

public class FlameRocketSword : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/RocketSword";

    public override void SetDefaults()
    {
        Item.DamageType = DamageClass.Melee;
        Item.damage = 30;
        Item.width = 30;
        Item.height = 50;

        Item.useTime = 40;
        Item.useAnimation = 40;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.knockBack = 6f;
        Item.crit = 8;
        Item.UseSound = SoundID.Item1;

        Item.value = Item.buyPrice(0, 3, 0, 0);
        Item.rare = ItemRarityID.Green;
        Item.autoReuse = true;

        // Default left-click projectile (Rocket I)
        Item.shoot = ProjectileID.RocketI;
        Item.shootSpeed = 12f;
    }

    // Enable right-click alternate function
    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    // Change behavior depending on left/right click
    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2) // Right-click
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item11;
            Item.shoot = ProjectileID.RocketII;
            Item.shootSpeed = 14f;

            // Optional: cooldown or ammo usage rules can go here
        }
        else // Left-click
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.UseSound = SoundID.Item1;
            Item.shoot = ProjectileID.RocketI;
            Item.shootSpeed = 12f;
        }

        return base.CanUseItem(player);
    }

    // Apply fire debuff on melee hit
    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.OnFire, 180); // 3 seconds
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.IronBar, 25),
            new(ItemID.Wire, 10),
            new(ItemID.ExplosivePowder, 15)
        ], TileID.Anvils);
        recipe.Register();
    }
}

