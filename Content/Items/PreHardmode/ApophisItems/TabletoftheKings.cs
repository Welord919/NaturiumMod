using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace NaturiumMod.Content.Items.PreHardmode.ApophisItems;
public class TabletoftheKings : ModItem
    {
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/TabletoftheKings";
    public override void SetDefaults()
    {
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(gold: 4);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var boost = player.GetModPlayer<WeaponBoostPlayer>();
        boost.activeBoosts["Apophis"] = true;
        player.pickSpeed -= 0.25f;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<MillenniumPiece>(), 15),
            new(ItemID.AncientChisel, 1),
            new(ItemID.Amber, 10)
        ], TileID.Anvils);
        recipe.Register();

    }
}
    
