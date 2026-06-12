using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace NaturiumMod.Content.Items.Accessories
{
    public class VanitySwitch : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/VanitySwitch";

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(gold: 1);

            // Allow right-click alternate function
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.consumable = false;
        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            var v = player.GetModPlayer<VanitySwitchPlayer>();

            if (player.altFunctionUse == 2)
            {
                // RIGHT CLICK → cycle mode
                v.CurrentMode = (v.CurrentMode + 1) % VanitySwitchPlayer.ModeCount;
                Main.NewText($"Vanity Switch mode: {v.GetModeName()}", Color.Cyan);
            }
            else
            {
                // LEFT CLICK → toggle or cycle monolith
                if (v.CurrentMode == 0)
                {
                    // Toggle monolith mode
                    v.ModeEnabled[0] = !v.ModeEnabled[0];

                    if (v.ModeEnabled[0])
                    {
                        // If turning ON, cycle monolith type
                        v.MonolithIndex = (v.MonolithIndex + 1) % 8;
                        Main.NewText($"Monolith set to: {v.MonolithIndex}", Color.LightPink);
                    }
                    else
                    {
                        Main.NewText("Monolith OFF", Color.LightPink);
                    }
                }
                else
                {
                    // Toggle other modes normally
                    v.ModeEnabled[v.CurrentMode] = !v.ModeEnabled[v.CurrentMode];
                    Main.NewText($"{v.GetModeName()} toggled {(v.ModeEnabled[v.CurrentMode] ? "ON" : "OFF")}", Color.LightGreen);
                }
            }

            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<VanitySwitchPlayer>().HasSwitchEquipped = true;
        }
        /*
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DontStarveShaderItem)
                .AddIngredient(ItemID.RainbowCursor)
                .AddIngredient(ItemID.Sunglasses)
                .AddIngredient(ItemID.MiningHelmet)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
        */
    }
    public class VanitySwitchPlayer : ModPlayer
    {
        public const int ModeCount = 4;

        public int CurrentMode = 0;
        public bool[] ModeEnabled = new bool[ModeCount];
        public bool HasSwitchEquipped = false;

        // Monolith shader index
        // 0 = off, 1 = Solar, 2 = Vortex, 3 = Nebula, 4 = Stardust, 5 = Blood Moon, 6 = Void, 7 = Aether
        public int MonolithIndex = 0;

        public override void ResetEffects()
        {
            HasSwitchEquipped = false;
        }

        public override void PostUpdate()
        {
            if (!HasSwitchEquipped)
                return;

            // MODE 0 — Monolith Shader
            if (ModeEnabled[0])
            {
                Player.ManageSpecialBiomeVisuals("MonolithSolar", MonolithIndex == 1);
                Player.ManageSpecialBiomeVisuals("MonolithVortex", MonolithIndex == 2);
                Player.ManageSpecialBiomeVisuals("MonolithNebula", MonolithIndex == 3);
                Player.ManageSpecialBiomeVisuals("MonolithStardust", MonolithIndex == 4);
                Player.ManageSpecialBiomeVisuals("MonolithBloodMoon", MonolithIndex == 5);
                Player.ManageSpecialBiomeVisuals("MonolithVoid", MonolithIndex == 6);
                Player.ManageSpecialBiomeVisuals("MonolithAether", MonolithIndex == 7);
            }

            // MODE 1 — Rainbow Cursor
            if (ModeEnabled[1])
                Main.cursorColor = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);

            // MODE 2 — Sunglasses (sun glare reduction)
            Player.ManageSpecialBiomeVisuals("ForceField", ModeEnabled[2]);

            // MODE 3 — Mining Helmet Light
            if (ModeEnabled[3])
                Lighting.AddLight(Player.Center, 1f, 0.95f, 0.75f);
        }

        public string GetModeName()
        {
            return CurrentMode switch
            {
                0 => "Monolith Shader",
                1 => "Rainbow Cursor",
                2 => "Sunglasses",
                3 => "Mining Helmet Light",
                _ => "Unknown"
            };
        }
    }

}
