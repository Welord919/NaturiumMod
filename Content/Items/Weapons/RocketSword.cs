using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingGang.Content.Items.Weapons
{
	public class RocketSword : ModItem
	{
		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
            Item.damage = 30;
            Item.width = 30;
            Item.height = 50;
            Item.useTime = 22;
            Item.useAnimation = 22;
            Item.shootSpeed = 12f;
            Item.knockBack = 6f;
            Item.crit = 8;
            Item.UseSound = SoundID.Item1;

            Item.value = Item.buyPrice(gold: 3);
            Item.rare = ItemRarityID.Green;
            Item.autoReuse = true;

            Item.useStyle = ItemUseStyleID.Swing;

            Item.shoot = ProjectileID.RocketI;
        }

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.IronBar, 25);
            recipe.AddIngredient(ItemID.Wire, 10);
            recipe.AddIngredient(ItemID.ExplosivePowder, 15);
            recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
