using ModdingGang.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingGang.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	public class BarkionsLeggings : ModItem
	{
		public override void SetStaticDefaults() {}

		public override void SetDefaults() 
		{
			Item.width = 20;
            Item.height = 20;
            Item.value = 2000;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 4;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetKnockback(DamageClass.Magic) += 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BarkionsBark>(), 75);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}