using System;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace NaturiumMod.Content.Helpers;

public class ClickablePanel : UIPanel
{
    public event Action<UIMouseEvent, UIElement> OnClick;   

    public override void LeftMouseDown(UIMouseEvent evt)
    {
        base.LeftMouseDown(evt);
        OnClick?.Invoke(evt, this);
    }
}