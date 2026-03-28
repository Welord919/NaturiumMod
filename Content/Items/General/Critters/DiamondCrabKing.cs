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

namespace NaturiumMod.Content.Items.General.Critters
{
    public class DiamondCrabKingItem : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Critters/DiamondCrabKing";

        public override void SetDefaults()
        {
            Item.Size = new(12, 12);
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.noUseGraphic = true;
            Item.makeNPC = ModContent.NPCType<DiamondCrabKing>();
            Item.value += Item.buyPrice(0, 0, 77, 0);
            Item.rare = ItemRarityID.Blue;
        }
    }

    public class DiamondCrabKing : ModNPC
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Critters/DiamondCrabKingSprite";

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.Crab];
            int index = NPCID.Sets.NormalGoldCritterBestiaryPriority.IndexOf(NPCID.Crab);
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 24;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 5;

            NPC.catchItem = (short)ModContent.ItemType<DiamondCrabKingItem>();

            NPC.aiStyle = NPCAIStyleID.Passive;
            AIType = NPCID.Crab;

            NPC.noGravity = false;
            NPC.noTileCollide = false;

            NPC.knockBackResist = 0.8f;
        }
        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;

            // Same animation speed as vanilla Crab
            if (NPC.frameCounter >= 6)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if (NPC.frame.Y >= frameHeight * Main.npcFrameCount[NPC.type])
                    NPC.frame.Y = 0;
            }
        }
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                // Spawn gore pieces
                for (int i = 1; i <= 2; i++)
                {
                    int goreType = ModContent.Find<ModGore>($"NaturiumMod/DiamondCrabKingGore{i}").Type;
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, goreType);
                }
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool ocean = spawnInfo.Player.ZoneBeach;

            if (ocean)
                return 0.005f; // 0.5% spawn chance

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // 1/10 chance to drop 3–5 diamonds
            npcLoot.Add(ItemDropRule.Common(ItemID.Diamond, 10, 3, 5));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry entry)
        {
            entry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
                new FlavorTextBestiaryInfoElement("A rare crab infused with diamond energy. Has the number 52 written into it's arm for some reason.")
            });
        }
    }
    public class DiamondCrabKingGore1 : ModGore
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Critters/DCKGore1";
    }
    public class DiamondCrabKingGore2 : ModGore
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Critters/DCKGore2";
    }

}
