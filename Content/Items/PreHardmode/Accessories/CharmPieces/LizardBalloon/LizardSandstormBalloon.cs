using Microsoft.Xna.Framework;
using NaturiumMod.Content.NPCs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces.LizardBalloon
{
    [AutoloadEquip(EquipType.Balloon)]
    public class LizardSandstormBalloon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/LizardSandstormBalloon";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 32;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 40);
            Item.defense = 4;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.jumpBoost = true;
            player.GetJumpState<LizardSandstormJump>().Enable();
            player.jumpSpeedBoost += 1.2f;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SandstorminaBottle, 1)
                .AddIngredient(ModContent.ItemType<BalloonLizardBalloon>(), 1)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    public class LizardSandstormJump : ExtraJump
    {
        // Place after Sandstorm in the jump order
        public override Position GetDefaultPosition() => new After(SandstormInABottle);

        public override float GetDurationMultiplier(Player player)
        {
            // Sandstorm is ~1.75f — this is stronger
            return 2f;
        }

        public override void UpdateHorizontalSpeeds(Player player)
        {
            // Stronger horizontal control than Sandstorm
            player.runAcceleration *= 2.0f;
            player.maxRunSpeed *= 2.2f;
        }

        public override void OnStarted(Player player, ref bool playSound)
        {
            playSound = true;

            // Yellowish-brown dust burst
            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(
                    player.position,
                    player.width,
                    player.height,
                    DustID.Sandnado, // perfect yellow-brown dust
                    Main.rand.NextFloat(-3f, 3f),
                    Main.rand.NextFloat(-3f, 3f),
                    0,
                    new Color(255, 210, 120),
                    1.4f
                );
                d.noGravity = true;
            }
        }

        public override void ShowVisuals(Player player)
        {
            // Continuous sand dust trail
            Dust d = Dust.NewDustDirect(
                player.position,
                player.width,
                10,
                DustID.Sandnado,
                player.velocity.X * 0.2f,
                player.velocity.Y * 0.2f,
                0,
                new Color(255, 200, 100),
                1.1f
            );
            d.noGravity = true;
        }
    }

}
