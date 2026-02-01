using Terraria;

namespace NaturiumMod.Content.Helpers;

public static class RecipeHelper
{
    public static Recipe GetNewRecipe(Recipe recipe, Ingredient ingredient)
    {
        recipe.AddIngredient(ingredient.ItemID, ingredient.Count);
        
        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, Ingredient[] ingredients)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            recipe.AddIngredient(ingredient.ItemID, ingredient.Count);
        }
        
        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, Ingredient ingredient, int tile)
    {
        recipe.AddIngredient(ingredient.ItemID, ingredient.Count);
        recipe.AddTile(tile);
        
        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, Ingredient ingredient, int[] tiles)
    {
        recipe.AddIngredient(ingredient.ItemID, ingredient.Count);
        
        foreach (int tile in tiles)
        {
            recipe.AddTile(tile);
        }

        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, Ingredient[] ingredients, int tile)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            recipe.AddIngredient(ingredient.ItemID, ingredient.Count);
        }

        recipe.AddTile(tile);

        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, Ingredient[] ingredients, int[] tiles)
    {
        foreach (Ingredient ingredient in ingredients)
        {
            recipe.AddIngredient(ingredient.ItemID, ingredient.Count);
        }

        foreach (int tile in tiles)
        {
            recipe.AddTile(tile);
        }

        return recipe;
    }
}