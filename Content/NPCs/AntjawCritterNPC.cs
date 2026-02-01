using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Items.General.Critters;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace NaturiumMod.Content.NPCs;

public class AntjawCritterNPC : ModNPC
{
    public override string Texture => "NaturiumMod/Assets/NPCs/AntjawCritterNPC";

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[Type] = 10;
        Main.npcCatchable[Type] = true;

        NPCID.Sets.CountsAsCritter[Type] = true;
        NPCID.Sets.TakesDamageFromHostilesWithoutBeingFriendly[Type] = true;
        NPCID.Sets.TownCritter[Type] = true;

        // Place in bestiary priority near vanilla butterfly
        int index = NPCID.Sets.NormalGoldCritterBestiaryPriority.IndexOf(NPCID.Butterfly);
        if (index >= 0)
        {
            NPCID.Sets.NormalGoldCritterBestiaryPriority.Insert(index + 1, Type);
        }
    }

    public override void SetDefaults()
    {
        NPC.Size = new(18, 13);
        NPC.aiStyle = NPCAIStyleID.Passive;
        NPC.damage = 0;
        NPC.defense = 0;
        NPC.lifeMax = 5;
        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;

        NPC.catchItem = ModContent.ItemType<AntjawCritter>();

        NPC.lavaImmune = false;

        NPC.CloneDefaults(NPCID.Butterfly);
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.frameCounter += 1.0;
        if (NPC.frameCounter > 3)
        {
            NPC.frameCounter = 0.0;
            NPC.frame.Y += frameHeight;
        }

        if (NPC.frame.Y > frameHeight * 9)
        {
            NPC.frame.Y = 0;
        }

        base.FindFrame(frameHeight);
    }

    public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
    {
        Texture2D critterTexture = TextureAssets.Npc[NPC.type].Value;

        Vector2 drawPosition = NPC.Center - screenPos + Vector2.UnitY;
        drawPosition.Y += DrawOffsetY;

        SpriteEffects direction = NPC.direction == 1
            ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        spriteBatch.Draw(critterTexture, drawPosition, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, NPC.frame.Size() * 0.5f, NPC.scale, direction, 0f);
        return false;
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
