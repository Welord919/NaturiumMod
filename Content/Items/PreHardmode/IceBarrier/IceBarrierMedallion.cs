using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.PreHardmode.IceBarrier;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class IceBarrierMedallion : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/IceBarrierMedallion";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 1);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<IceDamagePlayer>().iceMedallionActive = true;
            player.GetModPlayer<FrostburnMinionPlayer>().frostburnMinions = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
        new(ModContent.ItemType<RedBeads>(), 1),
        new(ModContent.ItemType<IceBarrierIcon>(), 1),
        new(ModContent.ItemType<WaterEssence>(), 10),
        new(ModContent.ItemType<IceBarrierCore>(), 50)
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            int ice = ModContent.ItemType<IceBarrierIcon>();
            int beads = ModContent.ItemType<RedBeads>();

            return incomingItem.type != ice && incomingItem.type != beads;
        }

    }
}