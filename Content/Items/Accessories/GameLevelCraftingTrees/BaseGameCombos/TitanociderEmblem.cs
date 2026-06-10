using global::NaturiumMod.Content.Items.Materials;
using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace NaturiumMod.Content.Items.Accessories.GameLevelCraftingTrees.BaseGameCombos
{
    public class TitanociderEmblem : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/TitanociderEmblem";

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 15, 0, 0);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Recon Scope (ranged damage, crit, zoom, aggro reduction)
            player.GetDamage(DamageClass.Ranged) += 0.25f;
            player.GetCritChance(DamageClass.Ranged) += 15;
            player.scope = true;
            player.aggro -= 400;

            // Molten Quiver (arrow speed, damage, no-consume chance, flaming arrows)
            player.arrowDamage += 0.10f;
            player.hasMoltenQuiver = true;

            // Ammo Box effect (20% chance not to consume ammo)
            player.ammoBox = true;

            // Ammo Reservation Potion effect (20% more)
            player.ammoPotion = true;

            // Night-time precision bonus
            if (!Main.dayTime)
                player.GetCritChance(DamageClass.Ranged) += 5;

            // Slight mobility buff for kiting
            player.moveSpeed += 0.08f;
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.ReconScope)
                .AddIngredient(ItemID.MoltenQuiver)
                .AddIngredient(ItemID.AmmoBox)
                .AddIngredient(ModContent.ItemType<InfusedNaturiumBar>(), 30)
                .AddIngredient(ModContent.ItemType<WindEssence>(), 50)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }


}