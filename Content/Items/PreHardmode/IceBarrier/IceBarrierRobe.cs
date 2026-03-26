using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.IceBarrier
{
    [AutoloadEquip(EquipType.Body)]
    public class IceBarrierRobe : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/IceBarrierRobe";

        public override void SetDefaults()
        {
            Item.width = 22;
            Item.height = 18;
            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 7;
        }

        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Summon) += 0.04f;
            player.statManaMax2 += 20;
            player.maxMinions += 1;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<IceBarrierCore>(), 12);
            recipe.AddIngredient(ItemID.FlinxFur, 10);
            recipe.AddIngredient(ItemID.Sapphire, 3);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}