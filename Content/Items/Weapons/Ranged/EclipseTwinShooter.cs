using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class EclipseTwinShooter : ModItem
{
    private int helixCharge = 0;

    public override string Texture => "NaturiumMod/Assets/Items/Weapons/EclipseTwinShooter";

    public override void SetDefaults()
    {
        Item.width = 58;
        Item.height = 24;

        Item.damage = 35;
        Item.DamageType = DamageClass.Ranged;
        Item.knockBack = 2f;
        Item.crit = 4;

        Item.useTime = 13;    
        Item.useAnimation = 13;
        Item.useStyle = ItemUseStyleID.Shoot;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item41;

        Item.shoot = ProjectileID.Bullet;
        Item.shootSpeed = 12f;
        Item.useAmmo = AmmoID.Bullet;

        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 4, 0, 0);
    }
    public override bool CanConsumeAmmo(Item ammo, Player player)
    {
        return Main.rand.NextFloat() >= 0.22f;
    }
    public override bool CanUseItem(Player player)
    {
        // Increase fire rate as helix tightens
        int t = helixCharge;

        // useTime goes from 13 → 6
        Item.useTime = Utils.Clamp(13 - (t / 10), 6, 13);
        Item.useAnimation = Item.useTime;

        return base.CanUseItem(player);
    }

    public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
    Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // Move spawn to barrel tip
        Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
        if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            position += muzzleOffset;

        // Increase helix charge while firing
        helixCharge++;
        if (helixCharge > 60)
            helixCharge = 60;

        // Helix angle goes from 30° → 5°
        float maxSpread = MathHelper.ToRadians(30f);
        float minSpread = MathHelper.ToRadians(5f);

        float spread = MathHelper.Lerp(maxSpread, minSpread, helixCharge / 60f);

        // Helix rotation offset
        float helixOffset = (float)(Main.GameUpdateCount * 0.25f);

        // Two bullets in opposite helix arcs
        Vector2 v1 = velocity.RotatedBy(spread * MathF.Sin(helixOffset));
        Vector2 v2 = velocity.RotatedBy(-spread * MathF.Sin(helixOffset));

        Projectile.NewProjectile(source, position, v1, type, damage, knockback, player.whoAmI);
        Projectile.NewProjectile(source, position, v2, type, damage, knockback, player.whoAmI);

        return false;
    }


    public override void HoldItem(Player player)
    {
        // If not firing, reset helix
        if (!player.controlUseItem)
        {
            helixCharge = 0;
        }
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.AdamantiteBar, 18);
        recipe.AddIngredient(ItemID.IllegalGunParts, 2);
        recipe.AddIngredient(ItemID.SoulofLight, 10);
        recipe.AddTile(TileID.MythrilAnvil);
        recipe.Register();
    }
}
