using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModdingGang.Content.Items.Materials;

namespace ModdingGang.Content.Items.Weapons
{
    public class BarkionsBlaster: ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 36;
            Item.useTime = 36;
            Item.crit += 25;
            Item.width = 44;
            Item.height = 14;

            Item.UseSound = SoundID.Item40;
            Item.damage = 35;

            Item.noMelee = true;
            Item.value = Item.buyPrice(0, 20);
            Item.knockBack = 8f;
            Item.rare = ItemRarityID.Yellow;
            Item.DamageType = DamageClass.Ranged;

            //Gun Properties
            Item.shootSpeed = 15f;
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = Mod.Find<ModItem>("NaturiumClump").Type;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            CreateNewRecipe<NaturiumBar>(25, ItemID.LeadBar, 15, TileID.Anvils);
            CreateNewRecipe<NaturiumBar>(25, ItemID.IronBar, 15, TileID.Anvils);
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