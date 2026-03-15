using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Rares;

public class Gaia : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/Gaia";
    private int usesLeft = 3;
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 40;

        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.useTime = 20;
        Item.useAnimation = 20;

        Item.noUseGraphic = true;
        Item.noMelee = true;

        Item.damage = 20;
        Item.knockBack = 3f;
        Item.DamageType = ModContent.GetInstance<CardDamage>();

        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 3);

        Item.UseSound = SoundID.Item1;

        Item.shoot = ModContent.ProjectileType<GaiaChargeProjectile>();
        Item.shootSpeed = 0f;

        Item.maxStack = 999;
    }

    public override bool CanUseItem(Player player)
    {
        if (usesLeft <= 0)
            return false;
        if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
            return false;

        return true;
    }

    public override bool? UseItem(Player player)
    {
        usesLeft--;
        player.AddBuff(ModContent.BuffType<SummoningSickness>(), 25);

        Vector2 dir = (Main.MouseWorld - player.Center)
            .SafeNormalize(Vector2.UnitX);

        if (Main.myPlayer == player.whoAmI)
        {
            Projectile.NewProjectile(
                player.GetSource_ItemUse(Item),
                player.Center,
                dir,
                Item.shoot,
                Item.damage,
                Item.knockBack,
                player.whoAmI
            );
        }
        if (usesLeft <= 0)
        {
            Item.stack--;     
            usesLeft = 3;  
        }
        return true;
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        return false;
    }
}
public class GaiaChargeProjectile : ModProjectile
{
    private float distanceTraveled = 0f;

    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/GaiaChargeProjectile";

    public override void SetDefaults()
    {
        Projectile.width = 40;
        Projectile.height = 40;

        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = -1;

        Projectile.tileCollide = false; // ⭐ no getting stuck
        Projectile.ignoreWater = true;

        Projectile.timeLeft = 80; // ⭐ longer charge

        Projectile.DamageType = DamageClass.Melee;

        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;
    }

    public override void OnSpawn(IEntitySource source)
    {
        // ⭐ Normalize ONCE
        Projectile.velocity = Projectile.velocity.SafeNormalize(Vector2.UnitX) * 18f;
    }

    public override void AI()
    {
        // Rotate sprite to match direction
        Projectile.rotation = Projectile.velocity.ToRotation();

        // Move forward
        Projectile.position += Projectile.velocity;

        // Track distance
        distanceTraveled += Projectile.velocity.Length();

        // Optional light
        Lighting.AddLight(Projectile.Center, 0.2f, 0.9f, 0.2f);
    }

    public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
    {
        // ⭐ Stronger scaling
        float bonus = distanceTraveled / 100f; // scales faster
        bonus = MathHelper.Clamp(bonus, 0f, 5f); // up to +250% damage

        modifiers.SourceDamage *= 1f + bonus;
        modifiers.Knockback *= 1f + bonus;
    }
}