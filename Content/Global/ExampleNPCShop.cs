using NaturiumMod.Content.Items.Accessories.PreHardmodeAccessories;
using NaturiumMod.Content.Items.Materials.PreHardmode;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Global;

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

    private int[] GetItemsToAdd(int npcID)
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
