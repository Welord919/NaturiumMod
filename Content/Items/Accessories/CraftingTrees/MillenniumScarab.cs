using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Materials;
using NaturiumMod.Content.Items.Tools;
using NaturiumMod.Content.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories.CraftingTrees
{
    // ============================================================
    // MILLENNIUM SCARAB ITEM
    // ============================================================
    public class MillenniumScarab : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/MillenniumScarab";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 12);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var mp = player.GetModPlayer<MillenniumScarabPlayer>();
            mp.ScarabEquipped = true;

            player.maxMinions += 3;
            player.maxTurrets += 1;
            player.GetDamage(DamageClass.Summon) += 0.25f;
            player.GetKnockback(DamageClass.Summon) += 0.12f;
            player.GetAttackSpeed(DamageClass.Summon) += 0.15f;
            player.dangerSense = true;
            player.findTreasure = true;
            player.moveSpeed += 0.05f;
            player.GetModPlayer<MillenniumScarabPlayer>().hasMillenniumScarab = true;
            mp.CardDropBoost = player.GetModPlayer<CardDropPlayer>().CardDropBoost;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<MillenniumNecklace>())
                .AddIngredient(ModContent.ItemType<MillenniumScale>())
                .AddIngredient(ModContent.ItemType<MillenniumKey>())
                .AddIngredient(ItemID.PapyrusScarab)
                .AddIngredient(ItemID.SummonerEmblem)
                .AddIngredient(ModContent.ItemType<InfusedNaturiumBar>(), 50)
                .AddIngredient(ModContent.ItemType<LightEssence>(), 30)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }

    // ============================================================
    // PLAYER DATA
    // ============================================================
    public class MillenniumScarabPlayer : ModPlayer
    {
        public bool ScarabEquipped;
        public float CardDropBoost;
        public bool hasMillenniumScarab;
        public override void ResetEffects()
        {
            ScarabEquipped = false;
            CardDropBoost = 0f;
            hasMillenniumScarab = false;
        }


    }
}
