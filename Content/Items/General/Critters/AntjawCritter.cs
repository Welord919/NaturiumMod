using NaturiumMod.Content.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.General.Critters;

public class AntjawCritter : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/General/Critters/AntjawCritter";

    public override void SetStaticDefaults()
    {
        ItemID.Sets.IsLavaBait[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Frog);
        
        Item.Size = new(12, 12);
        Item.useStyle = ItemUseStyleID.Swing;
        Item.autoReuse = true;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.maxStack = 99;
        Item.consumable = true;
        Item.makeNPC = 361;
        Item.noUseGraphic = true;

        Item.makeNPC = ModContent.NPCType<AntjawCritterNPC>();
        Item.value += Item.buyPrice(0, 0, 30, 0);
        Item.rare = ItemRarityID.Blue;
    }
}
