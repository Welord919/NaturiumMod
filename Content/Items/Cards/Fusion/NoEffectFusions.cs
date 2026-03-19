using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion;

    public class MetalDragon : ModItem
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/MetalDragon";
    public override void SetDefaults()
    {
        Item.width = 32;
        Item.height = 32;
        Item.maxStack = 999;
        Item.rare = ItemRarityID.LightPurple;
        Item.value = Item.buyPrice(silver:10);
    }
    }
    public class Dragoness : MetalDragon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Dragoness";
}

    public class FlameGhost : MetalDragon
{
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/FlameGhost";
    }

    public class FlowerWolf : MetalDragon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/FlowerWolf";
}

    public class Fusionist : MetalDragon
        {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Fusionist";
}

    public class GaiaChampion : MetalDragon
        {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/GaiaChampion";
}
    public class Karbonala : MetalDragon
        {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Karbonala";
}
public class Charubin : MetalDragon
        {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/Charubin";
}
