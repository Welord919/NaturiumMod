using NaturiumMod.Content.Items.Accessories.PreHardmodeAccessories;
using NaturiumMod.Content.Items.Materials.PreHardmodeMaterials;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Common.Global;

class ExampleNPCShop : GlobalNPC
{
    public override void ModifyShop(NPCShop shop)
    {
        if (shop.NpcType == NPCID.Dryad)
        {
            shop.Add(ModContent.ItemType<BarkionsBark>());
            shop.Add(ModContent.ItemType<BarkionsMedallion>());
            shop.Add(ItemID.Vine);
        }
        else if (shop.NpcType == NPCID.Demolitionist)
        {
            shop.Add(ItemID.Diamond);
            shop.Add(ItemID.Ruby);
            shop.Add(ItemID.Emerald);
            shop.Add(ItemID.Sapphire);
            shop.Add(ItemID.Topaz);
            shop.Add(ItemID.Amethyst);
        }
    }
}
