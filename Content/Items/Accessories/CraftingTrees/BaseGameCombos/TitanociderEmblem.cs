using global::NaturiumMod.Content.Items.Materials;
using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories.CraftingTrees.BaseGameCombos
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

            // Allow alt-use while held
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.noUseGraphic = false;
        }

        public override bool AltFunctionUse(Player player) => true;

        // Right-click while holding the emblem toggles scope mode and sends chat feedback
        public override bool CanUseItem(Player player)
        {
            if (player.altFunctionUse == 2)
            {
                var modPlayer = player.GetModPlayer<TitanociderPlayer>();

                // Toggle the scopeDisabled flag
                modPlayer.scopeDisabled = !modPlayer.scopeDisabled;

                // Immediate effect on the current player instance
                player.scope = !modPlayer.scopeDisabled;

                // Chat message
                if (modPlayer.scopeDisabled)
                {
                    Main.NewText("Scope OFF", Color.LightSkyBlue);
                }
                else
                {
                    Main.NewText("Scope ON", Color.LightGreen);
                }

                // Feedback sound
                SoundEngine.PlaySound(SoundID.MenuTick, player.Center);
            }

            return base.CanUseItem(player);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<TitanociderPlayer>().hasTitanocider = true;

            // Recon Scope (ranged damage, crit, zoom, aggro reduction)
            player.GetDamage(DamageClass.Ranged) += 0.25f;
            player.GetCritChance(DamageClass.Ranged) += 15;
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

            // Apply scope based on the player's toggle state
            var modPlayer = player.GetModPlayer<TitanociderPlayer>();
            if (modPlayer.scopeDisabled)
                player.scope = false;
            else
                player.scope = true;
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

    // Player class to hold the toggle and the equipped flag
    public class TitanociderPlayer : ModPlayer
    {
        public bool hasTitanocider;
        // Persisted toggle for scope suppression; not reset every tick so it acts like a mode toggle
        public bool scopeDisabled;

        public override void ResetEffects()
        {
            hasTitanocider = false;
            // NOTE: do NOT reset scopeDisabled here — it is a persistent toggle
        }

        // Ensure scope is suppressed while right-clicking with the emblem held (extra safety)
        public override void PostUpdate()
        {
            if (hasTitanocider && scopeDisabled)
            {
                Player.scope = false;
            }
        }
    }
}
