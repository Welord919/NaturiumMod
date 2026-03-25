using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.MillenniumItems;

public class MillenniumRod : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumRod";

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.useStyle = ItemUseStyleID.Thrust;
        Item.rare = ItemRarityID.Yellow;
        Item.noMelee = true;
        Item.DamageType = DamageClass.Summon;
        Item.mana = 10;
        Item.value = Item.buyPrice(gold: 5);
        Item.damage = 10;
        Item.knockBack = 2f;

        Item.buffType = ModContent.BuffType<MillenniumRodBuff>();
        Item.shoot = ModContent.ProjectileType<MillenniumEye>();
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<MillenniumPiece>(), 25),
            new(ItemID.CrimsonRod, 1),
            new(ItemID.Amber, 10)
        ], TileID.Anvils);
        recipe.Register();
    }

    public override bool AltFunctionUse(Player player) => true;

    public override Vector2? HoldoutOffset() => new Vector2(-10, 0);

    public override bool CanUseItem(Player player)
    {
        // RIGHT CLICK — Confusion Pulse
        if (player.altFunctionUse == 2)
        {
            if (player.HasBuff(BuffID.Slow))
            {
                if (Main.myPlayer == player.whoAmI)
                    Main.NewText("The Millennium Rod needs time to recharge...", Color.Gray);

                return false;
            }

            Item.useStyle = ItemUseStyleID.HoldUp;

            ConfuseNearbyEnemies(player);

            player.AddBuff(BuffID.Slow, 300); // 5 seconds

            return false;
        }

        // LEFT CLICK — normal summon behavior
        Item.useStyle = ItemUseStyleID.Thrust;
        Item.mana = 10;

        return true; // allow Shoot() to run normally
    }

    // SUMMON LOGIC GOES HERE — NOT IN CanUseItem
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // Prevent summoning on right-click
        if (player.altFunctionUse == 2)
            return false;

        // Play summon sound
        SoundEngine.PlaySound(SoundID.Item44 with { Volume = 0.8f, Pitch = -0.1f }, player.Center);

        // Apply buff
        player.AddBuff(Item.buffType, 2);

        // Spawn minion
        Projectile.NewProjectile(
            source,
            player.Center,
            Vector2.Zero,
            type,
            damage,
            knockback,
            player.whoAmI
        );

        return false; // prevent vanilla from spawning a second one
    }

    private void ConfuseNearbyEnemies(Player player)
    {
        SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.8f, Pitch = -0.2f }, player.Center);

        float radius = 300f;

        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (!npc.CanBeChasedBy()) continue;

            if (Vector2.Distance(npc.Center, player.Center) <= radius)
                npc.AddBuff(BuffID.Confused, 180);
        }
    }
}

public class MillenniumRodBuff : ModBuff
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumRodBuff";

    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.buffNoSave[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        if (player.ownedProjectileCounts[ModContent.ProjectileType<MillenniumEye>()] > 0)
            player.buffTime[buffIndex] = 18000;
        else
            player.DelBuff(buffIndex);
    }
}

public class MillenniumEye : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumEye";

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.MinionSacrificable[Type] = true;
        ProjectileID.Sets.MinionTargettingFeature[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 20;
        Projectile.height = 20;

        Projectile.friendly = false;
        Projectile.hostile = false;

        Projectile.minion = true;
        Projectile.minionSlots = 1f;

        Projectile.tileCollide = false;
        Projectile.penetrate = -1;
        Projectile.timeLeft = 18000;

        Projectile.DamageType = DamageClass.Summon;
    }

    public override void AI()
    {
        Player player = Main.player[Projectile.owner];

        if (!player.HasBuff(ModContent.BuffType<MillenniumRodBuff>()))
        {
            Projectile.Kill();
            return;
        }

        player.AddBuff(ModContent.BuffType<MillenniumRodBuff>(), 2);

        Projectile.timeLeft = 2;

        int totalEyes = 0;
        foreach (Projectile p in Main.projectile)
            if (p.active && p.owner == player.whoAmI && p.type == Projectile.type)
                totalEyes++;

        if (totalEyes <= 0)
            totalEyes = 1;

        int index = 0;
        int counter = 0;
        foreach (Projectile p in Main.projectile)
        {
            if (p.active && p.owner == player.whoAmI && p.type == Projectile.type)
            {
                if (p.whoAmI == Projectile.whoAmI)
                    index = counter;

                counter++;
            }
        }

        float orbitRadius = 60f;
        float angleOffset = MathHelper.TwoPi / totalEyes;
        float globalRotation = player.miscCounter / 60f;
        float angle = globalRotation + index * angleOffset;

        Vector2 offset = new Vector2(
            (float)Math.Cos(angle),
            (float)Math.Sin(angle)
        ) * orbitRadius;

        Projectile.Center = player.Center + offset;

        NPC target = FindTarget(player, 500f);
        if (target != null)
            FireAtTarget(player, target);
    }

    private NPC FindTarget(Player player, float range)
    {
        NPC closest = null;
        float dist = range;

        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (!npc.CanBeChasedBy()) continue;

            float d = Vector2.Distance(player.Center, npc.Center);
            if (d < dist)
            {
                dist = d;
                closest = npc;
            }
        }

        return closest;
    }

    private void FireAtTarget(Player player, NPC target)
    {
        if (Projectile.localAI[0] > 0)
        {
            Projectile.localAI[0]--;
            return;
        }

        Projectile.localAI[0] = 30;

        Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 10f;

        SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.7f, Pitch = -0.2f }, Projectile.Center);

        Projectile.NewProjectile(
            Projectile.GetSource_FromThis(),
            Projectile.Center,
            direction,
            ModContent.ProjectileType<ApophisProj>(),
            Projectile.originalDamage,
            0f,
            player.whoAmI
        );
    }
}