using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace NaturiumMod.Content.Items.Materials
{
    public class ExteriosFang : ModItem 
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 15;
        }

        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 12;

            Item.damage = 12;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 1.25f;

            Item.maxStack = 9999;
            Item.consumable = true;

            Item.ammo = Item.type;
            Item.shoot = Mod.Find<ModProjectile>("ExteriosFangProj").Type;
            Item.value = 5000;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(20);
            recipe.AddIngredient(ModContent.ItemType<NaturiumBar>(), 10);
            recipe.AddIngredient(ItemID.SoulofLight, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 1);
            recipe.AddIngredient(ItemID.SoulofMight, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();
        }
    }
}