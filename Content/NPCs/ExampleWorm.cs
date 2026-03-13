using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace NaturiumMod.Content.NPCs
{
    public class PlagueWormHead : ModNPC
    {
        public override string Texture => "NaturiumMod/Assets/NPCs/PlagueWormHead";

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers draw = new()
            {
                Velocity = 1f,
                Direction = 1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, draw);
        }

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 26;

            NPC.lifeMax = 120;
            NPC.damage = 20;
            NPC.defense = 4;

            NPC.knockBackResist = 0f;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.behindTiles = true;

            NPC.aiStyle = NPCAIStyleID.Worm;
            AIType = NPCID.DiggerHead;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            Banner = Type;
        }

        public override void AI()
        {
            NPC.TargetClosest(true);

            if (NPC.localAI[0] == 0f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.realLife = NPC.whoAmI;

                int latest = NPC.whoAmI;
                int segments = 10;

                for (int i = 0; i < segments; i++)
                {
                    int body = NPC.NewNPC(
                        NPC.GetSource_FromAI(),
                        (int)NPC.Center.X,
                        (int)NPC.Center.Y,
                        ModContent.NPCType<PlagueWormBody>(),
                        NPC.whoAmI
                    );

                    Main.npc[body].realLife = NPC.whoAmI;
                    Main.npc[body].ai[2] = NPC.whoAmI;
                    Main.npc[body].ai[1] = latest;

                    latest = body;
                }

                int tail = NPC.NewNPC(
                    NPC.GetSource_FromAI(),
                    (int)NPC.Center.X,
                    (int)NPC.Center.Y,
                    ModContent.NPCType<PlagueWormTail>(),
                    NPC.whoAmI
                );

                Main.npc[tail].realLife = NPC.whoAmI;
                Main.npc[tail].ai[2] = NPC.whoAmI;
                Main.npc[tail].ai[1] = latest;

                NPC.localAI[0] = 1f;
                NPC.netUpdate = true;
            }
        }
    }

    public class PlagueWormBody : ModNPC
    {
        public override string Texture => "NaturiumMod/Assets/NPCs/PlagueWormBody";

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 26;

            NPC.lifeMax = 120;
            NPC.damage = 15;
            NPC.defense = 4;

            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.aiStyle = NPCAIStyleID.Worm;
            AIType = NPCID.DiggerBody;

            Banner = ModContent.NPCType<PlagueWormHead>();
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers hide = new() { Hide = true };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, hide);
        }
    }

    public class PlagueWormTail : ModNPC
    {
        public override string Texture => "NaturiumMod/Assets/NPCs/PlagueWormTail";

        public override void SetDefaults()
        {
            NPC.width = 26;
            NPC.height = 26;

            NPC.lifeMax = 120;
            NPC.damage = 10;
            NPC.defense = 4;

            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.aiStyle = NPCAIStyleID.Worm;
            AIType = NPCID.DiggerTail;

            Banner = ModContent.NPCType<PlagueWormHead>();
        }

        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers hide = new() { Hide = true };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, hide);
        }
    }
}