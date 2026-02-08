using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Buffs;

public class SmithsBuff : ModBuff
{
    public float MultiplicativeDamageBonus { get; init; }
    public override string Texture => "NaturiumMod/Assets/Buffs/BarkionsBuff";

    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.debuff[Type] = false;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        // Artisan (Builder, Mining, Dangersense, Shine, Splunker)
        player.blockRange += 1;
        player.tileSpeed += 0.25f;
        player.wallSpeed += 0.25f;
        player.pickSpeed -= 0.25f;
        player.dangerSense = true;
        player.findTreasure = true;
        Lighting.AddLight(player.Center, 1f, 1f, 0.5f);

        // Angler (Fishing, Crate, Sonar, Gills, Water Walking)
        player.fishingSkill += 15;
        player.sonarPotion = true;
        player.cratePotion = true;
        player.gills = true;
        player.waterWalk = true;

        // Guardian (Ironskin, Endurance, Regen, Heartreach, Obsidian Skin)
        player.statDefense += 8;
        player.endurance += 0.10f;
        player.lifeRegen += 2;
        player.lifeMagnet = true;
        player.fireWalk = true;

        // Warrior (Rage, Wrath, Swiftness, Hunter, Inferno)
        player.GetCritChance(DamageClass.Generic) += 10;
        player.GetDamage(DamageClass.Generic) += 0.10f;
        player.moveSpeed += 0.25f;
        player.detectCreature = true;
        player.inferno = true;


        // Mystic (Magic Power, Mana Regen, Night Owl)
        player.GetDamage(DamageClass.Magic) += 0.20f;
        player.manaRegenBonus += 25;
        player.nightVision = true;
        player.maxMinions += 1;

    }

}