using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion;

    public abstract class FusionBase : BaseCardFusion
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/FlameGhost";
    public override void SetDefaults()
    {
        base.SetDefaults();
        Item.value = Item.buyPrice(silver:45);
    }
    }
    public class Dragoness : FusionBase
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Dragoness";
}
    public class FlowerWolf : FusionBase
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/FlowerWolf";
}

    public class Fusionist : FusionBase //Used in Crafting
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Fusionist";
}
    public class Karbonala : FusionBase
        {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Karbonala";
}
public class Charubin : FusionBase //Used in Crafting
        {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Charubin";
}