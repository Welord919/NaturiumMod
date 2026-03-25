using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.MillenniumItems;
using NaturiumMod.Content.Tiles;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Millennium
{
    public class MillenniumRing : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumRing";
        private enum RingMode
        {
            RareNPCs,
            RarestOre
        }

        private RingMode Mode
        {
            get => (RingMode)Main.LocalPlayer.GetModPlayer<RingModePlayer>().Mode;
            set => Main.LocalPlayer.GetModPlayer<RingModePlayer>().Mode = (int)value;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 5);
            Item.UseSound = SoundID.Item4;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<MillenniumPiece>(), 25),
            new(ItemID.LifeformAnalyzer, 1),
            new(ItemID.SpelunkerPotion, 5)
            ], TileID.Anvils);
            recipe.Register();
        }
        public override bool CanUseItem(Player player)
        {
            // RIGHT CLICK → Change mode
            if (player.altFunctionUse == 2)
            {
                CycleMode(player);
                return false;
            }

            // LEFT CLICK → Perform search
            PerformSearch(player);
            return false;
        }

        // ------------------------------
        // MODE CYCLING
        // ------------------------------
        private void CycleMode(Player player)
        {
            Mode = (RingMode)(((int)Mode + 1) % 2);

            if (Main.myPlayer == player.whoAmI)
            {
                string modeName = Mode switch
                {
                    RingMode.RareNPCs => "Rare NPCs",
                    RingMode.RarestOre => "Rarest Ore",
                    _ => "Unknown"
                };

                Main.NewText($"Millennium Ring mode set to: {modeName}", Color.Gold);
            }
        }

        // ------------------------------
        // SEARCH HANDLER
        // ------------------------------
        private void PerformSearch(Player player)
        {
            if (player.HasBuff(BuffID.Slow))
            {
                if (Main.myPlayer == player.whoAmI)
                    Main.NewText("The Ring needs time to recharge...", Color.Gray);
                return;
            }

            switch (Mode)
            {
                case RingMode.RareNPCs:
                    SearchRareNPCs(player);
                    break;

                case RingMode.RarestOre:
                    SearchRarestOre(player);
                    break;
            }

            player.AddBuff(BuffID.Slow, 180); // 3 second cooldown
        }

        // ------------------------------
        // RARE NPC SEARCH
        // ------------------------------
        private void SearchRareNPCs(Player player)
        {
            NPC target = FindNearestRareNPC(player);

            if (target != null)
            {
                DrawDustBeam(player.Center, target.Center);
                int tiles = (int)(Vector2.Distance(player.Center, target.Center) / 16f);

                if (Main.myPlayer == player.whoAmI)
                    Main.NewText($"The Ring points toward: {target.FullName} ({tiles} tiles away)", Color.Gold);
            }
            else
            {
                if (Main.myPlayer == player.whoAmI)
                    Main.NewText("The Ring senses no rare NPCs nearby...", Color.Gray);
            }
        }

        private NPC FindNearestRareNPC(Player player, float maxRange = 3000f)
        {
            NPC best = null;
            float bestDist = maxRange;

            // Truly rare / valuable NPCs only
            int[] rareNPCs =
            {
            // Transforming NPCs
            NPCID.LostGirl,
            NPCID.Nymph,

            // Extremely rare overworld spawns
            NPCID.RuneWizard,
            NPCID.Tim,
            NPCID.Pinky,
            NPCID.DoctorBones,

            // Dungeon high-value rare enemies
            NPCID.Paladin,
            NPCID.BoneLee,
            NPCID.SkeletonSniper,
            NPCID.TacticalSkeleton,
            NPCID.DiabolistRed,
            NPCID.DiabolistWhite,
            NPCID.Necromancer,
            NPCID.NecromancerArmored,
            NPCID.RaggedCaster,
            NPCID.RaggedCasterOpenCoat,

            // Mimics (all variants)
            NPCID.Mimic,
            NPCID.BigMimicCorruption,
            NPCID.BigMimicCrimson,
            NPCID.BigMimicHallow,
            NPCID.BigMimicJungle,

            // Bound NPCs
            NPCID.BoundGoblin,
            NPCID.BoundWizard,
            NPCID.BoundMechanic,
            NPCID.WebbedStylist,

            // Traveling NPCs
            NPCID.TravellingMerchant,
            NPCID.SkeletonMerchant,

            // Rare critters (gold)
            NPCID.GoldBird,
            NPCID.GoldBunny,
            NPCID.GoldButterfly,
            NPCID.GoldDragonfly,
            NPCID.GoldFrog,
            NPCID.GoldGrasshopper,
            NPCID.GoldMouse,
            NPCID.GoldSeahorse,
            NPCID.SquirrelGold,
            NPCID.GoldWaterStrider,
            NPCID.GoldWorm,

            // Hardmode rare overworld enemies
            NPCID.Moth,
            NPCID.IceGolem,
            NPCID.SandElemental,
            NPCID.RainbowSlime,

            // Solar Eclipse rare enemies
            NPCID.Reaper,
            NPCID.Eyezor,
            NPCID.Nailhead,
            NPCID.Psycho,

            // Pumpkin Moon rare enemies
            NPCID.HeadlessHorseman,

            // Frost Moon rare enemies
            NPCID.Yeti,
            NPCID.Krampus
        };

            foreach (NPC npc in Main.npc)
            {
                if (!npc.active)
                    continue;

                if (System.Array.IndexOf(rareNPCs, npc.type) != -1)
                {
                    float dist = Vector2.Distance(player.Center, npc.Center);
                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        best = npc;
                    }
                }
            }

            return best;
        }
        // Rarity ranking (0 = rarest)
        private static readonly int[] OreRarityOrder =
        {
            // Vanilla rarest
            TileID.LunarOre,
            TileID.Chlorophyte,
            TileID.Adamantite,
            TileID.Titanium,
            TileID.Orichalcum,
            TileID.Mythril,
            TileID.Palladium,
            TileID.Cobalt,

            ModContent.TileType<NibiricCrystalTile>(),
            // Special
            TileID.LifeFruit,
            TileID.Heart,

            // Pre-Hardmode rare
            TileID.Hellstone,
            TileID.Meteorite,
            ModContent.TileType<NaturiumOreTile>(),
            TileID.Crimtane,
            TileID.Demonite,


            // Pre-Hardmode common
            TileID.Platinum,
            TileID.Gold,
            TileID.Tungsten,
            TileID.Silver,
            TileID.Lead,
            TileID.Iron,
            TileID.Tin,
            TileID.Copper
            };
        private void SearchRarestOre(Player player)
        {
            Point? orePos = FindRarestOre(player);

            if (!orePos.HasValue)
            {
                if (Main.myPlayer == player.whoAmI)
                    Main.NewText("The Ring senses no ores nearby...", Color.Gray);
                return;
            }

            Vector2 worldPos = orePos.Value.ToVector2() * 16f;
            DrawDustBeam(player.Center, worldPos);

            Tile tile = Framing.GetTileSafely(orePos.Value.X, orePos.Value.Y);
            string oreName = GetOreName(tile.TileType);

            int tiles = (int)(Vector2.Distance(player.Center, worldPos) / 16f);

            if (Main.myPlayer == player.whoAmI)
                Main.NewText($"Rarest ore nearby: {oreName} ({tiles} tiles away)", Color.Gold);
        }

        private Point? FindRarestOre(Player player, int scanRadius = 120)
        {
            Point playerTile = player.Center.ToTileCoordinates();
            Point? bestPoint = null;
            int bestRarityIndex = int.MaxValue;
            float bestDist = float.MaxValue;

            // -------------------------
            // Load Calamity ores if installed
            // -------------------------
            List<int> calamityOres = new List<int>();

            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                // These names are correct for Calamity 1.5+
                calamityOres.Add(calamity.Find<ModTile>("AuricOre").Type);
                calamityOres.Add(calamity.Find<ModTile>("UelibloomOre").Type);
                calamityOres.Add(calamity.Find<ModTile>("AstralOre").Type);
                calamityOres.Add(calamity.Find<ModTile>("ScoriaOre").Type);
                calamityOres.Add(calamity.Find<ModTile>("PerennialOre").Type);
                calamityOres.Add(calamity.Find<ModTile>("CryonicOre").Type);
                calamityOres.Add(calamity.Find<ModTile>("AerialiteOre").Type);
                calamityOres.Add(calamity.Find<ModTile>("ExodiumOre").Type);
            }

            // -------------------------
            // Scan tiles around player
            // -------------------------
            for (int x = -scanRadius; x <= scanRadius; x++)
            {
                for (int y = -scanRadius; y <= scanRadius; y++)
                {
                    int checkX = playerTile.X + x;
                    int checkY = playerTile.Y + y;

                    if (!WorldGen.InWorld(checkX, checkY))
                        continue;

                    Tile tile = Framing.GetTileSafely(checkX, checkY);
                    if (!tile.HasTile)
                        continue;

                    int type = tile.TileType;

                    // -------------------------
                    // Check if tile is a vanilla or mod ore
                    // -------------------------
                    int rarityIndex = Array.IndexOf(OreRarityOrder, type);

                    // If not in vanilla/mod list, check Calamity
                    if (rarityIndex == -1 && calamityOres.Contains(type))
                    {
                        // Calamity ores are rarer than everything except Luminite
                        rarityIndex = -2;
                    }

                    // Not an ore we care about
                    if (rarityIndex == -1)
                        continue;

                    float dist = Vector2.Distance(player.Center, new Vector2(checkX * 16, checkY * 16));

                    // Better rarity OR same rarity but closer
                    if (rarityIndex < bestRarityIndex ||
                       (rarityIndex == bestRarityIndex && dist < bestDist))
                    {
                        bestRarityIndex = rarityIndex;
                        bestDist = dist;
                        bestPoint = new Point(checkX, checkY);
                    }
                }
            }

            return bestPoint;
        }

        private string GetOreName(int tileType)
        {
            // --- Naturium Mod Ores ---
            if (tileType == ModContent.TileType<NaturiumOreTile>())
                return "Naturium Ore";

            if (tileType == ModContent.TileType<NibiricCrystalTile>())
                return "Nibiric Crystal";

            // --- Calamity Mod Ores ---
            if (ModLoader.TryGetMod("CalamityMod", out Mod calamity))
            {
                if (tileType == calamity.Find<ModTile>("AerialiteOre").Type)
                    return "Aerialite Ore";

                if (tileType == calamity.Find<ModTile>("CryonicOre").Type)
                    return "Cryonic Ore";

                if (tileType == calamity.Find<ModTile>("PerennialOre").Type)
                    return "Perennial Ore";

                if (tileType == calamity.Find<ModTile>("ScoriaOre").Type)
                    return "Scoria Ore";

                if (tileType == calamity.Find<ModTile>("AstralOre").Type)
                    return "Astral Ore";

                if (tileType == calamity.Find<ModTile>("ExodiumOre").Type)
                    return "Exodium Cluster";

                if (tileType == calamity.Find<ModTile>("UelibloomOre").Type)
                    return "Uelibloom Ore";

                if (tileType == calamity.Find<ModTile>("AuricOre").Type)
                    return "Auric Ore";
            }

            // --- Vanilla Ores ---
            return tileType switch
            {
                // Hardmode
                TileID.LunarOre => "Luminite",
                TileID.Chlorophyte => "Chlorophyte",
                TileID.Adamantite => "Adamantite",
                TileID.Titanium => "Titanium",
                TileID.Orichalcum => "Orichalcum",
                TileID.Mythril => "Mythril",
                TileID.Palladium => "Palladium",
                TileID.Cobalt => "Cobalt",

                // Special
                TileID.LifeFruit => "Life Fruit",
                TileID.Heart => "Life Crystal",

                // Pre-Hardmode rare
                TileID.Hellstone => "Hellstone",
                TileID.Meteorite => "Meteorite",
                TileID.Crimtane => "Crimtane",
                TileID.Demonite => "Demonite",

                // Pre-Hardmode common
                TileID.Platinum => "Platinum",
                TileID.Gold => "Gold",
                TileID.Tungsten => "Tungsten",
                TileID.Silver => "Silver",
                TileID.Lead => "Lead",
                TileID.Iron => "Iron",
                TileID.Tin => "Tin",
                TileID.Copper => "Copper",

                _ => "Unknown Ore"
            };
        }


        // ------------------------------
        // GOLDEN DUST BEAM
        // ------------------------------
        private void DrawDustBeam(Vector2 start, Vector2 end)
        {
            Vector2 direction = (end - start).SafeNormalize(Vector2.Zero);
            float distance = Vector2.Distance(start, end);

            for (float i = 0; i < distance; i += 8f)
            {
                Vector2 pos = start + direction * i;
                Dust d = Dust.NewDustPerfect(pos, DustID.GoldFlame, Vector2.Zero, 150, Color.Yellow, 1.3f);
                d.noGravity = true;
            }
        }
    }

    // ------------------------------
    // PLAYER STORAGE FOR MODE
    // ------------------------------
    public class RingModePlayer : ModPlayer
    {
        public int Mode = 0;
    }
}