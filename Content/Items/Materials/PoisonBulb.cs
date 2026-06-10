using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Materials
{
    public class PoisonBulb : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Materials/PoisonBulb";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.LightPurple;
        }
    }
    public class PoisonBulbDrop : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            // Poison-themed enemies
            bool isPoisonEnemy =
                npc.type == NPCID.Hornet ||
                npc.type == NPCID.BigHornetFatty ||
                npc.type == NPCID.BigHornetHoney ||
                npc.type == NPCID.BigHornetLeafy ||
                npc.type == NPCID.BigHornetSpikey ||
                npc.type == NPCID.BigHornetStingy ||
                npc.type == NPCID.BlackRecluse ||
                npc.type == NPCID.BlackRecluseWall ||
                npc.type == NPCID.JungleCreeper ||
                npc.type == NPCID.JungleCreeperWall ||
                npc.type == NPCID.GiantTortoise ||
                npc.type == NPCID.AngryTrapper ||
                npc.type == NPCID.MossHornet ||
                npc.type == NPCID.SpikedJungleSlime;

            if (!isPoisonEnemy)
                return;

            if (Main.rand.NextFloat() < 0.08f)
            {
                Item.NewItem(
                    npc.GetSource_Loot(),
                    npc.getRect(),
                    ModContent.ItemType<PoisonBulb>()
                );
            }
        }
    }
}
