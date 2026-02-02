using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace NaturiumMod.Content.Items.General.Placeable;

public class NatSlimeBanner : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/General/Placeable/NatSlimeBanner";
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 10;
        Item.height = 24;
        Item.maxStack = 99;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.consumable = true;

        Item.createTile = ModContent.TileType<Tiles.NatSlimeBannerTile>();
        Item.placeStyle = 0;
    }

}
