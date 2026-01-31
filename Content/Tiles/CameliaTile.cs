using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Items.General.Placeable;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NaturiumMod.Content.Tiles
{
    public class CameliaTile : ModTile
    {
        // Number of growth stages (0 .. MaxStage-1). Each stage is one tile frame column (assumes 16px frames).
        private const int MaxStage = 4;
        public override string Texture => "NaturiumMod/Assets/Tiles/CameliaTile";

        public override void SetStaticDefaults()
        {
            // Important tile flags for an herb/plant
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileCut[Type] = true; // allow being cut by things that cut plants
            Main.tileNoFail[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch); // herb-like behaviour
            TileObjectData.newTile.CoordinateHeights = [16];
            TileObjectData.addTile(Type);

            AddMapEntry(new Color(90, 180, 80), Language.GetText("Camelia"));

            DustType = DustID.Grass;
            HitSound = SoundID.Grass;
            MineResist = 0.5f;
        }

        // Draw the tile with a small horizontal sway based on world wind (like Daybloom).
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Framing.GetTileSafely(i, j);

            // Determine source rectangle from tile frame
            int frameX = tile.TileFrameX;
            int frameY = tile.TileFrameY;
            Rectangle source = new Rectangle(frameX, frameY, 16, 16);

            // Get the tile texture (works for modded tile textures)
            Texture2D texture = TextureAssets.Tile[Type].Value;

            // Proper offscreen handling
            Vector2 offScreen = Main.drawToScreen ? Vector2.Zero : new Vector2(Main.offScreenRange, Main.offScreenRange);
            Vector2 position = new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + offScreen;

            // Lighting color so it blends with the world
            Color lightColor = Lighting.GetColor(i, j);

            // Wind influence (Main.windForVisuals is -1..1 depending on wind)
            float wind = Main.WindForVisuals;

            // Per-tile phase so adjacent plants don't all sway identically
            float phase = (float)(Main.GlobalTimeWrappedHourly * 2.0 + (i + j) * 0.3);

            // Final horizontal offset (tweak amplitude as desired)
            float offsetX = (float)(Math.Sin(phase) * wind * 2.5f);

            // Draw the tile shifted by the computed wind offset. Return false to skip default drawing.
            spriteBatch.Draw(texture, position + new Vector2(offsetX, 0f), source, lightColor, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            return false;
        }

        // Growth tick: randomly advance growth (called by world update)
        public override void RandomUpdate(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            int stage = tile.TileFrameX / 18;
            if (stage < MaxStage - 1)
            {
                // small chance to grow
                if (Main.rand.NextFloat() < 0.25f)
                {
                    tile.TileFrameX = (short)((stage + 1) * 18);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendTileSquare(-1, i, j, 1);
                    }
                }
            }
        }

        // Right click harvest (and kill the tile). Only if fully grown return flower + seeds.
        public override bool RightClick(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            int stage = tile.TileFrameX / 18;
            if (stage >= MaxStage - 1)
            {
                // drop one Camelia flower and 1-3 seeds
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Camelia>());
                int seedCount = Main.rand.Next(1, 4);
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<CameliaSeeds>(), seedCount);

                // Ensure we call Terraria's WorldGen to remove the tile (disambiguate project namespace)
                Terraria.WorldGen.KillTile(i, j, noItem: true);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                    NetMessage.SendTileSquare(-1, i, j, 1);

                SoundEngine.PlaySound(SoundID.Grass, new Microsoft.Xna.Framework.Vector2(i * 16, j * 16));
                return true;
            }

            return false;
        }

        // If tile is broken normally, drop seeds only when fully grown, otherwise nothing (or small chance)
        // NOTE: single-tile plants call KillTile, not KillMultiTile. Add KillTile override so breaking the plant
        // with a tool correctly drops items when fully grown.
        //
        // Pseudocode / Plan:
        // - This method runs when the tile is destroyed by a tool/player.
        // - If the destruction is flagged with noItem (meaning caller already suppressed drops), do nothing.
        // - Otherwise, read the tile's current frame to determine its growth stage.
        // - If stage is fully grown (>= MaxStage-1), spawn the Camelia item and 1-2 seeds.
        // - Use EntitySource_TileBreak for the Item.NewItem source.
        // - Don't change the incoming noItem flag here (we only early-return if it's already true).
        //
        // Implement the standard tModLoader KillTile signature (ref parameters) to match APIs that allow modifying flags.
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            // If the caller prevented item drops, respect that.
            if (noItem || effectOnly || fail)
                return;

            Tile tile = Framing.GetTileSafely(i, j);
            int stage = tile.TileFrameX / 18;
            if (stage >= MaxStage - 1)
            {
                // drop one Camelia flower and 1-2 seeds
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Camelia>());
                int seedCount = Main.rand.Next(1, 3);
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<CameliaSeeds>(), seedCount);
            }
        }

        // Keep the original KillMultiTile too (in case the tile ever becomes multi-tile).
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            int stage = frameX / 18;
            if (stage >= MaxStage - 1)
            {
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Camelia>());
                int seedCount = Main.rand.Next(1, 3);
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<CameliaSeeds>(), seedCount);
            }
        }

        // Prevent placement on bad tiles: allow only on Jungle grass and regular grass/mud (adjustable)
        public override bool CanPlace(int i, int j)
        {
            Tile tileBelow = Framing.GetTileSafely(i, j + 1);
            if (!tileBelow.HasTile)
                return false;

            int belowType = tileBelow.TileType;

            // Accept Jungle Grass, Grass, Mud as base.
            if (belowType == TileID.JungleGrass || belowType == TileID.Grass || belowType == TileID.Mud)
                return true;

            // Also allow placing on a Planter Box (so it can be planted in planters)
            if (belowType == TileID.PlanterBox)
                return true;

            return false;
        }
    }
}