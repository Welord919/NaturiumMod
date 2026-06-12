using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Accessories.CraftingTrees.BaseGameCombos;
using NaturiumMod.Content.Items.Accessories.CraftingTrees.BaseGameCombos.NaturiumMod.Content.Items.Accessories.ShiinaCharms;
using NaturiumMod.Content.Items.Accessories.LizardBalloon;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Accessories.CraftingTrees.BaseGameCombos.HeroicTundraShield;
using static NaturiumMod.Content.Items.Accessories.MillenniumShield;

namespace NaturiumMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class WickedWindWings : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/Black-WingedWings";

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Red;
            Item.value = Item.buyPrice(0, 80, 0, 0);

            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150, 8f, 2f);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // ============================================
            // STEAMED CLOAK EFFECTS
            // ============================================
            player.longInvince = true;
            player.shimmerImmune = true;
            player.moveSpeed += 0.10f;
            player.wingRunAccelerationMult += 0.10f;
            player.GetModPlayer<SteamedCloakPlayer>().steamedCloakEquipped = true;

            // ============================================
            // LIZARD MEGA BALLOON EFFECTS
            // ============================================
            player.GetJumpState<LizardSandstormJump>().Enable();
            player.noFallDmg = true;
            player.jumpSpeedBoost += 1.2f;

            // ============================================
            // UMI CORE EFFECTS
            // ============================================
            player.autoJump = true;
            player.jumpSpeedBoost += 2f;
            player.moveSpeed += 0.08f;
            player.noFallDmg = true;
            player.spikedBoots = 2;

            player.accFlipper = true;
            player.ignoreWater = true;
            player.gills = true;
            player.iceSkate = true;
            player.arcticDivingGear = true;

            player.lavaImmune = true;
            player.waterWalk = true;
            player.waterWalk2 = true;
            player.accFishingBobber = true;
            player.accFishingLine = true;
            player.accLavaFishing = true;
            player.fishingSkill += 10;
            player.sonarPotion = true;

            player.blackBelt = true;

            if (player.wet)
                Lighting.AddLight(player.Center, 3f, 3f, 3f);

            if (!Main.dayTime)
                player.fishingSkill += 15;

            // ============================================
            // UNBOUND MILLENNIUM SHIELD EFFECTS
            // ============================================
            player.noKnockback = true;
            player.maxTurrets += 2;
            player.GetDamage(DamageClass.Summon) += 0.20f;
            var boost = player.GetModPlayer<WeaponBoostPlayer>();
            boost.activeBoosts["Apophis"] = true;
            player.pickSpeed -= 0.25f;
            player.iceBarrier = true;
            player.endurance += 0.15f;
            player.GetModPlayer<FrozenShieldPlayer>().frozenShieldActive = true;
            int[] debuffs =
            {
                BuffID.Poisoned, BuffID.Darkness, BuffID.Cursed, BuffID.Silenced,
                BuffID.BrokenArmor, BuffID.Bleeding, BuffID.Confused, BuffID.Slow,
                BuffID.Weak, BuffID.Chilled, BuffID.Frozen
            };
            foreach (int debuff in debuffs)
                player.buffImmune[debuff] = true;
            var dash = player.GetModPlayer<MillenniumDash>();
            dash.DashAccessoryEquipped = true;
            dash.DashCooldown = 30;
            dash.DashDuration = 60;
            dash.DashVelocity = 15f;
            dash.DashDamage = 150;
            dash.DashKnockback = 8f;
            dash.DashIFrames = 50;

            // ============================================
            // TERRASPARK BOOTS EFFECTS
            // ============================================
            player.fireWalk = true;
            player.lavaImmune = true;
            player.iceSkate = true;
            player.waterWalk = true;
            player.waterWalk2 = true;
            player.moveSpeed += 0.08f;
            player.desertBoots = true;
            player.accRunSpeed = 8f;
            player.vanityRocketBoots = 4;


            // ============================================
            // BUNDLE OF BALLOONS EFFECTS
            // ============================================
            player.jumpBoost = true;
            player.jumpSpeedBoost += 2.4f;
            player.GetJumpState<WickedWindJump>().Enable();

            //Extra
            player.maxRunSpeed += 0.3f;
        }
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            int wickedWind = ModContent.ItemType<WickedWindWings>();

            // Components used to craft Wicked Wind
            int lizardMegaBalloon = ModContent.ItemType<LizardMegaBalloon>();
            int umiCore = ModContent.ItemType<UmiCore>();
            int steamedCloak = ModContent.ItemType<SteamedCloak>();
            int unboundMillenniumShield = ModContent.ItemType<UnboundMillenniumShield>();
            int terraspark = ItemID.TerrasparkBoots;
            int bundleOfBalloons = ItemID.BundleofBalloons;

            bool IsWickedWindComponent(int type) =>
                type == lizardMegaBalloon ||
                type == umiCore ||
                type == steamedCloak ||
                type == unboundMillenniumShield ||
                type == terraspark ||
                type == bundleOfBalloons;

            // If equipping Wicked Wind, block if any component is already equipped
            if (incomingItem.type == wickedWind)
            {
                if (IsWickedWindComponent(equippedItem.type))
                    return false;
            }

            // If equipping a component, block if Wicked Wind is already equipped
            if (IsWickedWindComponent(incomingItem.type))
            {
                if (equippedItem.type == wickedWind)
                    return false;
            }

            // Prevent equipping two Wicked Winds
            if (incomingItem.type == wickedWind && equippedItem.type == wickedWind)
                return false;

            return base.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SteamedCloak>())
                .AddIngredient(ModContent.ItemType<LizardMegaBalloon>())
                .AddIngredient(ModContent.ItemType<UnboundMillenniumShield>())
                .AddIngredient(ModContent.ItemType<UmiCore>())
                .AddIngredient(ItemID.TerrasparkBoots)
                .AddIngredient(ItemID.BundleofBalloons)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    public class WickedWindJump : ExtraJump
    {
        public override Position GetDefaultPosition() => new After(BlizzardInABottle);

        public override float GetDurationMultiplier(Player player)
        {
            return player.GetModPlayer<WickedWindJumpPlayer>().jumpsRemaining switch
            {
                1 => 0.25f,
                2 => 0.45f,
                3 => 0.65f,
                4 => 0.85f,
                5 => 1.0f,
                _ => 0f
            };
        }

        public override void OnRefreshed(Player player)
        {
            player.GetModPlayer<WickedWindJumpPlayer>().jumpsRemaining = 5;
        }

        public override void OnStarted(Player player, ref bool playSound)
        {
            ref int jumps = ref player.GetModPlayer<WickedWindJumpPlayer>().jumpsRemaining;

            // Dust ring center
            int offsetY = player.height;
            if (player.gravDir == -1f)
                offsetY = 0;
            offsetY -= 16;

            Vector2 center = player.Top + new Vector2(0, offsetY);

            // Dust count scales with jump number
            int dustCount = jumps switch
            {
                5 => 40,
                4 => 32,
                3 => 26,
                2 => 22,
                _ => 18
            };

            for (int i = 0; i < dustCount; i++)
            {
                float angle = MathHelper.ToRadians(i * 360f / dustCount);
                float sin = MathF.Sin(angle);
                float cos = MathF.Cos(angle);

                float ampX = cos * (player.width + (jumps * 2)) / 2f;
                float ampY = sin * (4 + jumps);

                Dust d = Dust.NewDustPerfect(
                    center + new Vector2(ampX, ampY),
                    DustID.Smoke,                          // BLACK dust
                    -player.velocity * (0.15f + jumps * 0.05f),
                    0,
                    Color.Black,
                    1.1f
                );
                d.noGravity = true;
            }

            // Custom sound pitch
            playSound = false;
            float pitch = jumps switch
            {
                5 => -0.3f,
                4 => -0.1f,
                3 => 0.1f,
                2 => 0.3f,
                _ => 0.5f
            };

            SoundEngine.PlaySound(SoundID.Item20 with { Pitch = pitch, PitchVariance = 0.05f }, player.Bottom);

            // Decrement jump counter
            jumps--;

            // Allow more jumps if remaining
            if (jumps > 0)
                player.GetJumpState(this).Available = true;
        }
    }

        public class WickedWindJumpPlayer : ModPlayer
    {
        public int jumpsRemaining;

        public override void ResetEffects()
        {
            // Do nothing — jumps reset only when touching ground (OnRefreshed)
        }
    }
}
