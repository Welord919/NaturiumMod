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
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/UtopianHelm";

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
            // -------------------------
            // Millennium Scarab effects
            // -------------------------
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

            // -------------------------
            // Titanocider Emblem effects (with scope applied)
            // -------------------------
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

            // Apply scope/zoom according to the persistent toggle stored in TitanociderPlayer
            // If the player has the emblem (now the gauntlet) and the toggle is NOT disabled, enable scope.
            if (!titan.scopeDisabled)
                player.scope = true;
            else
                player.scope = false;

            // -------------------------
            // Tree Medallion effects
            // -------------------------
            var boost = player.GetModPlayer<WeaponBoostPlayer>();
            boost.activeBoosts["Barkion"] = true;
            boost.activeBoosts["Exterio"] = true;
            boost.activeBoosts["Leodrake"] = true;
            boost.activeBoosts["Nibiru"] = true;

            player.GetModPlayer<IceDamagePlayer>().iceMedallionActive = true;
            player.GetModPlayer<FrostburnMinionPlayer>().frostburnMinions = true;
            //player.GetModPlayer<MinionInfoPlayer>().minionDisplayEquipped = true; Scarab already provides this

            // -------------------------
            // Celestial Vitality Core effects
            // -------------------------
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

            // -------------------------
            // Celestial Arcana Core effects
            // -------------------------
            player.statManaMax2 += 40;
            player.manaCost -= 0.12f;
            player.manaRegenBonus += 25;
            player.manaMagnet = true;
            player.manaFlower = true;
            player.GetDamage(DamageClass.Magic) += 0.10f;
            player.GetCritChance(DamageClass.Magic) += 5;
            player.GetDamage(DamageClass.Generic) += 0.05f;

            // -------------------------
            // Salamandra's Gauntlet effects
            // -------------------------
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
            // -------------------------
            // Hand of Creation effects
            // -------------------------
            player.autoPaint = true;
            player.equippedAnyWallSpeedAcc = true;
            player.equippedAnyTileSpeedAcc = true;
            player.equippedAnyTileRangeAcc = true;
            player.pickSpeed -= 0.25f;
            player.treasureMagnet = true;

        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            // Old medallion conflicts (same as TreeMedallion)
            int barkion = ModContent.ItemType<BarkionsMedallion>();
            int exterios = ModContent.ItemType<ExteriosMedallion>();
            int ice = ModContent.ItemType<IceBarrierMedallion>();
            int leodrake = ModContent.ItemType<LeodrakesMedallion>();
            int nibiru = ModContent.ItemType<NibiruMedallion>();
            int treeMed = ModContent.ItemType<TreeMedallion>();

            // Salamandra conflict items
            int fireGauntlet = ItemID.FireGauntlet;
            int berserkerGlove = ItemID.BerserkerGlove;

            // Materials / components used to craft the Utopian Gauntlet
            int millenniumScarab = ModContent.ItemType<MillenniumScarab>();
            int salamandraGauntlet = ModContent.ItemType<SalamandrasGauntlet>();
            int celestialVitality = ModContent.ItemType<CelestialVitalityCore>();
            int celestialArcana = ModContent.ItemType<CelestialArcanaCore>();
            int titanocider = ModContent.ItemType<TitanociderEmblem>();
            int treeMedallion = ModContent.ItemType<TreeMedallion>();
            int handOfCreation = ItemID.HandOfCreation;
            // If you later include UmiCore, add: int umiCore = ModContent.ItemType<UmiCore>();

            // Helper local to check if an item is one of the Utopian components
            bool IsUtopianComponent(int type) =>
                type == millenniumScarab ||
                type == salamandraGauntlet ||
                type == celestialVitality ||
                type == celestialArcana ||
                type == titanocider ||
                type == treeMedallion ||
                type == handOfCreation;

            // If equipping the Utopian Gauntlet, block if any old medallion or conflicting gauntlet is already equipped
            if (incomingItem.type == Type)
            {
                if (equippedItem.type == barkion || equippedItem.type == exterios || equippedItem.type == ice ||
                    equippedItem.type == leodrake || equippedItem.type == nibiru || equippedItem.type == treeMed ||
                    equippedItem.type == fireGauntlet || equippedItem.type == berserkerGlove)
                    return false;

                // Also block if any of the Utopian components are already equipped
                if (IsUtopianComponent(equippedItem.type))
                    return false;
            }

            // If equipping an old medallion or conflicting gauntlet, block if Utopian is already equipped
            if (incomingItem.type == barkion || incomingItem.type == exterios || incomingItem.type == ice ||
                incomingItem.type == leodrake || incomingItem.type == nibiru || incomingItem.type == treeMed ||
                incomingItem.type == fireGauntlet || incomingItem.type == berserkerGlove)
            {
                if (equippedItem.type == Type) return false;
            }

            // If equipping any of the Utopian components, block if Utopian is already equipped
            if (IsUtopianComponent(incomingItem.type))
            {
                if (equippedItem.type == Type) return false;
            }

            // If equipping Utopian, also block equipping any of its components in other accessory slots
            if (incomingItem.type == Type)
            {
                if (IsUtopianComponent(equippedItem.type)) return false;
            }

            // Finally, block equipping Fire Gauntlet / Berserker Glove when Utopian is equipped (Salamandra conflict)
            if (incomingItem.type == fireGauntlet || incomingItem.type == berserkerGlove)
            {
                if (equippedItem.type == Type) return false;
            }
            if (equippedItem.type == fireGauntlet || equippedItem.type == berserkerGlove)
            {
                if (incomingItem.type == Type) return false;
            }

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
