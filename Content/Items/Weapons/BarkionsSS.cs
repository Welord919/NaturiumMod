using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModdingGang.Content.Items.Materials;

namespace ModdingGang.Content.Items.Weapons
{
    public class BarkionsSS : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 62; 
            Item.height = 32;
            Item.scale = 0.75f;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 20);

            // Use Properties
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true; 
            Item.UseSound = SoundID.Item11;

            // Weapon Properties
            Item.DamageType = DamageClass.Ranged; 
            Item.damage = 12; 
            Item.knockBack = 1f; 
            Item.noMelee = true; 

            // Gun Properties
            Item.shoot = ProjectileID.PurificationPowder;
            Item.shoot = Mod.Find<ModProjectile>("BarkionsBarkProj").Type;
            Item.shootSpeed = 9f;
            Item.useAmmo = Mod.Find<ModItem>("BarkionsBark").Type;
        }
           
        public override void AddRecipes()
        {
            CreateNewRecipe<BarkionsBark>(50, ItemID.LeadBar, 5, TileID.Anvils);
            CreateNewRecipe<BarkionsBark>(50, ItemID.IronBar, 5, TileID.Anvils);
        }

        private void CreateNewRecipe<T>(int modItemStack, short itemNeeded, int itemStack, ushort tileID) where T : ModItem
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<T>(), modItemStack);
            recipe.AddIngredient(itemNeeded, itemStack);
            recipe.AddTile(tileID);
            recipe.Register();
        }
    }
}
