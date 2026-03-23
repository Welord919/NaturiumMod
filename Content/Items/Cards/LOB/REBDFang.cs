using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static AssGen.Assets;

namespace NaturiumMod.Content.Items.Cards.LOB
{
    public class REBDFang : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/REBDFang";

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<REBDFangPlayer>();
            modPlayer.rebdFangActive = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<REBD>(), 1),
        new(ModContent.ItemType<FireEssence>(), 3),
        new(ModContent.ItemType<DarkEssence>(), 3),
        new(ModContent.ItemType<NaturiumBar>(), 5),
        new(ModContent.ItemType<CharmBase>(), 1)
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }

    // ============================================================
    // PLAYER EFFECTS
    // ============================================================

    public class REBDFangPlayer : ModPlayer
    {
        public bool rebdFangActive;

        public override void ResetEffects()
        {
            rebdFangActive = false;
        }

        // ------------------------------------------------------------
        // 1. FIRE DAMAGE BOOST (ITEM DAMAGE)
        // ------------------------------------------------------------
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (!rebdFangActive)
                return;

            // 🔥 Specific boost ONLY for REBD
            if (item.type == ModContent.ItemType<REBD>())
            {
                damage *= 1.15f; // example: +15% boost
            }

            // Fire-tagged modded cards
            if (WeaponTag.ItemTags.TryGetValue(item.type, out var tags) &&
                tags.Contains("Fire"))
            {
                damage *= 1.03f;
            }

            // Vanilla + modded fire weapons
            if (FireWeaponRegistry.FireItems.Contains(item.type))
            {
                damage *= 1.03f;
            }
        }


        // ------------------------------------------------------------
        // 2. FIRE DAMAGE BOOST (PROJECTILE DAMAGE)
        // ------------------------------------------------------------
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!rebdFangActive)
                return;

            // 🔥 Specific boost ONLY for REBD projectiles
            if (proj.type == ModContent.ProjectileType<RedEyesFireball>() ||
                proj.type == ModContent.ProjectileType<RedEyesExplosion>())
            {
                modifiers.SourceDamage *= 1.15f; // same % as item
            }

            // Fire-tagged modded projectiles
            if (WeaponTagProj.ProjTags.TryGetValue(proj.type, out var tags) &&
                tags.Contains("Fire"))
            {
                modifiers.SourceDamage *= 1.03f;
            }

            // Vanilla + modded fire projectiles
            if (FireWeaponRegistry.FireProjectiles.Contains(proj.type))
            {
                modifiers.SourceDamage *= 1.03f;
            }

            if (proj.GetGlobalProjectile<REBDProjTag>().isREBD)
            {
                Player owner = Main.player[proj.owner];
                owner.GetModPlayer<REBDFangPlayer>().ReduceREBDSummoningSickness();
            }

        }
        public void ReduceREBDSummoningSickness()
        {
            if (!rebdFangActive)
                return;

            int buffID = ModContent.BuffType<SummoningSickness>();
            int index = Player.FindBuffIndex(buffID);

            if (index == -1)
                return;

            int current = Player.buffTime[index];

            current = (int)(current * 0.95f);

            Player.buffTime[index] = Math.Max(1, current);
        }

    }
}