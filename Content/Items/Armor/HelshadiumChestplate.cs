using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Body)]
    public class HelshadiumChestplate : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Armor/HelshadiumChestplate";

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.Orange;
            Item.defense = 13;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetKnockback(DamageClass.Melee) += 0.8f;
            player.GetAttackSpeed(DamageClass.Melee) += 0.04f;
            player.buffImmune[BuffID.OnFire] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<Helshadium>(), 15);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}
