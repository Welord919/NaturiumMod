using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint;
using NaturiumMod.Content.Items.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.CardSpecific.Fusion.FusionAltar
{
    public class ShardofGreed : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/ShardofGreed";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 9999;
            Item.value = Item.buyPrice(silver: 7);
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ModContent.ItemType<PotofGreed>(), 1)
                .AddIngredient(ModContent.ItemType<GenomeExtractor>(), 1)
                .AddTile(ModContent.TileType<FusionAltarTile>())
                .Register();
        }

    }
}
