using Microsoft.Xna.Framework;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using NaturiumMod.Content.NPCs;

namespace NaturiumMod.Content.Items.Critter
{
	public class ExampleCritterItem : ModItem
	{
		public override void SetStaticDefaults() {
			ItemID.Sets.IsLavaBait[Type] = true; // While this item is not bait, this will require a lava bug net to catch.
		}

		public override void SetDefaults() {
			Item.useStyle = 1;
			Item.autoReuse = true;
			Item.useTurn = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.maxStack = 99;
			Item.consumable = true;
			Item.width = 12;
			Item.height = 12;
			Item.makeNPC = 361;
			Item.noUseGraphic = true;
            
            //Cloning ItemID.Frog sets the preceding values
            Item.CloneDefaults(ItemID.Frog);
			Item.makeNPC = ModContent.NPCType<ExampleCritterNPC>();
			Item.value += Item.buyPrice(0, 0, 30, 0); // Make this critter worth slightly more than the frog
			Item.rare = ItemRarityID.Blue;
		}
	}
}