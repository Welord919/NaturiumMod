using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.NPCs;
using System;
using System.Collections.Generic;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using Terraria.ObjectData;

namespace NaturiumMod.Content.Tiles;

public class NatSlimeBannerTile : ModTile
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NatSlimeBannerTile";
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
        TileObjectData.newTile.Height = 3;
        TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(200, 200, 200), CreateMapEntryName());
    }

}
