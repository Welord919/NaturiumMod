using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Accessories.LizardBalloon
{
    [AutoloadEquip(EquipType.Balloon)]
    public class LizardMegaBalloon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/LizardMegaBalloon";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 1);
            Item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpBoost = true;
            player.GetJumpState<LizardSandstormJump>().Enable();
            player.noFallDmg = true;
            player.jumpSpeedBoost += 1.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.LuckyHorseshoe, 1)
                .AddIngredient(ModContent.ItemType<LizardSandstormBalloon>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }

    }
}