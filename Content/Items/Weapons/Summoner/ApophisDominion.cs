using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Projectiles.Summoner;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons.Summoner;

public class ApophisDominion : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/AnubisDominion";

    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;

        Item.useTime = 28;
        Item.useAnimation = 28;
        Item.useStyle = ItemUseStyleID.HoldUp;

        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(gold: 12);

        Item.noMelee = true;
        Item.mana = 15;

        Item.DamageType = DamageClass.Summon;
        Item.damage = 64;
        Item.knockBack = 3f;

        // Minion + Sentry
        Item.buffType = ModContent.BuffType<ApophisEyeBuff>();
        Item.buffType = ModContent.BuffType<AnubisJudgeBuff>();

        Item.shoot = ModContent.ProjectileType<ApophisEyeMinion>();
        Item.sentry = true;
    }

    public override bool AltFunctionUse(Player player) => true;

    public override bool CanUseItem(Player player)
    {
        // RIGHT CLICK — Obelisk
        if (player.altFunctionUse == 2)
        {
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.shoot = ModContent.ProjectileType<ApophisObeliskSentry>();
            Item.mana = 20;

            // Apply the correct buff
            Item.buffType = ModContent.BuffType<AnubisJudgeBuff>();

            return true;
        }

        // LEFT CLICK — Eye Minion
        Item.useStyle = ItemUseStyleID.HoldUp;
        Item.shoot = ModContent.ProjectileType<ApophisEyeMinion>();
        Item.mana = 15;

        // Apply the correct buff
        Item.buffType = ModContent.BuffType<ApophisEyeBuff>();

        return true;
    }


    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
     Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // RIGHT CLICK — Obelisk Sentry
        if (player.altFunctionUse == 2)
        {
            // Apply correct buff
            player.AddBuff(ModContent.BuffType<AnubisJudgeBuff>(), 2);

            // Count existing sentries and find the oldest one
            int sentryCount = 0;
            int oldestIndex = -1;
            int oldestTimeLeft = int.MaxValue;

            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile proj = Main.projectile[i];

                if (proj.active && proj.owner == player.whoAmI && proj.sentry)
                {
                    sentryCount++;

                    if (proj.timeLeft < oldestTimeLeft)
                    {
                        oldestTimeLeft = proj.timeLeft;
                        oldestIndex = i;
                    }
                }
            }

            // If at max, kill the oldest sentry (vanilla behavior)
            if (sentryCount >= player.maxTurrets && oldestIndex != -1)
            {
                Main.projectile[oldestIndex].Kill();
            }

            // Spawn Obelisk at cursor
            Projectile.NewProjectile(
                source,
                Main.MouseWorld,
                Vector2.Zero,
                ModContent.ProjectileType<ApophisObeliskSentry>(),
                damage,
                knockback,
                player.whoAmI
            );

            return false;
        }

        // LEFT CLICK — Apophis Eye Minion
        player.AddBuff(ModContent.BuffType<ApophisEyeBuff>(), 2);

        Projectile.NewProjectile(
            source,
            player.Center,
            Vector2.Zero,
            ModContent.ProjectileType<ApophisEyeMinion>(),
            damage,
            knockback,
            player.whoAmI
        );

        return false;
    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<JudgementofAnubis>(), 1),
            new(ModContent.ItemType<MillenniumRod>(), 1),
            new(ItemID.Ectoplasm, 15),
            new(ItemID.Pumpkin, 25),
            new(ItemID.SoulofNight, 10)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
}
