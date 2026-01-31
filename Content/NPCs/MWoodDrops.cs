using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using NaturiumMod.Content.Tiles;

namespace NaturiumMod.Content.NPCs;

internal class NPCLOOT : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.MourningWood)
        {
            //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DaltonPainting>(), 4, 1, 1));
        }
    }
}
