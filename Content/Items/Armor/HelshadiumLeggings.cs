using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Legs)]
    public class HelshadiumLeggings : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Armor/HelshadiumLeggings";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 16;
            Item.value = Item.buyPrice(silver: 40);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 11;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetAttackSpeed(DamageClass.Melee) += 0.08f;
            player.GetDamage(DamageClass.Melee) += 0.04f;
            player.moveSpeed += 0.5f;
            player.fireWalk = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Helshadium>(), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
