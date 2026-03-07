using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Helpers
{
    public class NaturiumRecipeSystem : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup evilChunks = new RecipeGroup(
                () => "Any Evil Chunk",
                new int[]
                {
                ItemID.RottenChunk,
                ModContent.ItemType<PlagueChunk>()
                }
            );

            RecipeGroup.RegisterGroup("NaturiumMod:EvilChunks", evilChunks);
        }
    public override void PostAddRecipes()
        {
            foreach (Recipe recipe in Main.recipe)
            {
                for (int i = 0; i < recipe.requiredItem.Count; i++)
                {
                    // If the recipe uses Rotten Chunks
                    if (recipe.requiredItem[i].type == ItemID.RottenChunk)
                    {
                        int stack = recipe.requiredItem[i].stack;

                        // Remove the vanilla ingredient
                        recipe.requiredItem.RemoveAt(i);

                        // Add your recipe group instead
                        recipe.AddRecipeGroup("NaturiumMod:EvilChunks", stack);
                    }
                }
            }
        }
    }
}
