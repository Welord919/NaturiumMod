using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NaturiumMod.Content.UI;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace NaturiumMod.Content.Items.Tools.PlatformCreators;

internal class PlatformWheelState : UIState
{
    private UIElement _Root;
    private UIPanel _BackgroundPanel;
    private ClickablePanel _CenterPanel;
    private UIText _CenterText;
    private bool _ReplaceMode = false;
    private int _SelectedCount = 25;
    private readonly int[] _Counts = [ 25, 50, 100, 200, 400 ];
    private readonly List<ClickablePanel> _OptionPanels = [];

    private const float WheelSize = 260f;
    private const float OptionSize = 64f;
    private const float CenterSize = 80f;

    public override void OnInitialize()
    {
        _Root = new UIElement()
        {
            Left = StyleDimension.FromPixels(0f),
            Top = StyleDimension.FromPixels(0f),
            Width = StyleDimension.FromPixels(Main.screenWidth),
            Height = StyleDimension.FromPixels(Main.screenHeight),
        };

        _BackgroundPanel = new UIPanel()
        {
            Width = StyleDimension.FromPixels(WheelSize),
            Height = StyleDimension.FromPixels(WheelSize),
            HAlign = 0.5f,
            VAlign = 0.5f,
            BackgroundColor = new Color(30, 30, 30) * 0.0f,
            BorderColor = new Color(0, 0, 0, 0)
        };

        _Root.Append(_BackgroundPanel);

        _CenterPanel = new ClickablePanel()
        {
            Width = StyleDimension.FromPixels(CenterSize),
            Height = StyleDimension.FromPixels(CenterSize),
            BackgroundColor = new Color(40, 40, 40) * 0.9f,
            BorderColor = new Color(80, 80, 80)
        };
        _CenterPanel.SetPadding(0);
        _CenterPanel.OnClick += Center_OnClick;

        _CenterText = new UIText("Safe")
        {
            HAlign = 0.5f,
            VAlign = 0.5f
        };
        _CenterPanel.Append(_CenterText);

        _BackgroundPanel.Append(_CenterPanel);

        for (int i = 0; i < _Counts.Length; i++)
        {
            ClickablePanel p = new()
            {
                Width = StyleDimension.FromPixels(OptionSize),
                Height = StyleDimension.FromPixels(OptionSize),
                BackgroundColor = new Color(60, 60, 60) * 0.95f,
                BorderColor = new Color(110, 110, 110),
            };
            p.SetPadding(0);
            int capturedIndex = i;
            p.OnClick += (evt, elem) => Option_OnClick(evt, elem, _Counts[capturedIndex]);

            UIText t = new(_Counts[i].ToString())
            {
                HAlign = 0.5f,
                VAlign = 0.5f
            };
            p.Append(t);
            _OptionPanels.Add(p);
            _BackgroundPanel.Append(p);
        }

        Append(_Root);
    }

    public void SetInitialState(bool replace, int currentCount)
    {
        _ReplaceMode = replace;
        _SelectedCount = currentCount;
        _CenterText.SetText(_ReplaceMode ? "Replace" : "Safe");
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Position center panel in the middle of the wheel
        _CenterPanel.Left.Set((WheelSize - CenterSize) / 2f, 0f);
        _CenterPanel.Top.Set((WheelSize - CenterSize) / 2f, 0f);

        // Position option panels in a circle around the center
        float radius = (WheelSize - OptionSize) / 2f - 8f;
        int optionCount = _OptionPanels.Count;
        for (int i = 0; i < optionCount; i++)
        {
            float angle = MathHelper.ToRadians(-90f + i * (360f / optionCount));
            float cx = (WheelSize / 2f) + (float)Math.Cos(angle) * radius - (OptionSize / 2f);
            float cy = (WheelSize / 2f) + (float)Math.Sin(angle) * radius - (OptionSize / 2f);
            _OptionPanels[i].Left.Set(cx, 0f);
            _OptionPanels[i].Top.Set(cy, 0f);
        }

        if (Main.ingameOptionsWindow || Main.LocalPlayer.talkNPC >= 0)
        {
            PlatformWheelSystem.Instance?.ToggleOpen();
        }
    }

    private void Center_OnClick(UIMouseEvent evt, UIElement listeningElement)
    {
        _ReplaceMode = !_ReplaceMode;
        _CenterText.SetText(_ReplaceMode ? "Replace" : "Safe");
        SoundEngine.PlaySound(SoundID.MenuTick);
    }

    private void Option_OnClick(UIMouseEvent evt, UIElement listeningElement, int chosenCount)
    {
        PlatformCreatorFinal.SelectedCountStatic = chosenCount;
        PlatformCreatorFinal.ReplaceModeStatic = _ReplaceMode;
        SoundEngine.PlaySound(SoundID.MenuTick);
        PlatformWheelSystem.Instance?.ToggleOpen();
    }
}
