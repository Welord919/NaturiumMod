using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.IceBarrier
{
    [AutoloadEquip(EquipType.Legs)]
    public class IceBarrierLeggings : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/IceBarrierLeggings";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 16;
            Item.value = Item.buyPrice(silver: 40);
            Item.rare = ItemRarityID.Blue;
            Item.defense = 6;
        }

        public override void UpdateEquip(Player player)
        {
            player.moveSpeed += 0.5f;
            player.iceSkate = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<IceBarrierCore>(), 10);
            recipe.AddIngredient(ItemID.IceBlock, 30);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }
}