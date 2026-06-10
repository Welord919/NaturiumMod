using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Consumables;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories
{
    public class VenomFusionDragonRing : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/VenomFusionDragonRing";

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Pink;
            Item.value = Item.buyPrice(gold: 12);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<VenomFusionDragonPlayer>();
            mp.active = true;

            // ============================================================
            // INHERIT FUSIONIST RING EFFECTS
            // ============================================================
            player.GetModPlayer<FusionistRingPlayer>().fusionistRingActive = true;

            var boost = player.GetModPlayer<WeaponBoostPlayer>();
            boost.activeBoosts["Fusion"] = true;

            player.blockRange += 2;
            player.moveSpeed += 0.10f;
            player.runAcceleration *= 1.03f;

        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            // Prevent equipping if Greedy Venom Fusion Dragon Ring is already equipped
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item item = player.armor[i];
                if (item != null && !item.IsAir)
                {
                    if (item.type == ModContent.ItemType<GreedyVenomFusionDragonRing>())
                    {
                        return false;
                    }
                }
            }

            return base.CanEquipAccessory(player, slot, modded);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<VenomRing>(), 1);
            recipe.AddIngredient(ModContent.ItemType<PlagueResin>(), 10);
            recipe.AddIngredient(ModContent.ItemType<DarkEssence>(), 10);
            recipe.AddIngredient(ModContent.ItemType<FusionistRing>(), 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
        public override bool CanRightClick() => true;

        public override bool ConsumeItem(Player player) => false;

        public override void RightClick(Player player)
        {
            var mp = player.GetModPlayer<VenomFusionDragonPlayer>();

            mp.mode = mp.mode switch
            {
                VenomMode.Melee => VenomMode.Ranged,
                VenomMode.Ranged => VenomMode.Melee,
                _ => VenomMode.Melee
            };

            Main.NewText($"Venom Mode: {mp.mode}", Color.LimeGreen);
        }
    }
    public enum VenomMode
    {
        Melee,
        Ranged
    }

    public class VenomFusionDragonPlayer : ModPlayer
    {
        public bool active;
        public VenomMode mode = VenomMode.Melee; // default

        public override void ResetEffects()
        {
            active = false;
        }
    }

    public class VenomCounterGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public int venomStacks;
        private int decayTimer;

        public override void ResetEffects(NPC npc)
        {
            // stacks persist but decay over time
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
                float bonus = 1f + 0.03f * venomStacks; // +3% per stack
                modifiers.SourceDamage *= bonus;
            }
        }
    }

    public class VenomFusionDragonGlobalItem : GlobalItem
    {
        public static void ApplyVenom(NPC target)
        {
            target.AddBuff(BuffID.Venom, 180);

            var g = target.GetGlobalNPC<VenomCounterGlobalNPC>();
            g.venomStacks = Math.Min(g.venomStacks + 1, 10);
        }
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var mp = player.GetModPlayer<VenomFusionDragonPlayer>();
            if (!mp.active)
                return;

            // CARD DAMAGE ALWAYS APPLIES
            if (item.DamageType == ModContent.GetInstance<CardDamage>())
            {
                ApplyVenom(target);
                return;
            }

            // MELEE MODE
            if (mp.mode == VenomMode.Melee && item.DamageType == DamageClass.Melee)
            {
                ApplyVenom(target);
            }

            // RANGED MODE
            if (mp.mode == VenomMode.Ranged && item.DamageType == DamageClass.Ranged)
            {
                ApplyVenom(target);
            }
        }

    }
    public class VenomFusionDragonGlobalProj : GlobalProjectile
    {
        public static void ApplyVenom(NPC target)
        {
            target.AddBuff(BuffID.Venom, 180);

            var g = target.GetGlobalNPC<VenomCounterGlobalNPC>();
            g.venomStacks = Math.Min(g.venomStacks + 1, 10);
        }
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player player = Main.player[proj.owner];
            var mp = player.GetModPlayer<VenomFusionDragonPlayer>();
            if (!mp.active)
                return;

            // CARD DAMAGE ALWAYS APPLIES
            if (proj.DamageType == ModContent.GetInstance<CardDamage>())
            {
                ApplyVenom(target);
                player.GetDamage(ModContent.GetInstance<CardDamage>()) *= 1.05f;
                return;
            }

            // MELEE MODE (boomerangs, flails, etc.)
            if (mp.mode == VenomMode.Melee && proj.DamageType == DamageClass.Melee)
            {
                ApplyVenom(target);
            }

            // RANGED MODE (bows, guns, thrown)
            if (mp.mode == VenomMode.Ranged && proj.DamageType == DamageClass.Ranged)
            {
                ApplyVenom(target);
            }
        }
    }

}
