using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using NaturiumMod.Content.Items.PreHardmode.Accessories;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using System;

namespace NaturiumMod.Content.Items.PostHardmode.Accessories
{
    // ============================================================
    //  ENUM — VENOM MODES
    // ============================================================
    public enum VenomMode
    {
        Melee,
        Ranged,
        Magic,
        Summoner
    }

    // ============================================================
    //  ITEM — GREEDY VENOM FUSION DRAGON RING
    // ============================================================
    public class GreedyVenomFusionDragonRing : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Accessories/GreedyVenomFusionDragonRing";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(gold: 20);
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            // Prevent equipping if Venom Fusion Dragon Ring is already equipped
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item item = player.armor[i];
                if (item != null && !item.IsAir)
                {
                    if (item.type == ModContent.ItemType<VenomFusionDragonRing>())
                    {
                        return false;
                    }
                }
            }

            return base.CanEquipAccessory(player, slot, modded);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<GreedyVenomFusionDragonPlayer>();
            mp.active = true;

            // ============================================================
            // INHERIT FUSIONIST RING EFFECTS
            // ============================================================
            player.GetModPlayer<FusionistRingPlayer>().fusionistRingActive = true;

            var boost = player.GetModPlayer<WeaponBoostPlayer>();
            boost.activeBoosts["Fusion"] = true;

            player.blockRange += 3;
            player.moveSpeed += 0.10f;
            player.runAcceleration *= 1.03f;
            player.discountAvailable = true;
            player.coinLuck += 0.30f;
            player.luck += 0.05f;

        }

        // Allow right-click toggle
        public override bool CanRightClick() => true;
        public override bool ConsumeItem(Player player) => false;

        public override void RightClick(Player player)
        {
            var mp = player.GetModPlayer<GreedyVenomFusionDragonPlayer>();

            mp.mode = mp.mode switch
            {
                VenomMode.Melee => VenomMode.Ranged,
                VenomMode.Ranged => VenomMode.Magic,
                VenomMode.Magic => VenomMode.Summoner,
                VenomMode.Summoner => VenomMode.Melee,
                _ => VenomMode.Melee
            };

            Main.NewText($"Greedy Venom Mode: {mp.mode}", new Color(120, 255, 120));
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VenomFusionDragonRing>(), 1);
            recipe.AddIngredient(ItemID.GreedyRing, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 10);
            recipe.AddIngredient(ModContent.ItemType<DarkEssence>(), 30);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }

    // ============================================================
    //  PLAYER — STORES MODE + ACTIVE FLAG
    // ============================================================
    public class GreedyVenomFusionDragonPlayer : ModPlayer
    {
        public bool active;
        public VenomMode mode = VenomMode.Melee;

        public override void ResetEffects()
        {
            active = false;
        }
    }

    // ============================================================
    //  GLOBAL NPC — VENOM COUNTERS
    // ============================================================
    public class GreedyVenomCounterGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int venomStacks;
        private int decayTimer;

        public override void ResetEffects(NPC npc)
        {
            decayTimer++;
            if (decayTimer >= 180) // 3 seconds
            {
                decayTimer = 0;
                if (venomStacks > 0)
                    venomStacks--;
            }
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (venomStacks > 0)
            {
                float bonus = 1f + (0.05f * venomStacks); // +5% per stack
                modifiers.SourceDamage *= bonus;
            }
        }
    }

    // ============================================================
    //  HELPER — APPLY VENOM + STACKS
    // ============================================================
    public static class GreedyVenomHelper
    {
        public static void ApplyVenom(NPC target)
        {
            target.AddBuff(BuffID.Venom, 180);

            var g = target.GetGlobalNPC<GreedyVenomCounterGlobalNPC>();
            g.venomStacks = Math.Min(g.venomStacks + 1, 10);
        }
    }

    // ============================================================
    //  GLOBAL ITEM — MELEE / RANGED / MAGIC / SUMMONER / CARD
    // ============================================================
    public class GreedyVenomFusionDragonGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var mp = player.GetModPlayer<GreedyVenomFusionDragonPlayer>();
            if (!mp.active)
                return;

            // CARD DAMAGE ALWAYS APPLIES
            if (item.DamageType == ModContent.GetInstance<CardDamage>())
            {
                GreedyVenomHelper.ApplyVenom(target);
                player.GetDamage(ModContent.GetInstance<CardDamage>()) *= 1.05f;
                return;
            }

            // MELEE MODE
            if (mp.mode == VenomMode.Melee && item.DamageType == DamageClass.Melee)
                GreedyVenomHelper.ApplyVenom(target);

            // RANGED MODE
            if (mp.mode == VenomMode.Ranged && item.DamageType == DamageClass.Ranged)
                GreedyVenomHelper.ApplyVenom(target);

            // MAGIC MODE
            if (mp.mode == VenomMode.Magic && item.DamageType == DamageClass.Magic)
                GreedyVenomHelper.ApplyVenom(target);

            // SUMMONER MODE (whips)
            if (mp.mode == VenomMode.Summoner && item.DamageType == DamageClass.SummonMeleeSpeed)
                GreedyVenomHelper.ApplyVenom(target);
        }

    }

    // ============================================================
    //  GLOBAL PROJECTILE — SAME LOGIC FOR PROJECTILES
    // ============================================================
    public class GreedyVenomFusionDragonGlobalProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[proj.owner];
            var mp = player.GetModPlayer<GreedyVenomFusionDragonPlayer>();
            if (!mp.active)
                return;

            // CARD DAMAGE ALWAYS APPLIES
            if (proj.DamageType == ModContent.GetInstance<CardDamage>())
            {
                GreedyVenomHelper.ApplyVenom(target);
                return;
            }

            // MELEE MODE (boomerangs, flails)
            if (mp.mode == VenomMode.Melee && proj.DamageType == DamageClass.Melee)
                GreedyVenomHelper.ApplyVenom(target);

            // RANGED MODE (arrows, bullets, thrown)
            if (mp.mode == VenomMode.Ranged && proj.DamageType == DamageClass.Ranged)
                GreedyVenomHelper.ApplyVenom(target);

            // MAGIC MODE
            if (mp.mode == VenomMode.Magic && proj.DamageType == DamageClass.Magic)
                GreedyVenomHelper.ApplyVenom(target);

            // SUMMONER MODE (minions + whips)
            if (mp.mode == VenomMode.Summoner && proj.DamageType == DamageClass.Summon)
                GreedyVenomHelper.ApplyVenom(target);
        }
    }
}
