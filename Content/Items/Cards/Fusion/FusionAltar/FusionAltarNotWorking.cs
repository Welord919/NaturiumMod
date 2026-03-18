/*using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.Fusion.FusionAltar;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

// ============================================================
//  PLACEABLE WORKSTATION ITEM
// ============================================================
namespace NaturiumMod.Content.Items.Cards.Fusion.FusionAltar;

public class FusionAltarItem : ModItem
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
}

// ============================================================
//  WORKSTATION TILE
// ============================================================

public class FusionAltarTile : ModTile
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FusionAltar";
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        TileID.Sets.InteractibleByNPCs[Type] = true;

        AddMapEntry(new Color(200, 200, 200), CreateMapEntryName());
    }

    public override bool RightClick(int i, int j)
    {
        if (Main.myPlayer == Main.LocalPlayer.whoAmI)
        {
            FusionUI.Visible = true;
            Main.playerInventory = true; // REQUIRED
        }
        return true;
    }

    public override void MouseOver(int i, int j)
    {
        Player player = Main.LocalPlayer;
        player.noThrow = 2;
        player.cursorItemIconEnabled = true;
        player.cursorItemIconID = ModContent.ItemType<FusionAltarItem>();
    }
}
// ============================================================
//  ESSENCE ITEMS (6 TYPES)
// ============================================================

public abstract class BaseEssence : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 999;
        Item.rare = ItemRarityID.Blue;
    }
}

public class FireEssence : BaseEssence
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";
}
public class WaterEssence : BaseEssence
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";
}
public class EarthEssence : BaseEssence
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";
}
public class WindEssence : BaseEssence
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";
}
public class LightEssence : BaseEssence
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";
}
public class DarkEssence : BaseEssence
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";
}
public abstract class Polymerization : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/BaseEssence";

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 999;
        Item.rare = ItemRarityID.Blue;
    }
}
// ============================================================
//  ESSENCE EXTRACTION LOGIC
// ============================================================

public static class EssenceExtractor
{
    public static int GetEssenceYield(Item card)
    {
        // You can replace this with your card base class
        int rarity = card.rare;

        return rarity switch
        {
            ItemRarityID.White => 1,   // Common
            ItemRarityID.Blue => 2,    // Rare
            ItemRarityID.Green => 3,   // Shortprint / Super
            ItemRarityID.Orange => 5,  // Ultra
            _ => 1
        };
    }

    public static int GetEssenceItem(string attribute)
    {
        return attribute switch
        {
            "Fire" => ModContent.ItemType<FireEssence>(),
            "Water" => ModContent.ItemType<WaterEssence>(),
            "Earth" => ModContent.ItemType<EarthEssence>(),
            "Wind" => ModContent.ItemType<WindEssence>(),
            "Light" => ModContent.ItemType<LightEssence>(),
            "Dark" => ModContent.ItemType<DarkEssence>(),
            _ => 0
        };
    }
}

// ============================================================
//  FUSION RECIPE SYSTEM
// ============================================================

public static class FusionRecipes
{
    private static Dictionary<string, int> recipes = new();

    public static void LoadRecipes()
    {
        AddRecipe(
            new int[]
            {
                ModContent.ItemType<Firegrass>(),
                ModContent.ItemType<PetiteDragon>()
            },
            ModContent.ItemType<DarkfireDragon>()
        );
    }

    private static void AddRecipe(int[] ingredients, int result)
    {
        System.Array.Sort(ingredients);
        string key = string.Join(",", ingredients);
        recipes[key] = result;
    }

    public static int? TryFusion(List<Item> items)
    {
        int[] ids = items.Select(i => i.type).ToArray();
        System.Array.Sort(ids);
        string key = string.Join(",", ids);

        if (recipes.TryGetValue(key, out int result))
            return result;

        return null;
    }
}

// ============================================================
//  FUSION CRAFTING LOGIC
// ============================================================

public static class FusionCrafting
{
    public static Item TryCraft(Item polymerization, List<Item> ingredients)
    {
        if (polymerization.type != ModContent.ItemType<Polymerization>())
            return null;

        int? result = FusionRecipes.TryFusion(ingredients);
        if (result == null)
            return null;

        Item output = new Item();
        output.SetDefaults(result.Value);
        return output;
    }
}

// ============================================================
//  UI STATE WITH TWO MENUS
// ============================================================

public class FusionUI : UIState
{
    public static bool Visible;

    private UIPanel panel;
    private UIText tabEssence;
    private UIText tabFusion;

    private int currentTab = 0;

    public override void OnInitialize()
    {
        panel = new UIPanel();
        panel.Width.Set(400f, 0f);
        panel.Height.Set(300f, 0f);
        panel.HAlign = 0.5f;
        panel.VAlign = 0.5f;
        Append(panel);

        tabEssence = new UIText("Essence Extractor");
        tabEssence.Left.Set(20, 0);
        tabEssence.Top.Set(10, 0);
        tabEssence.OnLeftClick += (evt, el) => currentTab = 0;
        panel.Append(tabEssence);

        tabFusion = new UIText("Fusion Table");
        tabFusion.Left.Set(220, 0);
        tabFusion.Top.Set(10, 0);
        tabFusion.OnLeftClick += (evt, el) => currentTab = 1;
        panel.Append(tabFusion);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Visible)
            return;

        base.Draw(spriteBatch);

        if (currentTab == 0)
            DrawEssenceExtractor(spriteBatch);
        else
            DrawFusionTable(spriteBatch);
    }

    private void DrawEssenceExtractor(SpriteBatch sb)
    {
        Utils.DrawBorderString(sb, "Insert a card to extract essence", panel.GetDimensions().Position() + new Vector2(20, 60), Color.White);
    }

    private void DrawFusionTable(SpriteBatch sb)
    {
        Utils.DrawBorderString(sb, "Insert Polymerization + Essences", panel.GetDimensions().Position() + new Vector2(20, 60), Color.White);
    }
}*/