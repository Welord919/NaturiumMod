using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Accessories.CraftingTrees.BaseGameCombos;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Accessories.MillenniumShield;

namespace NaturiumMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class MillenniumShieldPlated : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Millennium/MillenniumShieldPlated";

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 8);

            Item.defense = 6;
            Item.damage = 40;
            Item.knockBack = 6f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<MillenniumShield>(), 1),
                new(ItemID.SquireShield, 1),
                new(ModContent.ItemType<TabletoftheKings>(), 1),
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item item = player.armor[i];
                if (item != null && !item.IsAir)
                {
                    if (item.type == ModContent.ItemType<UnboundMillenniumShield>() || item.type == ModContent.ItemType<MillenniumShieldPlated>() || item.type == ModContent.ItemType<MillenniumShield>())
                    {
                        return false;
                    }
                }
            }

            return base.CanEquipAccessory(player, slot, modded);
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Millennium Shield dash
            player.GetModPlayer<MillenniumDash>().DashAccessoryEquipped = true;

            // Knockback immunity (Obsidian Shield effect)
            player.noKnockback = true;

            // SQUIRE’S SHIELD EFFECTS
            player.maxTurrets += 1; // +1 sentry
            player.GetDamage(DamageClass.Summon) += 0.10f; // +10% summon damage

            // TABLET OF THE KINGS EFFECTS
            var boost = player.GetModPlayer<WeaponBoostPlayer>();
            boost.activeBoosts["Apophis"] = true;

            // Sand mining speed
            player.pickSpeed -= 0.25f;

            //extra
            player.fireWalk = true;

            var dash = player.GetModPlayer<MillenniumDash>();
            dash.DashAccessoryEquipped = true;
            dash.DashCooldown = 45;
            dash.DashDuration = 35;
            dash.DashVelocity = 11f;
            dash.DashDamage = 40;
            dash.DashKnockback = 7f;
            dash.DashIFrames = 40;
        }
    }
}
