using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Accessories.CraftingTrees;
using NaturiumMod.Content.Items.Accessories.CraftingTrees.BaseGameCombos;
using NaturiumMod.Content.Items.Accessories.NaturiumMod.Content.Items.Accessories;
using NaturiumMod.Content.Items.Cards.LOB;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories
{
    public class UtopianHelm : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/UtopicHelm";
          
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(gold: 50);
            Item.defense = 12;
            // Allow alt-use while held
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.noUseGraphic = false;
        }
        public override bool AltFunctionUse(Player player) => true;

        // Right-click while holding the emblem toggles scope mode and sends chat feedback
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                var modPlayer = player.GetModPlayer<TitanociderPlayer>();

                // Toggle the scopeDisabled flag
                modPlayer.scopeDisabled = !modPlayer.scopeDisabled;

                // Immediate effect on the current player instance
                player.scope = !modPlayer.scopeDisabled;

                // Chat message
                if (modPlayer.scopeDisabled)
                {
                    Main.NewText("Scope OFF", Color.LightSkyBlue);
                }
                else
                {
                    Main.NewText("Scope ON", Color.LightGreen);
                }

                // Feedback sound
                SoundEngine.PlaySound(SoundID.MenuTick, player.Center);
            }

            return base.CanUseItem(player);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.maxMinions += 3;
            player.maxTurrets += 1;
            player.GetDamage(DamageClass.Summon) += 0.25f;
            player.GetKnockback(DamageClass.Summon) += 0.12f;
            player.GetAttackSpeed(DamageClass.Summon) += 0.15f;
            player.dangerSense = true;
            player.findTreasure = true;
            player.moveSpeed += 0.05f;
            player.GetModPlayer<MillenniumScarabPlayer>().ScarabEquipped = true;
            player.GetModPlayer<MillenniumScarabPlayer>().hasMillenniumScarab = true;
            player.GetModPlayer<MillenniumScarabPlayer>().CardDropBoost = player.GetModPlayer<CardDropPlayer>().CardDropBoost;

            var titan = player.GetModPlayer<TitanociderPlayer>();
            titan.hasTitanocider = true;
            player.GetDamage(DamageClass.Ranged) += 0.25f;
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.aggro -= 400;
            player.arrowDamage += 0.10f;
            player.hasMoltenQuiver = true;
            player.ammoBox = true;
            player.ammoPotion = true;
            if (!Main.dayTime)
                player.GetCritChance(DamageClass.Ranged) += 5;
            player.moveSpeed += 0.08f;
            if (!titan.scopeDisabled)
                player.scope = true;
            else
                player.scope = false;

            var boost = player.GetModPlayer<WeaponBoostPlayer>();
            boost.activeBoosts["Barkion"] = true;
            boost.activeBoosts["Exterio"] = true;
            boost.activeBoosts["Leodrake"] = true;
            boost.activeBoosts["Nibiru"] = true;
            player.GetModPlayer<IceDamagePlayer>().iceMedallionActive = true;
            player.GetModPlayer<FrostburnMinionPlayer>().frostburnMinions = true;

            player.GetAttackSpeed(DamageClass.Melee) += 0.10f;
            player.GetDamage(DamageClass.Generic) += 0.10f;
            player.GetCritChance(DamageClass.Generic) += 2;
            player.lifeRegen += 1;
            player.statDefense += 4;
            player.pickSpeed -= 0.15f;
            player.GetKnockback(DamageClass.Summon) += 0.5f;
            player.pStone = true;
            if (!Main.dayTime && !player.wet)
                player.wereWolf = true;
            if (player.wet)
            {
                player.merman = true;
                player.gills = true;
                player.accFlipper = true;
            }

            player.statManaMax2 += 40;
            player.manaCost -= 0.12f;
            player.manaRegenBonus += 25;
            player.manaMagnet = true;
            player.manaFlower = true;
            player.GetDamage(DamageClass.Magic) += 0.10f;
            player.GetCritChance(DamageClass.Magic) += 5;
            player.GetDamage(DamageClass.Generic) += 0.05f;

            player.GetDamage(DamageClass.Melee) += 0.15f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.25f;
            player.GetCritChance(DamageClass.Melee) += 5;
            player.autoReuseGlove = true;
            player.meleeScaleGlove = true;
            if (player.statLife < player.statLifeMax2 * 0.5f)
            {
                player.endurance += 0.10f;
                player.GetDamage(DamageClass.Melee) += 0.10f;
                player.GetAttackSpeed(DamageClass.Melee) += 0.05f;
                player.GetCritChance(DamageClass.Melee) += 5;
            }
            var fs = player.GetModPlayer<FlameSwordsmanPlayer>();
            fs.salamandraGauntletEquipped = true;

            player.autoPaint = true;
            player.equippedAnyWallSpeedAcc = true;
            player.equippedAnyTileSpeedAcc = true;
            player.equippedAnyTileRangeAcc = true;
            player.pickSpeed -= 0.25f;
            player.treasureMagnet = true;
        }

        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            int barkion = ModContent.ItemType<BarkionsMedallion>();
            int exterios = ModContent.ItemType<ExteriosMedallion>();
            int ice = ModContent.ItemType<IceBarrierMedallion>();
            int leodrake = ModContent.ItemType<LeodrakesMedallion>();
            int nibiru = ModContent.ItemType<NibiruMedallion>();
            int treeMed = ModContent.ItemType<TreeMedallion>();

            int fireGauntlet = ItemID.FireGauntlet;
            int berserkerGlove = ItemID.BerserkerGlove;

            int millenniumScarab = ModContent.ItemType<MillenniumScarab>();
            int salamandraGauntlet = ModContent.ItemType<SalamandrasGauntlet>();
            int celestialVitality = ModContent.ItemType<CelestialVitalityCore>();
            int celestialArcana = ModContent.ItemType<CelestialArcanaCore>();
            int titanocider = ModContent.ItemType<TitanociderEmblem>();
            int treeMedallion = ModContent.ItemType<TreeMedallion>();
            int handOfCreation = ItemID.HandOfCreation;

            bool IsOldMedallion(int type) =>
                type == barkion || type == exterios || type == ice ||
                type == leodrake || type == nibiru || type == treeMed;

            bool IsSalamandraConflict(int type) =>
                type == fireGauntlet || type == berserkerGlove;

            bool IsUtopianComponent(int type) =>
                type == millenniumScarab ||
                type == salamandraGauntlet ||
                type == celestialVitality ||
                type == celestialArcana ||
                type == titanocider ||
                type == treeMedallion ||
                type == handOfCreation;

            bool incomingIsUtopian = incomingItem.type == Type;
            bool equippedIsUtopian = equippedItem.type == Type;

            // -----------------------------------------------------
            // If equipping Utopian Gauntlet
            // -----------------------------------------------------
            if (incomingIsUtopian)
            {
                if (IsOldMedallion(equippedItem.type)) return false;
                if (IsSalamandraConflict(equippedItem.type)) return false;
                if (IsUtopianComponent(equippedItem.type)) return false;
            }

            // -----------------------------------------------------
            // If equipping an old medallion or Salamandra-conflict
            // -----------------------------------------------------
            if (IsOldMedallion(incomingItem.type) || IsSalamandraConflict(incomingItem.type))
            {
                if (equippedIsUtopian) return false;
            }

            // -----------------------------------------------------
            // If equipping a Utopian component
            // -----------------------------------------------------
            if (IsUtopianComponent(incomingItem.type))
            {
                if (equippedIsUtopian) return false;
            }

            // -----------------------------------------------------
            // Prevent equipping two Utopian Gauntlets
            // -----------------------------------------------------
            if (incomingIsUtopian && equippedIsUtopian)
                return false;

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MillenniumScarab>())
                .AddIngredient(ModContent.ItemType<SalamandrasGauntlet>())
                .AddIngredient(ModContent.ItemType<CelestialVitalityCore>())
                .AddIngredient(ModContent.ItemType<CelestialArcanaCore>())
                .AddIngredient(ModContent.ItemType<TreeMedallion>())
                .AddIngredient(ModContent.ItemType<TitanociderEmblem>())
                .AddIngredient(ItemID.HandOfCreation)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
