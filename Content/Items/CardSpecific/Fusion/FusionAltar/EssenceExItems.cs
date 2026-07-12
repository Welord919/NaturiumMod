using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.LOB.ShortPrint;
using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NaturiumMod.Content.Items.Cards.Fusion
{
    // ---------------------------
    //  FUSION ALTAR
    // ---------------------------
    public class EssenceExItems : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FusionAltar";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<FusionAltarTile>();
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<NaturiumBar>(), 25)
                .AddIngredient(ModContent.ItemType<Polymerization>(), 5)
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    public class FusionAltarTile : ModTile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FusionAltar";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16 };
            TileObjectData.addTile(Type);

            AddMapEntry(new Microsoft.Xna.Framework.Color(150, 50, 200), CreateMapEntryName());
        }
    }

    // ---------------------------
    //  ESSENCE EXTRACTOR
    // ---------------------------
    public class EssenceExtractor : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/EssenceExtractor";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 999;
            Item.value = 500;
            Item.rare = ItemRarityID.Blue;
        }

        public override void AddRecipes()
        {
            CreateRecipe(15)
                .AddIngredient(ModContent.ItemType<NaturiumBar>(), 5)
                .AddTile(ModContent.TileType<FusionAltarTile>())
                .Register();
        }
    }

    // ---------------------------
    //  ESSENCE ITEMS
    // ---------------------------
    public abstract class BaseEssence : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = 50;
            Item.rare = ItemRarityID.White;
        }
    }

    public class FireEssence : BaseEssence { public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FireEssence"; }
    public class WaterEssence : BaseEssence { public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/WaterEssence"; }
    public class EarthEssence : BaseEssence { public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/EarthEssence"; }
    public class WindEssence : BaseEssence { public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/WindEssence"; }
    public class LightEssence : BaseEssence { public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/LightEssence"; }
    public class DarkEssence : BaseEssence { public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/DarkEssence"; }
    public class SpellEssence : BaseEssence { public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/SpellEssence"; }
    public class TrapEssence : BaseEssence { public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/TrapEssence"; }
}
