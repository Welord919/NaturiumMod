using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace NaturiumMod.Content.Items.Tools.PlatformCreators
{
    public class PlatformCreatorFinal : ModItem
    {
        internal static bool ReplaceModeStatic = false;
        internal static int SelectedCountStatic = 25;

        public override void SetStaticDefaults() { }

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useTime = 14;
            Item.useAnimation = 14;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.buyPrice(silver: 180);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;

            Item.noMelee = true;
            Item.createTile = -1;
            Item.autoReuse = true;
            Item.useTurn = true;
            Item.tileBoost = 2;
            Item.attackSpeedOnlyAffectsWeaponAnimation = true;
        }

        public override bool AltFunctionUse(Player player) => true;

        // Block item use at the earliest stage if the wheel is open or if the system requests left-click blocking.
        public override bool CanUseItem(Player player)
        {
            var sys = PlatformWheelSystem.Instance;
            if (sys is not null)
            {
                // Never allow normal (left-click) use while the wheel is open or while the system is blocking left clicks.
                if ((sys.IsOpen || sys.BlockLeftClick) && player.altFunctionUse != 2)
                    return false;
            }

            return base.CanUseItem(player);
        }

        // Prevent left clicks while the wheel requests blocking; right click toggles wheel.
        public override bool? UseItem(Player player)
        {
            if (player == null) return base.UseItem(player);

            if (PlatformWheelSystem.Instance?.BlockLeftClick == true)
                return false;

            if (PlatformWheelSystem.Instance?.IsOpen == true && player.altFunctionUse != 2)
                return false;

            if (player.altFunctionUse == 2)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    PlatformWheelSystem.Instance?.ToggleOpen();
                    SoundEngine.PlaySound(SoundID.MenuOpen);
                }
                return true;
            }

            if (Main.myPlayer == player.whoAmI)
            {
                PlatformWheelSystem.Instance?.PlacePlatforms(SelectedCountStatic, ReplaceModeStatic);
            }

            return true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string modeText = ReplaceModeStatic
                ? "Mode: Replace (overwrites blocks)"
                : "Mode: Safe (avoids overwriting)";

            TooltipLine modeLine = new(Mod, "PlatformCreatorMode", modeText)
            {
                OverrideColor = ReplaceModeStatic ? new Color(255, 150, 50) : new Color(50, 200, 150)
            };
            tooltips.Add(modeLine);

            TooltipLine countLine = new(Mod, "PlatformCreatorCount", $"Selected: {SelectedCountStatic}")
            {
                OverrideColor = new Color(200, 200, 200)
            };
            tooltips.Add(countLine);

            TooltipLine hint = new(Mod, "PlatformCreatorHint", "Right-click to open wheel. Left-click to place.")
            {
                OverrideColor = new Color(180, 180, 180)
            };
            tooltips.Add(hint);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            // Require one of each earlier platform creator (assumes classes exist in same namespace)
            recipe.AddIngredient(ModContent.ItemType<PlatformCreator>(), 1);
            recipe.AddIngredient(ModContent.ItemType<PlatformCreator2>(), 1);
            recipe.AddIngredient(ModContent.ItemType<PlatformCreator3>(), 1);
            recipe.AddIngredient(ModContent.ItemType<PlatformCreator4>(), 1);

            // Require 5 of each soul
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(ItemID.SoulofNight, 5);

            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }

    public class PlatformWheelSystem : ModSystem
    {
        internal static PlatformWheelSystem Instance;
        private UserInterface userInterface;
        private PlatformWheelState wheelState;
        private bool visible;

        // When true left-click uses are ignored (used while menu is open and until mouse is released).
        internal bool BlockLeftClick;
        public bool IsOpen => visible;

        public override void OnModLoad()
        {
            Instance = this;
            userInterface = new UserInterface();
            wheelState = new PlatformWheelState();
            wheelState.Activate();
            userInterface.SetState(null);
            visible = false;
            BlockLeftClick = false;
        }

        public override void Unload()
        {
            Instance = null;
            userInterface = null;
            wheelState = null;
        }

        // Center the wheel using UI alignment (HAlign/VAlign).
        public void ToggleOpen()
        {
            visible = !visible;

            if (visible)
            {
                wheelState.SetInitialState(PlatformCreatorFinal.ReplaceModeStatic, PlatformCreatorFinal.SelectedCountStatic);
                userInterface.SetState(wheelState);
                // Block left clicks until mouse release to avoid immediate placement.
                BlockLeftClick = true;
            }
            else
            {
                userInterface.SetState(null);
                // Keep blocking until mouse is released so the click that closed the UI doesn't place.
                BlockLeftClick = true;
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (BlockLeftClick && !Main.mouseLeft)
            {
                BlockLeftClick = false;
            }

            if (visible && userInterface?.CurrentState != null)
            {
                userInterface.Update(gameTime);
            }
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "NaturiumMod: Platform Wheel",
                    delegate
                    {
                        if (visible && userInterface?.CurrentState != null)
                        {
                            userInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI));
            }
        }

        // Called to place platforms (left-click action).
        public void PlacePlatforms(int count, bool replace)
        {
            // Extra safety: never place while the menu is open.
            if (visible)
                return;

            PlatformCreatorFinal.ReplaceModeStatic = replace;
            PlatformCreatorFinal.SelectedCountStatic = count;

            Player player = Main.LocalPlayer;
            Vector2 mouseWorld = Main.MouseWorld;
            int startX = (int)(mouseWorld.X / 16f);
            int startY = (int)(mouseWorld.Y / 16f);

            int dir;
            if (mouseWorld.X < player.Center.X) dir = -1;
            else if (mouseWorld.X > player.Center.X) dir = 1;
            else
            {
                dir = player.direction;
                if (dir == 0) dir = 1;
            }

            int platformTileType = TileID.Platforms;
            bool placedAny = false;

            for (int i = 0; i < count; i++)
            {
                int x = startX + i * dir;
                int y = startY;

                if (x < 10 || x > Main.maxTilesX - 10 || y < 10 || y > Main.maxTilesY - 10)
                    continue;

                if (replace)
                {
                    if (Main.tile[x, y].HasTile)
                        Terraria.WorldGen.KillTile(x, y, fail: false, effectOnly: false, noItem: true);

                    if (Terraria.WorldGen.PlaceTile(x, y, platformTileType, mute: true, forced: true, plr: -1, style: 0))
                        placedAny = true;
                }
                else
                {
                    if (Terraria.WorldGen.PlaceTile(x, y, platformTileType, mute: true, forced: false, plr: -1, style: 0))
                        placedAny = true;
                }
            }

            if (placedAny && Main.netMode == NetmodeID.MultiplayerClient)
            {
                int radius = Math.Max(1, count / 2);
                int centerX = startX + dir * radius;
                NetMessage.SendTileSquare(-1, centerX, startY, radius);
            }

            SoundEngine.PlaySound(SoundID.Dig, player.position);

            if (visible)
                ToggleOpen();
        }
    }

    internal class PlatformWheelState : UIState
    {
        private UIElement root;
        private UIPanel backgroundPanel;
        private ClickablePanel centerPanel;
        private UIText centerText;
        private bool replaceMode = false;
        private int selectedCount = 25;
        private readonly int[] counts = new[] { 25, 50, 100, 200, 400 };
        private readonly List<ClickablePanel> optionPanels = new();

        private const float WheelSize = 260f;
        private const float OptionSize = 64f;
        private const float CenterSize = 80f;

        public override void OnInitialize()
        {
            root = new UIElement()
            {
                Left = StyleDimension.FromPixels(0f),
                Top = StyleDimension.FromPixels(0f),
                Width = StyleDimension.FromPixels(Main.screenWidth),
                Height = StyleDimension.FromPixels(Main.screenHeight),
            };

            backgroundPanel = new UIPanel()
            {
                Width = StyleDimension.FromPixels(WheelSize),
                Height = StyleDimension.FromPixels(WheelSize),
                HAlign = 0.5f,
                VAlign = 0.5f,
                BackgroundColor = new Color(30, 30, 30) * 0.0f,
                BorderColor = new Color(0, 0, 0, 0)
            };

            root.Append(backgroundPanel);

            centerPanel = new ClickablePanel()
            {
                Width = StyleDimension.FromPixels(CenterSize),
                Height = StyleDimension.FromPixels(CenterSize),
                BackgroundColor = new Color(40, 40, 40) * 0.9f,
                BorderColor = new Color(80, 80, 80)
            };
            centerPanel.SetPadding(0);
            centerPanel.OnClick += Center_OnClick;

            centerText = new UIText("Safe")
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            centerPanel.Append(centerText);

            backgroundPanel.Append(centerPanel);

            for (int i = 0; i < counts.Length; i++)
            {
                ClickablePanel p = new ClickablePanel()
                {
                    Width = StyleDimension.FromPixels(OptionSize),
                    Height = StyleDimension.FromPixels(OptionSize),
                    BackgroundColor = new Color(60, 60, 60) * 0.95f,
                    BorderColor = new Color(110, 110, 110)
                };
                p.SetPadding(0);
                int capturedIndex = i;
                p.OnClick += (evt, elem) => Option_OnClick(evt, elem, counts[capturedIndex]);

                UIText t = new UIText(counts[i].ToString())
                {
                    HAlign = 0.5f,
                    VAlign = 0.5f
                };
                p.Append(t);
                optionPanels.Add(p);
                backgroundPanel.Append(p);
            }

            Append(root);
        }

        public void SetInitialState(bool replace, int currentCount)
        {
            replaceMode = replace;
            selectedCount = currentCount;
            centerText.SetText(replaceMode ? "Replace" : "Safe");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Position center panel in the middle of the wheel
            centerPanel.Left.Set((WheelSize - CenterSize) / 2f, 0f);
            centerPanel.Top.Set((WheelSize - CenterSize) / 2f, 0f);

            // Position option panels in a circle around the center
            float radius = (WheelSize - OptionSize) / 2f - 8f;
            int optionCount = optionPanels.Count;
            for (int i = 0; i < optionCount; i++)
            {
                float angle = MathHelper.ToRadians(-90f + i * (360f / optionCount));
                float cx = (WheelSize / 2f) + (float)Math.Cos(angle) * radius - (OptionSize / 2f);
                float cy = (WheelSize / 2f) + (float)Math.Sin(angle) * radius - (OptionSize / 2f);
                optionPanels[i].Left.Set(cx, 0f);
                optionPanels[i].Top.Set(cy, 0f);
            }

            if (Main.ingameOptionsWindow || Main.player[Main.myPlayer].talkNPC >= 0)
                PlatformWheelSystem.Instance?.ToggleOpen();
        }

        private void Center_OnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            replaceMode = !replaceMode;
            centerText.SetText(replaceMode ? "Replace" : "Safe");
            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        private void Option_OnClick(UIMouseEvent evt, UIElement listeningElement, int chosenCount)
        {
            PlatformCreatorFinal.SelectedCountStatic = chosenCount;
            PlatformCreatorFinal.ReplaceModeStatic = replaceMode;
            SoundEngine.PlaySound(SoundID.MenuTick);
            PlatformWheelSystem.Instance?.ToggleOpen();
        }

        private class ClickablePanel : UIPanel
        {
            public event Action<UIMouseEvent, UIElement> OnClick;

            public override void LeftMouseDown(UIMouseEvent evt)
            {
                base.LeftMouseDown(evt);
                OnClick?.Invoke(evt, this);
            }
        }
    }
}
