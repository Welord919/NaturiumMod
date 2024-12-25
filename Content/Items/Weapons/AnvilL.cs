using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons
{
    public class AnvilL : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 62;
            Item.height = 32;
            Item.scale = 0.75f;
            Item.rare = ItemRarityID.Green;

            // Use Properties
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            Item.UseSound = SoundID.Item11;

            // Weapon Properties
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 12;
            Item.knockBack = 4f;
            Item.noMelee = true;


            // Gun Properties
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shoot = ModContent.ProjectileType<Projectiles.AnvilLProj>();
            Item.shootSpeed = 90f;
            //Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 11f;
            //Item.useAmmo = AmmoID.Bullet;
            Item.value = Item.buyPrice(gold: 3);
        }
        
        public override void AddRecipes()
        {
            CreateNewRecipe(ItemID.IronAnvil, 10, TileID.Anvils);
            CreateNewRecipe(ItemID.LeadAnvil, 10, TileID.Anvils);
        }

        private void CreateNewRecipe(short itemNeeded, int itemStack, ushort tileID)
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(itemNeeded, itemStack);
            recipe.AddTile(tileID);
            recipe.Register();
        }
    }
}
