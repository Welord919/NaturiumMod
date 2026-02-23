using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ID;

namespace NaturiumMod.Content.Tiles;

internal class EliteMudBrickTile : ModTile
{
    public override string Texture => "Terraria/Images/Tiles_" + TileID.LivingWood;


    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;

        TileID.Sets.IsATreeTrunk[Type] = TileID.Sets.IsATreeTrunk[TileID.LivingWood];



        DustType = DustID.Dirt;
        HitSound = SoundID.Dig;
        MineResist = 1.5f;
        MinPick = 45;

        AddMapEntry(new Color(75, 170, 30), Language.GetText("Elite Mud Bricks"));
    }
    public override bool CanKillTile(int i, int j, ref bool blockDamaged)
    {
        // Block mining until Queen Bee is defeated
        if (!NPC.downedQueenBee)
            return false;

        return base.CanKillTile(i, j, ref blockDamaged);
    }

    public override bool CanExplode(int i, int j)
    {
        // Also block bombs until Queen Bee is defeated
        if (!NPC.downedQueenBee)
            return false;

        return base.CanExplode(i, j);
    }

}
