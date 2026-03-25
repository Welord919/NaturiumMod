using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static AssGen.Assets;

namespace NaturiumMod.Content.Items.Cards.LOB
{
    public class REBDRing : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/REBDRing";

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
            var modPlayer = player.GetModPlayer<REBDRingPlayer>();
            modPlayer.rebdRingActive = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<REBDFang>(), 1),
        new(ModContent.ItemType<FireEssence>(), 5),
        new(ModContent.ItemType<DarkEssence>(), 5),
        new(ItemID.Meteorite, 10)
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }

    // ============================================================
    // PLAYER EFFECTS
    // ============================================================

    public class REBDRingPlayer : ModPlayer
    {
        public bool rebdRingActive;

        public override void ResetEffects()
        {
            rebdRingActive = false;
        }

        // ------------------------------------------------------------
        // 1. FIRE DAMAGE BOOST (ITEM DAMAGE)
        // ------------------------------------------------------------
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (!rebdRingActive)
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
                damage *= 1.07f;
            }

            // Vanilla + modded fire weapons
            if (FireWeaponRegistry.FireItems.Contains(item.type))
            {
                damage *= 1.07f;
            }
        }


        // ------------------------------------------------------------
        // 2. FIRE DAMAGE BOOST (PROJECTILE DAMAGE)
        // ------------------------------------------------------------
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!rebdRingActive)
                return;
            if (proj.type == ModContent.ProjectileType<RedEyesFireball>() ||
                proj.type == ModContent.ProjectileType<RedEyesExplosion>())
            {
                modifiers.SourceDamage *= 1.15f; // same % as item
            }
            // Fire-tagged modded projectiles
            if (WeaponTagProj.ProjTags.TryGetValue(proj.type, out var tags) &&
                tags.Contains("Fire"))
            {
                modifiers.SourceDamage *= 1.07f;
            }

            // Vanilla + modded fire projectiles
            if (FireWeaponRegistry.FireProjectiles.Contains(proj.type))
            {
                modifiers.SourceDamage *= 1.07f;
            }
        }


        // ------------------------------------------------------------
        // 2. SUMMONING SICKNESS REDUCTION
        // ------------------------------------------------------------
        public void ReduceSummoningSickness(bool killedWithREBD)
        {
            if (!rebdRingActive)
                return;

            int buffID = ModContent.BuffType<SummoningSickness>();
            int index = Player.FindBuffIndex(buffID);
            if (index == -1)
                return;

            int current = Player.buffTime[index];

            var modPlayer = Player.GetModPlayer<SSReduction>();
            if (!modPlayer.OriginalBuffTimes.ContainsKey(buffID))
                modPlayer.OriginalBuffTimes[buffID] = current;
            int original = modPlayer.OriginalBuffTimes[buffID];

            // floor = half of original
            int minAllowed = (int)(original * 0.5f);

            // compute proposed reduction from current remaining time
            int proposed = (int)(current * 0.97f);
            if (killedWithREBD)
                proposed = (int)(proposed * 0.95f);

            // clamp proposed to the floor
            if (proposed < minAllowed)
                proposed = minAllowed;

            // IMPORTANT: only shorten the timer — never increase it
            int newTime = Math.Min(current, proposed);
            if (newTime == current)
                return;

            // Apply change (see multiplayer note below)
            Player.buffTime[index] = newTime;
        }

    }
    public class SSReduction : ModPlayer
    {
        public Dictionary<int, int> OriginalBuffTimes = new();

        public override void PostUpdateBuffs()
        {
            // Record original buff times for any active buff we haven't seen yet
            for (int i = 0; i < Player.buffTime.Length; i++)
            {
                int type = Player.buffType[i];
                if (type > 0 && Player.buffTime[i] > 0)
                {
                    if (!OriginalBuffTimes.ContainsKey(type))
                        OriginalBuffTimes[type] = Player.buffTime[i];
                }
            }

            // Clean up entries for buffs that are no longer present
            // This prevents the dictionary from growing indefinitely
            var keysToRemove = new List<int>();
            foreach (var kvp in OriginalBuffTimes)
            {
                int buffType = kvp.Key;
                if (!Player.HasBuff(buffType))
                    keysToRemove.Add(buffType);
            }

            foreach (int k in keysToRemove)
                OriginalBuffTimes.Remove(k);
        }
    }


    // ============================================================
    // GLOBAL PROJECTILE TAG FOR REBD
    // ============================================================

    public class REBDProjTag : GlobalProjectile
    {
        public bool isREBD;

        public override bool InstancePerEntity => true;
    }

    // ============================================================
    // APPLY ON FIRE + TRIGGER SICKNESS REDUCTION
    // ============================================================

    public class REBDGlobalProj : GlobalProjectile
    {
        public override void OnHitNPC(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Only REBD projectiles apply OnFire
            if (proj.GetGlobalProjectile<REBDProjTag>().isREBD)
                target.AddBuff(BuffID.OnFire, 180);

            // Reduce summoning sickness
            Player owner = Main.player[proj.owner];
            bool isREBD = proj.GetGlobalProjectile<REBDProjTag>().isREBD;

            owner.GetModPlayer<REBDRingPlayer>().ReduceSummoningSickness(isREBD);
        }
    }
}