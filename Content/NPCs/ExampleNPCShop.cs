using NaturiumMod.Content.Items.PreHardmode.Accessories;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.NPCs;

public class ExampleNPCShop : GlobalNPC
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
}
