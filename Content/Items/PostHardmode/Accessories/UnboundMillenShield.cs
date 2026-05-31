using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.MillenniumItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.PostHardmode.Accessories.HeroicTundraShield;
using static NaturiumMod.Content.Items.PreHardmode.MillenniumItems.MillenniumShield;

namespace NaturiumMod.Content.Items.PostHardmode.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class UnboundMillenniumShield : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Accessories/UnboundMillenniumShield";

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(gold: 25);

            Item.defense = 15;
            Item.damage = 60;
            Item.knockBack = 7f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MillenniumShieldPlated>(), 1);
            recipe.AddIngredient(ModContent.ItemType<HeroicTundraShield>(), 1);
            recipe.AddIngredient(ItemID.AnkhCharm, 1);
            recipe.AddIngredient(ItemID.ObsidianShield, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<MillenniumShieldPlated>(), 1);
            recipe2.AddIngredient(ModContent.ItemType<HeroicTundraShield>(), 1);
            recipe2.AddIngredient(ItemID.AnkhShield, 1);
            recipe2.AddTile(TileID.TinkerersWorkbench);
            recipe2.Register();
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
            // MILLENNIUM SHIELD EFFECTS
            player.GetModPlayer<MillenniumDash>().DashAccessoryEquipped = true;
            player.noKnockback = true;

            // SQUIRE’S SHIELD + Huntress' Buckler EFFECTS
            player.maxTurrets += 2;
            player.GetDamage(DamageClass.Summon) += 0.20f;

            // TABLET OF THE KINGS EFFECTS
            var boost = player.GetModPlayer<WeaponBoostPlayer>();
            boost.activeBoosts["Apophis"] = true;
            player.pickSpeed -= 0.25f;

            // HEROIC TUNDRA SHIELD EFFECTS
            player.iceBarrier = true; // Frozen Shield
            player.endurance += 0.15f; // Hero Shield DR
            player.aggro += 400;
            player.GetModPlayer<FrozenShieldPlayer>().frozenShieldActive = true;


            // AHK CHARM EFFECTS (full debuff immunity)
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Darkness] = true;
            player.buffImmune[BuffID.Cursed] = true;
            player.buffImmune[BuffID.Silenced] = true;
            player.buffImmune[BuffID.BrokenArmor] = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Confused] = true;
            player.buffImmune[BuffID.Slow] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.Chilled] = true;
            player.buffImmune[BuffID.Frozen] = true;

            // OBSIDIAN SHIELD EFFECTS
            player.fireWalk = true;

            var dash = player.GetModPlayer<MillenniumDash>();
            dash.DashAccessoryEquipped = true;
            dash.DashCooldown = 30;
            dash.DashDuration = 45;
            dash.DashVelocity = 13f;
            dash.DashDamage = 60;
            dash.DashKnockback = 8f;
            dash.DashIFrames = 50;
        }
    }
}
