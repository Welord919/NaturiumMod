using NaturiumMod.Content.Items.General.Placeable;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.Weapons.Melee;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace NaturiumMod.Content.NPCs;

internal class Plaguespreader : ModNPC
{
    public override string Texture => "NaturiumMod/Assets/NPCs/Plaguespreader";

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.Zombie];

        NPCID.Sets.NPCBestiaryDrawModifiers value = new()
        {
            Velocity = 1f
        };
        NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
    }

    public override void SetDefaults()
    {
        NPC.CloneDefaults(NPCID.Zombie);

        NPC.damage = 18;
        NPC.defense = 4;
        NPC.lifeMax = 85;
        NPC.value = 200f;
        NPC.knockBackResist = 0.3f;

        AIType = NPCID.Zombie;
        AnimationType = NPCID.Zombie;

        Banner = NPC.type;
        BannerItem = ModContent.ItemType<PlaguespreaderBanner>();
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        // copy zombie drops
        var zombieDrops = Main.ItemDropsDB.GetRulesForNPCID(NPCID.Zombie, false);
        foreach (var rule in zombieDrops)
            npcLoot.Add(rule);

        // your custom drops
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlagueChunk>(), 1, 4, 7));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlaguespreaderArm>(), 20, 1, 1));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        bool inDesert = spawnInfo.Player.ZoneDesert;
        bool inCorrupt = spawnInfo.Player.ZoneCorrupt;
        bool inCrimson = spawnInfo.Player.ZoneCrimson;

        if (inDesert && (inCorrupt || inCrimson))
            return 0.5f;

        return 0f;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        IBestiaryInfoElement[] info =
        [
            new FlavorTextBestiaryInfoElement(
                "Once an ordinary zombie, now infected by a devious plague"
            )
        ];

        bestiaryEntry.Info.AddRange(info);
    }
}
