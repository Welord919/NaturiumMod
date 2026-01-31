using Terraria;

namespace NaturiumMod.Content.Helpers;

public static class RecipeHelper
{
    public static Recipe GetNewRecipe(Recipe recipe, (int itemId, int stack) ingredient)
    {
        recipe.AddIngredient(ingredient.itemId, ingredient.stack);
        
        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, (int itemId, int stack)[] ingredients)
    {
        foreach ((int itemId, int stack) in ingredients)
        {
            recipe.AddIngredient(itemId, stack);
        }
        
        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, (int itemId, int stack) ingredient, int tile)
    {
        recipe.AddIngredient(ingredient.itemId, ingredient.stack);
        recipe.AddTile(tile);
        
        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, (int itemId, int stack) ingredient, int[] tiles)
    {
        recipe.AddIngredient(ingredient.itemId, ingredient.stack);
        
        foreach (int tile in tiles)
        {
            recipe.AddTile(tile);
        }

        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, (int itemId, int stack)[] ingredients, int tile)
    {
        foreach ((int itemId, int stack) in ingredients)
        {
            recipe.AddIngredient(itemId, stack);
        }

        recipe.AddTile(tile);

        return recipe;
    }

    public static Recipe GetNewRecipe(Recipe recipe, (int itemId, int stack)[] ingredients, int[] tiles)
    {
        foreach ((int itemId, int stack) in ingredients)
        {
            recipe.AddIngredient(itemId, stack);
        }

        foreach (int tile in tiles)
        {
            recipe.AddTile(tile);
        }

        return recipe;
    }
}