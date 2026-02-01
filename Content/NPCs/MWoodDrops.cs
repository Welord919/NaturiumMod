using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.NPCs;

public class NPCLOOT : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type != NPCID.MourningWood)
        {
            return;
        }

        //npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DaltonPainting>(), 4, 1, 1));
    }
}
