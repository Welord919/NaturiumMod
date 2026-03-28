using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace NaturiumMod.Content.Items.Cards.PSA
{
    public class CardZoomUI : UIState
    {
        public static bool Visible;
        private bool previousMouseLeft;

        private Texture2D cardTexture;
        private UIPanel panel;

        // X button
        private Texture2D closeTexture;
        private Rectangle closeRect;

        // Header info
        public string HeaderCardName = "";
        public string HeaderPSA = "";
        public string HeaderMultiplier = "";
        public Color HeaderMultiplierColor = Color.White;

        public bool IsFoil = false;

        public override void OnInitialize()
        {
            panel = new UIPanel();
            panel.Width.Set(300f, 0f);
            panel.Height.Set(420f, 0f);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.5f;
            Append(panel);

            closeTexture = ModContent.Request<Texture2D>("Terraria/Images/UI/ButtonDelete").Value;
            closeRect = new Rectangle(0, 0, 22, 22);
        }

        public void SetTexture(Asset<Texture2D> texture, string cardName, string psa, string multiplier, Color multColor, bool isFoil)
        {
            cardTexture = texture.Value;
            HeaderCardName = cardName;
            HeaderPSA = psa;
            HeaderMultiplier = multiplier;
            HeaderMultiplierColor = multColor;
            IsFoil = isFoil;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!Visible)
                return;

            // Position the close rect before hit-testing
            Rectangle panelRect = panel.GetDimensions().ToRectangle();
            closeRect.X = panelRect.Right - closeRect.Width - 6;
            closeRect.Y = panelRect.Y + 6;

            // Edge detection: true only when the left button was pressed last frame and released this frame
            bool leftDownNow = Main.mouseLeft;
            bool leftReleasedThisFrame = previousMouseLeft && !leftDownNow;

            if (leftReleasedThisFrame && closeRect.Contains(Main.mouseX, Main.mouseY))
            {
                // Close the UI once on click release
                ModContent.GetInstance<CardZoomSystem>().HideCardZoom();
                // Optional: play a close sound
                // SoundEngine.PlaySound(SoundID.MenuClose);
            }

            previousMouseLeft = leftDownNow;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Visible || cardTexture == null)
                return;

            base.Draw(spriteBatch);

            Rectangle panelRect = panel.GetDimensions().ToRectangle();

            // X button
            closeRect.X = panelRect.Right - closeRect.Width - 6;
            closeRect.Y = panelRect.Y + 6;

            Color closeColor = closeRect.Contains(Main.mouseX, Main.mouseY)
                ? Color.White
                : Color.LightGray;

            spriteBatch.Draw(closeTexture, closeRect, closeColor);

            // Header
            int headerHeight = 40;
            Vector2 textPos = new Vector2(panelRect.X + 10, panelRect.Y + 8);

            Utils.DrawBorderString(spriteBatch, HeaderCardName, textPos, Color.White);

            Utils.DrawBorderString(spriteBatch,
                $"{HeaderPSA}   |   {HeaderMultiplier}",
                textPos + new Vector2(0, 18),
                HeaderMultiplierColor
            );

            // Card image
            Rectangle paddedRect = panelRect;
            int padding = 10;
            paddedRect.Inflate(-padding, -padding);
            paddedRect.Y += headerHeight;
            paddedRect.Height -= headerHeight;

            float scaleX = paddedRect.Width / (float)cardTexture.Width;
            float scaleY = paddedRect.Height / (float)cardTexture.Height;
            float scale = Math.Min(scaleX, scaleY);

            Vector2 position = new Vector2(
                paddedRect.X + paddedRect.Width / 2f,
                paddedRect.Y + paddedRect.Height / 2f
            );

            Vector2 origin = cardTexture.Size() / 2f;

            spriteBatch.Draw(
                cardTexture,
                position,
                null,
                Color.White,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );

            if (IsFoil)
                DrawFoil(spriteBatch, position, origin, scale);
        }

        private void DrawFoil(SpriteBatch spriteBatch, Vector2 position, Vector2 origin, float scale)
        {
            Texture2D foil = ModContent.Request<Texture2D>("NaturiumMod/Assets/Items/Cards/PSA/FoilOverlayZoom").Value;

            float time = (float)Main.timeForVisualEffects * 0.03f;

            Color foilColor = new Color(
                (byte)(100 + Math.Sin(time) * 25),
                (byte)(100 + Math.Sin(time + 2) * 25),
                (byte)(100 + Math.Sin(time + 4) * 25),
                50
            );

            spriteBatch.Draw(
                foil,
                position,
                null,
                foilColor,
                0f,
                foil.Size() / 2f,
                scale,
                SpriteEffects.None,
                0f
            );
        }
    }

    public class CardZoomSystem : ModSystem
    {
        internal CardZoomUI zoomUI;
        private UserInterface zoomInterface;

        public override void Load()
        {
            if (!Main.dedServ)
            {
                zoomUI = new CardZoomUI();
                zoomUI.Activate();
                zoomInterface = new UserInterface();
            }
        }

        public override void UpdateUI(GameTime gameTime)
        {
            if (CardZoomUI.Visible)
                zoomInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));

            if (mouseTextIndex != -1)
            {
                layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                    "NaturiumMod: Card Zoom",
                    delegate
                    {
                        if (CardZoomUI.Visible)
                            zoomInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }

        public void ShowCardZoom(Asset<Texture2D> texture, string name, string psa, string mult, Color multColor, bool isFoil)
        {
            zoomUI.SetTexture(texture, name, psa, mult, multColor, isFoil);
            CardZoomUI.Visible = true;
            zoomInterface.SetState(zoomUI);
        }

        public void HideCardZoom()
        {
            CardZoomUI.Visible = false;
            zoomInterface.SetState(null);
        }
    }

}