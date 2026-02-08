using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

public class EoCDrops : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type != NPCID.EyeofCthulhu)
            return;

        // Classic only
        if (!Main.expertMode)
        {
            npcLoot.Add(ItemDropRule.Common(
                ModContent.ItemType<NaturiumOre>(),
                1,
                18,
                36
            ));
        }
    }
}
