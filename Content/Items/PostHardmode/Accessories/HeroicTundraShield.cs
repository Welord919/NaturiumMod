using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.PreHardmode.MillenniumItems;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PostHardmode.Accessories
{
    [AutoloadEquip(EquipType.Shield)]
    public class HeroicTundraShield : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Accessories/HeroicTundraShield";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 15);

            Item.defense = 10;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FrozenShield, 1);
            recipe.AddIngredient(ItemID.HeroShield, 1);
            recipe.AddIngredient(ItemID.HuntressBuckler, 1);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.Register();
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            for (int i = 0; i < player.armor.Length; i++)
            {
                Item item = player.armor[i];
                if (item != null && !item.IsAir)
                {
                    if (item.type == ItemID.HeroShield ||
                        item.type == ItemID.FrozenShield)
                    {
                        return false;
                    }
                }
            }

            return base.CanEquipAccessory(player, slot, modded);
        }
        public class HeroShieldBlock : GlobalItem
        {
            public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
            {
                if (item.type == ItemID.HeroShield)
                {
                    for (int i = 0; i < player.armor.Length; i++)
                    {
                        Item equipped = player.armor[i];
                        if (equipped != null && !equipped.IsAir)
                        {
                            if (equipped.type == ModContent.ItemType<HeroicTundraShield>())
                                return false;
                        }
                    }
                }

                return base.CanEquipAccessory(item, player, slot, modded);
            }
        }
        public class FrozenShieldBlock : GlobalItem
        {
            public override bool CanEquipAccessory(Item item, Player player, int slot, bool modded)
            {
                if (item.type == ItemID.FrozenShield)
                {
                    for (int i = 0; i < player.armor.Length; i++)
                    {
                        Item equipped = player.armor[i];
                        if (equipped != null && !equipped.IsAir)
                        {
                            if (equipped.type == ModContent.ItemType<HeroicTundraShield>())
                                return false;
                        }
                    }
                }

                return base.CanEquipAccessory(item, player, slot, modded);
            }
        }
            public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // FROZEN SHIELD EFFECTS
            player.iceBarrier = true; // vanilla Frozen Shield effect
            player.GetModPlayer<FrozenShieldPlayer>().frozenShieldActive = true;
            // HERO SHIELD EFFECTS
            player.aggro += 400;
            player.endurance += 0.15f; // 15% DR
            // Huntress' Buckler
            player.maxTurrets += 1; // +1 sentry
            player.GetDamage(DamageClass.Summon) += 0.10f; // +10% summon damage
        }
        public class FrozenShieldPlayer : ModPlayer
        {
            public bool frozenShieldActive;

            public override void ResetEffects()
            {
                frozenShieldActive = false;
            }

            // ============================================================
            // EFFECT 1: 25% DAMAGE REDUCTION BELOW 50% HP
            // ============================================================
            public override void ModifyHurt(ref Player.HurtModifiers modifiers)
            {
                if (frozenShieldActive && Player.statLife < Player.statLifeMax2 * 0.50f)
                {
                    modifiers.FinalDamage *= 0.75f; // 25% DR
                }
            }

            // ============================================================
            // EFFECT 2: DAMAGE REDIRECTION FROM TEAMMATES
            // ============================================================
            public override void OnHurt(Player.HurtInfo info)
            {
                if (!frozenShieldActive)
                    return;

                // Only redirect if wearer is above 25% HP
                if (Player.statLife <= Player.statLifeMax2 * 0.25f)
                    return;

                // Loop through all players
                for (int i = 0; i < Main.maxPlayers; i++)
                {
                    Player ally = Main.player[i];

                    if (!ally.active || ally.dead || ally.whoAmI == Player.whoAmI)
                        continue;

                    // Must be same team (and team not 0)
                    if (ally.team != Player.team || Player.team == 0)
                        continue;

                    // Must be within 50 tiles (Frozen Shield range)
                    if (Vector2.Distance(Player.Center, ally.Center) > 50 * 16)
                        continue;

                    // Redirect 25% of ally's damage to wearer
                    int redirected = (int)(info.Damage * 0.25f);

                    // Apply redirected damage to wearer
                    Player.Hurt(
                        PlayerDeathReason.ByCustomReason(
                            NetworkText.FromLiteral($"{Player.name} absorbed damage for an ally!")
                        ),
                        redirected,
                        0
                    );
                }
            }
        }
    }
}
