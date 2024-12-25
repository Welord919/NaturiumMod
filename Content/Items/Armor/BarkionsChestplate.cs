using ModdingGang.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingGang.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	public class BarkionsChestplate : ModItem
	{
		public override void SetStaticDefaults() {}

		public override void SetDefaults() 
		{
            Item.width = 20;
            Item.height = 20;
            Item.value = 2500;
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetKnockback(DamageClass.Melee) += 0.1f;
            player.GetKnockback(DamageClass.Summon) += 0.1f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BarkionsBark>(), 125);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}