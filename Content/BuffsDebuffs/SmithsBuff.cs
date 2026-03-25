using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Buffs;

public class SmithsBuff : ModBuff
{
    public float MultiplicativeDamageBonus { get; init; }
    public override string Texture => "NaturiumMod/Assets/Buffs/SmithsCharmBuff";

    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.debuff[Type] = false;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        // --- Apply your custom effects (same as before) ---
        player.blockRange += 1;
        player.tileSpeed += 0.25f;
        player.wallSpeed += 0.25f;
        player.pickSpeed -= 0.25f;
        player.dangerSense = true;
        player.findTreasure = true;
        Lighting.AddLight(player.Center, 1f, 1f, 0.5f);

        player.fishingSkill += 15;
        player.sonarPotion = true;
        player.cratePotion = true;
        player.gills = true;
        player.waterWalk = true;

        player.statDefense += 8;
        player.endurance += 0.10f;
        player.lifeRegen += 2;
        player.lifeMagnet = true;
        player.fireWalk = true;

        player.GetCritChance(DamageClass.Generic) += 10;
        player.GetDamage(DamageClass.Generic) += 0.10f;
        player.moveSpeed += 0.25f;
        player.detectCreature = true;
        player.inferno = true;

        player.GetDamage(DamageClass.Magic) += 0.20f;
        player.manaRegenBonus += 25;
        player.nightVision = true;
        player.maxMinions += 1;

        // --- Prevent stacking with base buffs ---
        // Add or remove buff IDs here to match the base buffs you want blocked.
        // Example list includes common builder/mining/fishing/guardian/warrior/magic buffs.
        int[] conflictingBuffs = new int[]
        {
            BuffID.Builder,       // Builder / Construction
            BuffID.Mining,        // Mining
            BuffID.Spelunker,     // Spelunker
            BuffID.Dangersense,   // Dangersense
            BuffID.Shine,         // Shine
            BuffID.Fishing,       // Fishing (if present in your tML version)
            BuffID.Crate,         // Crate potion
            BuffID.Sonar,         // Sonar (if present)
            BuffID.Gills,         // Gills
            BuffID.WaterWalking,  // Water Walking
            BuffID.Ironskin,      // Ironskin
            BuffID.Endurance,     // Endurance
            BuffID.Regeneration,  // Regen
            BuffID.Heartreach,    // Heartreach
            BuffID.ObsidianSkin,  // Obsidian Skin
            BuffID.Rage,          // Rage
            BuffID.Wrath,         // Wrath
            BuffID.Swiftness,     // Swiftness
            BuffID.Hunter,        // Hunter
            BuffID.Inferno,       // Inferno
            BuffID.MagicPower,    // Magic Power
            BuffID.ManaRegeneration, // Mana Regen
            BuffID.NightOwl       // Night Owl
        };

        // Clear and immunize conflicting buffs while this buff is active
        for (int i = 0; i < conflictingBuffs.Length; i++)
        {
            int b = conflictingBuffs[i];
            if (b <= 0) continue;

            // If the player currently has that buff, remove it
            if (player.HasBuff(b))
            {
                player.ClearBuff(b);
            }

            // Prevent reapplication while our buff is active
            player.buffImmune[b] = true;
        }
    }
}