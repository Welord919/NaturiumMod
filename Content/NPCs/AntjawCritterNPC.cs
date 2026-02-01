using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.General.Critters;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace NaturiumMod.Content.NPCs;

public class AntjawCritterNPC : ModNPC
{
    private const int ClonedNPCID = NPCID.Butterfly; // Use the vanilla butterfly for AI and animations
    public override string Texture => "NaturiumMod/Assets/NPCs/AntjawCritterNPC";

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = Main.npcFrameCount[ClonedNPCID];

        Main.npcCatchable[Type] = true;
        NPCID.Sets.CountsAsCritter[Type] = true;
        NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
        NPCID.Sets.TownCritter[Type] = true;

        // Place in bestiary priority near vanilla butterfly
        int index = NPCID.Sets.NormalGoldCritterBestiaryPriority.IndexOf(ClonedNPCID);
        if (index >= 0)
        {
            NPCID.Sets.NormalGoldCritterBestiaryPriority.Insert(index + 1, Type);
        }
    }

    public override void SetDefaults()
    {
        NPC.width = 18;
        NPC.height = 18;
        NPC.aiStyle = NPCAIStyleID.Passive;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        NPC.CloneDefaults(NPCID.Frog);

        NPC.catchItem = ModContent.ItemType<AntjawCritter>();

        NPC.lavaImmune = false;

        //AIType = ClonedNPCID;
        //AnimationType = ClonedNPCID;
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
    {
        bestiaryEntry.AddTags(BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime, BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface, new FlavorTextBestiaryInfoElement("Le Antjaw"));
    }

    public override float SpawnChance(NPCSpawnInfo spawnInfo) =>
        SpawnCondition.OverworldDay.Chance * 0.06f;


    public override Color? GetAlpha(Color drawColor)
    {
        // Apply an earthy green glow by shifting channels towards muted green tones.
        return drawColor with
        {
            R = Utils.Clamp<byte>(drawColor.R, 60, 120),   // reduce red component for less magenta
            G = Utils.Clamp<byte>(drawColor.G, 180, 240),  // emphasize green for the glow
            B = Utils.Clamp<byte>(drawColor.B, 60, 150),   // keep blue moderate to avoid cyan
            A = 255                                        // full opacity
        };
    }

    public override bool PreAI() => true;
}
