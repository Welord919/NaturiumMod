using Microsoft.Xna.Framework;
using NaturiumMod.Content.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NaturiumMod.Content.Items.PreHardmode.MillenniumItems
{
    public class MillenniumScale : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumScale";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 99;

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.consumable = true;
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Yellow;

            Item.createTile = ModContent.TileType<MillenniumScaleTile>();
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MillenniumPiece>(), 15);
            recipe.AddIngredient(ItemID.Bone, 10);
            recipe.AddIngredient(ItemID.GoldBar, 5);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();
        }
    }
    public class MillenniumScaleTile : ModTile
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumScale";

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = false;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);

            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.CoordinatePadding = 0;

            TileObjectData.addTile(Type);

            AddMapEntry(new Color(90, 180, 80), Language.GetText("Millennium Scale"));
        }



        public override bool RightClick(int i, int j)
        {
            Player player = Main.LocalPlayer;

            // Player luck
            float luck = player.luck;

            // Your mod’s kill tracker (from Mysterious Grandpa / KillTracker)
            int kills = CardQuestWorld.totalCardDamageKills;

            // Message
            Main.NewText(
                $"The Millennium Scale judges your soul...\n" +
                $"Luck: {luck:F2}\n" +
                $"Card Damage kills in this world: {kills}",
                new Color(255, 230, 120)
            );

            // Optional sound
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item29, player.Center);

            return true;
        }
    }

}
