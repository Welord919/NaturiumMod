using NaturiumMod.Content.Items.General.Placeable;
using NaturiumMod.Content.Items.PostHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Accessories;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.NPCs;

public class ManyGlobalNPC : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        int[] itemsToAdd = GetItemsToAdd(shop.NpcType);
        foreach (int item in itemsToAdd)
        {
            shop.Add(item);
        }
    }

    private static int[] GetItemsToAdd(int npcID)
    {
        return npcID switch
        {
            NPCID.Dryad =>
            [
                ModContent.ItemType<BarkionsBark>(),
                ModContent.ItemType<BarkionsMedallion>(),
                ModContent.ItemType<CameliaSeeds>(),
                ItemID.Vine
            ],
            NPCID.Demolitionist =>
            [
                ItemID.Diamond,
                ItemID.Ruby,
                ItemID.Emerald,
                ItemID.Sapphire,
                ItemID.Topaz,
                ItemID.Amethyst
            ],
            _ => []
        };
    }
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
    public class NaturesEssenceDrop : GlobalNPC

    {
        public override void OnKill(NPC npc)
        {
            Player player = Main.player[npc.lastInteraction];

            if (!Main.hardMode)
                return;

            if (!player.ZoneJungle || !player.ZoneRockLayerHeight)
                return;

            // Exclude statue-spawned NPCs
            if (npc.SpawnedFromStatue)
                return;

            // Exclude critters and town NPCs
            if (npc.friendly || npc.lifeMax <= 5)
                return;

            // Drop chance, 1/20
            if (Main.rand.NextBool(20))
            {
                Item.NewItem(
                    npc.GetSource_Loot(),
                    npc.Hitbox,
                    ModContent.ItemType<NaturesEssence>()
                );
            }
        }

    }
    public class AntiGravityGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;
        public bool hadAntiGravity;

        public override void ResetEffects(NPC npc)
        {
            if (!hadAntiGravity)
                npc.noGravity = false;

            hadAntiGravity = false;
        }
    }

}
