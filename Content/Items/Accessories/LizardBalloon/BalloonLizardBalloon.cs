using NaturiumMod.Content.NPCs.Enemies;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories.LizardBalloon
{
    [AutoloadEquip(EquipType.Balloon)]
    public class BalloonLizardBalloon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/BalloonLizardBalloon";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 30);
            Item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpBoost = true; // same effect as balloons
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ShinyRedBalloon, 1)
                .AddIngredient(ModContent.ItemType<LizardScale>(), 7)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    

}
