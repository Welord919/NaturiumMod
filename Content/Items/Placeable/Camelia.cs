using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Placeable
{
	public class Camelia : ModItem
	{
	
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 99;
			Item.value = Item.buyPrice(silver: 2);
			Item.rare = ItemRarityID.White;
			Item.consumable = false;
		}
	}
}