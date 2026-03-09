using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.ApophisItems;

public class MillenniumRod : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/MillenniumRod";

    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;

        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.useStyle = ItemUseStyleID.Thrust;

        Item.noMelee = true;
        Item.DamageType = DamageClass.Summon;
        Item.mana = 10;

        Item.damage = 10;
        Item.knockBack = 2f;

        Item.buffType = ModContent.BuffType<MillenniumRodBuff>();
        Item.shoot = ModContent.ProjectileType<MillenniumEye>();
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<MillenniumPiece>(), 12),
            new(ItemID.CrimsonRod, 1),
            new(ItemID.Amber, 10)
        ], TileID.Anvils);
        recipe.Register();

    }

    // Enable right-click
    public override bool AltFunctionUse(Player player) => true;

    public override Vector2? HoldoutOffset()
    {
        return new Vector2(-10, 0);
    }

    // This decides what happens on left vs right click
    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse == 2)
        {
            // RIGHT CLICK — confuse enemies
            Item.useStyle = ItemUseStyleID.HoldUp;
            ConfuseNearbyEnemies(player);
        }
        else
        {
            // LEFT CLICK — summon
            Item.useStyle = ItemUseStyleID.Thrust;
            Shoot(player, null, Vector2.Zero, Vector2.Zero, 0, 0, 0); // manually call Shoot to summon without delay
            Item.mana = 10;
        }

        return base.CanUseItem(player);
    }

    private void ConfuseNearbyEnemies(Player player)
    {
        // Sound effect
        SoundEngine.PlaySound(SoundID.Item27 with { Volume = 0.8f, Pitch = -0.2f }, player.Center);

        float radius = 300f;

        foreach (NPC npc in Main.ActiveNPCs)
        {
            if (!npc.CanBeChasedBy()) continue;

            if (Vector2.Distance(npc.Center, player.Center) <= radius)
            {
                npc.AddBuff(BuffID.Confused, 180); // 3 seconds
            }
        }
    }
    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // Prevent summoning on right-click
        if (player.altFunctionUse == 2)
            return false;

        // LEFT CLICK — summon
        SoundEngine.PlaySound(SoundID.Item44 with { Volume = 0.8f, Pitch = -0.1f }, player.Center);

        player.AddBuff(Item.buffType, 2);

        Projectile.NewProjectile(
            source,
            player.Center,
            Vector2.Zero,
            type,
            damage,
            knockback,
            player.whoAmI
        );

        return false;
    }
}
public class MillenniumRodBuff : ModBuff
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/MillenniumRodBuff";

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
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/MillenniumEye";

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.MinionSacrificable[Type] = true;
        ProjectileID.Sets.MinionTargettingFeature[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.width = 20;
        Projectile.height = 20;

        Projectile.friendly = false; // orbiters do NO damage
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

        // If the player no longer has the buff, kill the minion
        if (!player.HasBuff(ModContent.BuffType<MillenniumRodBuff>()))
        {
            Projectile.Kill();
            return;
        }

        // Keep the buff alive
        player.AddBuff(ModContent.BuffType<MillenniumRodBuff>(), 2);


        Projectile.timeLeft = 2;

        // --- Count all eyes ---
        int totalEyes = 0;
        foreach (Projectile p in Main.projectile)
            if (p.active && p.owner == player.whoAmI && p.type == Projectile.type)
                totalEyes++;

        if (totalEyes <= 0)
            totalEyes = 1;

        // --- Get index ---
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

        // --- Orbit logic ---
        float orbitRadius = 60f;
        float angleOffset = MathHelper.TwoPi / totalEyes;
        float globalRotation = player.miscCounter / 60f;
        float angle = globalRotation + index * angleOffset;

        Vector2 offset = new Vector2(
            (float)Math.Cos(angle),
            (float)Math.Sin(angle)
        ) * orbitRadius;

        Projectile.Center = player.Center + offset;

        // --- Targeting + Firing ---
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

        // Firing sound
        SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.7f, Pitch = -0.2f }, Projectile.Center);

        // FIRE DAMAGING PROJECTILE
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


