using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PostHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using NaturiumMod.Content.Items.PreHardmode.Shark;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class HydrojetSharpshooter : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Shark/HydrojetSharpshooter";

    public override void SetDefaults()
    {
        Item.width = 42;
        Item.height = 21;

        Item.damage = 55;
        Item.DamageType = DamageClass.Ranged;
        Item.knockBack = 4.2f;
        Item.crit = 42; 

        Item.useTime = 21;
        Item.useAnimation = 21;
        Item.useStyle = ItemUseStyleID.Shoot;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item41;

        Item.shoot = ModContent.ProjectileType<HydrojetBeam>();
        Item.shootSpeed = 21f;
        Item.useAmmo = AmmoID.Bullet;

        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 12, 42, 0);
    }

    public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(source, position, velocity, Item.shoot, damage, knockback, player.whoAmI);
        return false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<SharkFinBlades>(), 9),
            new(ModContent.ItemType<InfusedNaturiumBar>(), 21),
            new(ItemID.WaterGun, 1)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
}

public class HydrojetBeam : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Shark/WaterBullet";

    public override void SetDefaults()
    {
        Projectile.width = 6;
        Projectile.height = 6;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.penetrate = 1;
        Projectile.timeLeft = 200;
        Projectile.extraUpdates = 3; // Hitscan feel
    }

    public override void AI()
    {
        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

        Dust d = Dust.NewDustDirect(Projectile.position, 2, 2, DustID.Water);
        d.scale = 1.2f;
        d.noGravity = true;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Wet, 180);
        target.AddBuff(BuffID.Slow, 120);
    }
}
