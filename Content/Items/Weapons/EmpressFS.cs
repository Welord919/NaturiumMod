using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace ModdingGang.Content.Items.Weapons
{
	public class EmpressFS : ModItem
	{
		public override void SetStaticDefaults()
		{
			CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
			Item.staff[Item.type] = true;
		}

        public override void SetDefaults()
        {
            Item.damage = 165;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 8;
            Item.width = 20;
            Item.height = 30;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.shootSpeed = 18f;
            Item.knockBack = 6f; 
            Item.crit = 10;
            Item.UseSound = SoundID.Item9;
            Item.value = Item.buyPrice(platinum: 5);
            Item.rare = ItemRarityID.Purple;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ProjectileID.FairyQueenMagicItemShot;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FairyQueenMagicItem);
            recipe.AddIngredient(ItemID.PiercingStarlight);
            recipe.AddIngredient(ItemID.RainbowRod);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}