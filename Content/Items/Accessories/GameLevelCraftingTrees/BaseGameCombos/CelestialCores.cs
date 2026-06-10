using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories.GameLevelCraftingTrees.BaseGameCombos
{
    // ===========================
    // Celestial Vitality Core
    // ===========================
    public class CelestialVitalityCore : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/CelestialVitalityCore";

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 10, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Celestial Shell "Always" effects
            player.GetAttackSpeed(DamageClass.Melee) += 0.10f;      // +10% melee speed
            player.GetDamage(DamageClass.Generic) += 0.10f;         // +10% all damage
            player.GetCritChance(DamageClass.Generic) += 2;         // +2% crit
            player.lifeRegen += 1;                                  // +1 HP/s
            player.statDefense += 4;                                // +4 defense
            player.pickSpeed -= 0.15f;                              // +15% mining speed
            player.GetKnockback(DamageClass.Summon) += 0.5f;        // +0.5 summon KB

            // Potion sickness reduction
            player.pStone = true;

            // Werewolf at night (not underwater)
            if (!Main.dayTime && !player.wet)
                player.wereWolf = true;

            // Merfolk while in water
            if (player.wet)
                player.merman = true;
                player.gills = true;
                player.accFlipper = true;

        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BandofRegeneration)
                .AddIngredient(ItemID.PhilosophersStone)
                .AddIngredient(ItemID.SunStone)
                .AddIngredient(ItemID.MoonStone)
                .AddIngredient(ItemID.MoonCharm)
                .AddIngredient(ItemID.NeptunesShell)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    // ===========================
    // Celestial Arcana Core
    // ===========================
    public class CelestialArcanaCore : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/CelestialArcanaCore";

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 10, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statManaMax2 += 40;          // max mana
            player.manaCost -= 0.12f;           // mana cost reduction
            player.manaRegenBonus += 25;        // mana regen
            player.manaMagnet = true;           // star/mana pickup range
            player.manaFlower = true;           // auto mana potions

            player.GetDamage(DamageClass.Magic) += 0.10f;  // Sorcerer Emblem
            player.GetCritChance(DamageClass.Magic) += 5;  // magic crit
            player.GetDamage(DamageClass.Generic) += 0.05f; // Putrid Scent generic damage
            player.aggro -= 400; // less likely to be targeted
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.BandofStarpower)
                .AddIngredient(ItemID.NaturesGift)
                .AddIngredient(ItemID.CelestialMagnet)
                .AddIngredient(ItemID.ManaRegenerationBand)
                .AddIngredient(ItemID.ManaFlower)
                .AddIngredient(ItemID.PutridScent)
                .AddIngredient(ItemID.SorcererEmblem)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
