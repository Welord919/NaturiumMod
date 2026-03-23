using NaturiumMod.Content.Items.Cards.Fusion;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons;

public abstract class NoEffectCommon : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftLeg";
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 999;
        Item.value = 25;
        Item.rare = ItemRarityID.White;
    }
}
public class SkullServant : NoEffectCommon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/SkullServant";
}
public class Armaill : NoEffectCommon
{
public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Armaill";
}

public class DarkworldThorns : NoEffectCommon
{
public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/DarkworldThorns";
}

public class Dissolverock : NoEffectCommon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Dissolverock";
}

public class Hinotama : NoEffectCommon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Hinotama";
}

public class LesserDragon : NoEffectCommon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LesserDragon";
}
public class MonsterEgg : NoEffectCommon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/MonsterEgg";
}
public class MWarrior1 : NoEffectCommon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/MWarrior1";
}

public class MWarrior2 : NoEffectCommon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/MWarrior2";
}

public class MysticalSheep2 : NoEffectCommon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/MysticalSheep2";
}

public class OneEyedSD : NoEffectCommon
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/OneEyedSD";
}

public class SteelOgre : NoEffectCommon
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/SteelOgre";
}
public class GiantSoldier : NoEffectCommon
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/GiantSoldier";
}
