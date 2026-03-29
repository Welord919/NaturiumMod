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
    public class IceBarrierMedallion : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/IceBarrierMedallion";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 80);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<IceDamagePlayer>().iceMedallionActive = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
        new(ModContent.ItemType<WaterEssence>(), 3),
        new(ModContent.ItemType<IceBarrierCore>(), 30),
        new(ModContent.ItemType<CharmBase>(), 1)
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
    public class IceDamagePlayer : ModPlayer
    {
        public bool iceMedallionActive;

        public override void ResetEffects()
        {
            iceMedallionActive = false;
        }
    }
    

}