using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace NaturiumMod.Content.NPCs
{
    internal class NaturiumSlime : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = Main.npcFrameCount[NPCID.BlueSlime];

            //NPCID.Sets.ShimmerTransformToNPC[NPC.type] = NPCID.Skeleton;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new() { Velocity = 1f };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BlueSlime);
            NPC.damage = 10;
            NPC.defense = 2;
            NPC.lifeMax = 60;
            NPC.value = 160f;
            NPC.knockBackResist = 0.5f;

            // NPC.aiStyle = 3;

            AIType = NPCID.BlueSlime;
            AnimationType = NPCID.BlueSlime;

            Banner = Item.NPCtoBanner(NPCID.BlueSlime);
            BannerItem = Item.BannerToItem(Banner);
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            var natDropRule = Main.ItemDropsDB.GetRulesForNPCID(NPCID.BlueSlime, false);
            foreach (var rule in natDropRule)
            {
                npcLoot.Add(rule);
            }

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<NaturiumOre>(), 1, 1, 6));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SpawnCondition.Cavern.Chance * 0.2f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            IBestiaryInfoElement[] bestiaryInfoElements = [new FlavorTextBestiaryInfoElement("This is SIGMA slime")];
            bestiaryEntry.Info.AddRange(bestiaryInfoElements);
        }
    }
}