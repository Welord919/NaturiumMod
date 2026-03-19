using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.NoEffect;
using NaturiumMod.Content.Items.Cards.LOB.Rares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion.FusionRecipies;

public struct FusionRecipe
{
    public int Result;
    public int[] Ingredients;
    public (int type, int amount)[] Essences;

    public FusionRecipe(int result, int[] ingredients, (int, int)[] essences)
    {
        Result = result;
        Ingredients = ingredients;
        Essences = essences;
    }
}
public class FusionRecipeLoader : ModSystem
{
    public override void OnModLoad()
    {
        FusionRegistry.RegisterFusions();
    }

    public override void AddRecipes()
    {
        foreach (var fusion in FusionRegistry.Recipes)
        {
            Recipe recipe = Recipe.Create(fusion.Result);

            foreach (int ingredient in fusion.Ingredients)
                recipe.AddIngredient(ingredient);

            foreach (var essence in fusion.Essences)
                recipe.AddIngredient(essence.type, essence.amount);

            recipe.AddTile(ModContent.TileType<FusionAltarTile>());
            recipe.Register();
        }
    }
}
public static class FusionRegistry
{
    public static readonly List<FusionRecipe> Recipes = new();

    public static void RegisterFusions()
    {
        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<FlameSwordsman>(),
            new int[]
            {
            ModContent.ItemType<FlameManipulator>(),
            ModContent.ItemType<Masaki>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<FireEssence>(), 2),
            (ModContent.ItemType<EarthEssence>(), 2)
            }
        ));

        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<DarkfireDragon>(),
            new int[]
            {
            ModContent.ItemType<Firegrass>(),
            ModContent.ItemType<PetiteDragon>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<FireEssence>(), 10),
            (ModContent.ItemType<LightEssence>(), 5),
            (ModContent.ItemType<DarkEssence>(), 5)
            }
        ));

        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<Charubin>(),
            new int[]
            {
            ModContent.ItemType<MonsterEgg>(),
            ModContent.ItemType<Hinotama>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<FireEssence>(), 2),
            (ModContent.ItemType<EarthEssence>(), 1)
            }
        ));

        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<Dragoness>(),
            new int[]
            {
            ModContent.ItemType<Armaill>(),
            ModContent.ItemType<OneEyedSD>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<DarkEssence>(), 2),
            (ModContent.ItemType<WindEssence>(), 2)
            }
        ));

        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<FlameGhost>(),
            new int[]
            {
            ModContent.ItemType<SkullServant>(),
            ModContent.ItemType<Dissolverock>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<FireEssence>(), 3),
            (ModContent.ItemType<DarkEssence>(), 1)
            }
        ));

        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<FlowerWolf>(),
            new int[]
            {
            ModContent.ItemType<SilverFang>(),
            ModContent.ItemType<DarkworldThorns>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<EarthEssence>(), 3),
            (ModContent.ItemType<WindEssence>(), 2)
            }
        ));

        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<Fusionist>(),
            new int[]
            {
            ModContent.ItemType<PetiteAngel>(),
            ModContent.ItemType<MysticalSheep2>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<LightEssence>(), 1),
            (ModContent.ItemType<DarkEssence>(), 1),
            (ModContent.ItemType<WindEssence>(), 1),
            (ModContent.ItemType<EarthEssence>(), 1),
            (ModContent.ItemType<FireEssence>(), 1),
            (ModContent.ItemType<WindEssence>(), 1)
            }
        ));

        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<GaiaChampion>(),
            new int[]
            {
            ModContent.ItemType<Gaia>(),
            ModContent.ItemType<CurseofDragon>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<LightEssence>(), 5),
            (ModContent.ItemType<DarkEssence>(), 5),
            (ModContent.ItemType<WindEssence>(), 3)
            }
        ));

        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<Karbonala>(),
            new int[]
            {
            ModContent.ItemType<MWarrior1>(),
            ModContent.ItemType<MWarrior2>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<EarthEssence>(), 4),
            (ModContent.ItemType<LightEssence>(), 1)
            }
        ));

        Recipes.Add(new FusionRecipe(
            ModContent.ItemType<MetalDragon>(),
            new int[]
            {
            ModContent.ItemType<SteelOgre>(),
            ModContent.ItemType<LesserDragon>(),
            ModContent.ItemType<Polymerization>()
            },
            new (int, int)[]
            {
            (ModContent.ItemType<EarthEssence>(), 5),
            (ModContent.ItemType<FireEssence>(), 2)
            }
        ));
    }
}
