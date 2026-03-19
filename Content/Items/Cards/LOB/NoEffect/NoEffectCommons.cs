using NaturiumMod.Content.Items.Cards.Fusion;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.NoEffect;

public abstract class Skullservant : ModItem
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
public class Armaill : Skullservant
{
public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Armaill";
}

public class DarkworldThorns : Skullservant
{
public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/DarkworldThorns";
}

public class Dissolverock : Skullservant
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Dissolverock";
}

public class Hinotama : Skullservant
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/Hinotama";
}

public class LesserDragon : Skullservant
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LesserDragon";
}
public class MonsterEgg : Skullservant
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/MonsterEgg";
}
public class MWarrior1 : Skullservant
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/MWarrior1";
}

public class MWarrior2 : Skullservant
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/MWarrior2";
}

public class MysticalSheep2 : Skullservant
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/MysticalSheep2";
}

public class OneEyedSD : Skullservant
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/OneEyedSD";
}

public class SteelOgre : Skullservant
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/SteelOgre";
}

public class SkullServant : Skullservant
    {
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/SkullServant";
}
public class GiantSoldier : Skullservant
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/GiantSoldier";
}
