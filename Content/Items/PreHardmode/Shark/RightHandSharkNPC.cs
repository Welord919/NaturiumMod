using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Shark;



public class RightHandSharkNPC : ModNPC
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/RightHandSharkNPC";

    public override void SetStaticDefaults()
    {
        Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Shark];
    }

    public override void SetDefaults()
    {
        NPC.width = 60;
        NPC.height = 40;

        NPC.damage = 50;
        NPC.defense = 6;
        NPC.lifeMax = 120;

        NPC.knockBackResist = 0.7f;

        NPC.aiStyle = 16;
        AIType = NPCID.Shark;

        NPC.noGravity = true;
        NPC.noTileCollide = false;

        NPC.HitSound = SoundID.NPCHit1;
        NPC.DeathSound = SoundID.NPCDeath1;
    }

    public override void FindFrame(int frameHeight)
    {
        NPC.spriteDirection = NPC.direction;

        NPC.frameCounter++;

        if (NPC.frameCounter >= 6)
        {
            NPC.frameCounter = 0;
            NPC.frame.Y += frameHeight;

            if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                NPC.frame.Y = 0;
        }
    }


    public override float SpawnChance(NPCSpawnInfo spawnInfo)
    {
        bool ocean = spawnInfo.Player.ZoneBeach;
        bool underwater = spawnInfo.Water;

        if (ocean && underwater)
            return 0.15f; // 15% of ocean spawns

        return 0f;
    }

    public override void HitEffect(NPC.HitInfo hit)
    {
        if (NPC.life <= 0)
        {
            for (int i = 1; i <= 4; i++)
            {
                int goreType = ModContent.Find<ModGore>($"NaturiumMod/SharkGore{i}").Type;
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, goreType);
            }
        }
    }

    public override void ModifyNPCLoot(NPCLoot npcLoot)
    {
        npcLoot.Add(ItemDropRule.Common(ItemID.SharkFin, 1, 0, 3));
        npcLoot.Add(ItemDropRule.Common(ItemID.Fish, 2, 0, 1));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SharkFinBladesDamaged>(), 1, 0, 3));
        npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SharkFinBlades>(), 10, 1, 1));
    }

    public override void SetBestiary(BestiaryDatabase database, BestiaryEntry entry)
    {
        entry.Info.AddRange(new IBestiaryInfoElement[]
        {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("A vicious shark that attacks with its giant blades on its back.")
        });
    }
}

public class SharkGore1 : ModGore
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SharkGore1";
}
public class SharkGore2 : ModGore
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SharkGore2";
}
public class SharkGore3 : ModGore
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SharkGore3";
}
public class SharkGore4 : ModGore
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Shark/SharkGore4";
}
