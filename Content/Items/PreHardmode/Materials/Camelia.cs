using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class Camelia : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/Camelia";

	public override void SetDefaults()
	{
		Item.Size = new(16, 16);
		Item.maxStack = 99;
		Item.value = Item.buyPrice(0, 0, 2, 0);
		Item.rare = ItemRarityID.White;
		Item.consumable = false;
	}
}