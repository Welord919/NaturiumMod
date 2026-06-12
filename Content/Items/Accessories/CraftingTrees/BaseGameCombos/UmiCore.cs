namespace NaturiumMod.Content.Items.Accessories.CraftingTrees.BaseGameCombos
{
    using global::NaturiumMod.Content.Items.Cards.Fusion;
    using global::NaturiumMod.Content.Items.Materials;
    using Terraria;
    using Terraria.ID;
    using Terraria.ModLoader;

    namespace NaturiumMod.Content.Items.Accessories.ShiinaCharms
    {
        public class UmiCore : ModItem
        {
            public override string Texture => "NaturiumMod/Assets/Items/Accessories/UmiCore";

            public override void SetDefaults()
            {
                Item.width = 34;
                Item.height = 34;
                Item.accessory = true;
                Item.rare = ItemRarityID.Lime;
                Item.value = Item.buyPrice(0, 12, 0, 0);
            }

            public override void UpdateAccessory(Player player, bool hideVisual)
            {
                // Frog Gear
                player.autoJump = true;
                player.jumpSpeedBoost += 2f;
                player.moveSpeed += 0.08f;
                player.noFallDmg = true;
                player.spikedBoots = 2;


                // Arctic Diving Gear
                player.accFlipper = true;
                player.ignoreWater = true;
                player.gills = true;
                player.iceSkate = true;
                player.arcticDivingGear = true;

                // Lavaproof Tackle Bag
                player.lavaImmune = true;
                player.waterWalk = true;
                player.waterWalk2 = true;
                player.accFishingBobber = true;
                player.accFishingLine = true;
                player.accLavaFishing = true;
                player.fishingSkill += 10;
                player.sonarPotion = true;

                // Master Ninja Gear (only the parts NOT already in Frog Gear)
                player.blackBelt = true;   // dodge chance
                player.dashType = 1;       // Tabi dash

                // UMI-themed bonuses
                if (player.wet)
                {
                    Lighting.AddLight(player.Center,2f,2f,2f);
                }

                if (!Main.dayTime)
                    player.fishingSkill += 15;
            }
            public override void AddRecipes()
            {
                CreateRecipe()
                    .AddIngredient(ItemID.FrogGear)
                    .AddIngredient(ItemID.ArcticDivingGear)
                    .AddIngredient(ItemID.LavaproofTackleBag)

                    // Master Ninja Gear components NOT already included in Frog Gear
                    .AddIngredient(ItemID.Tabi)
                    .AddIngredient(ItemID.BlackBelt)

                    // Your modded materials
                    .AddIngredient(ModContent.ItemType<InfusedNaturiumBar>(), 30)
                    .AddIngredient(ModContent.ItemType<WaterEssence>(), 50)

                    .AddTile(TileID.TinkerersWorkbench)
                    .Register();
            }
        }
    }
}
