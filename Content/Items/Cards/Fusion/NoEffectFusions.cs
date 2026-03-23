using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion;

    public abstract class FusionBase : ModItem
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/FlameGhost";
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.maxStack = 999;
        Item.rare = ItemRarityID.LightPurple;
        Item.value = Item.buyPrice(silver:20);
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

    public class Fusionist : FusionBase
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Fusionist";
}

    public class GaiaChampion : FusionBase
        {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/GaiaChampion";
}
    public class Karbonala : FusionBase
        {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Karbonala";
}
public class Charubin : FusionBase
        {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Charubin";
}