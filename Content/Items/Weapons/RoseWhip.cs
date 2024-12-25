using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModdingGang.Content.Items.Projectiles;
using ModdingGang.Content.Items.Materials;

namespace ModdingGang.Content.Items.Weapons
{
    public class RoseWhip : ModItem
    {
        public override void SetDefaults()
        {
            Item.DefaultToWhip(ModContent.ProjectileType<RoseWhipProj>(), 29, 6f, 4);
            Item.shootSpeed = 4;
            Item.rare = ItemRarityID.Green;
            Item.channel = true;

            Item.DamageType = DamageClass.Summon;
            Item.width = 40;
            Item.height = 40;
            
            Item.value = Item.buyPrice(gold: 1);
            Item.rare = ItemRarityID.Green;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<BarkionsBark>(), 36);
            recipe.AddIngredient(ItemID.Vine, 7);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        
        public override bool MeleePrefix()
        {  
            return true; 
        }
    }
}