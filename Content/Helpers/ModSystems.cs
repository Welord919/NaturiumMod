using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
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
public class CardWorldDraw : GlobalItem
{
    public override bool AppliesToEntity(Item entity, bool lateInstantiation)
    {
        return entity.type == ModContent.ItemType<FlameSwordsman>()
            || entity.type == ModContent.ItemType<BEWD>()
            || entity.type == ModContent.ItemType<PackLOB>();
    }

    // ⭐ Stop Terraria from drawing the original full-size sprite
    public override bool PreDrawInWorld(
        Item item,
        SpriteBatch spriteBatch,
        Color lightColor,
        Color alphaColor,
        ref float rotation,
        ref float scale,
        int whoAmI
    )
    {
        return false; // block default draw
    }

    // ⭐ Draw our own scaled-down version
    public override void PostDrawInWorld(
        Item item,
        SpriteBatch spriteBatch,
        Color lightColor,
        Color alphaColor,
        float rotation,
        float scale,
        int whoAmI
    )
    {
        Texture2D texture = Terraria.GameContent.TextureAssets.Item[item.type].Value;

        Vector2 position = item.position - Main.screenPosition + new Vector2(item.width / 2, item.height / 2);
        Vector2 origin = texture.Size() / 2f;

        float shrink = 0.5f; // adjust size here

        spriteBatch.Draw(
            texture,
            position,
            null,
            lightColor,
            rotation,
            origin,
            shrink,
            SpriteEffects.None,
            0f
        );
    }
}
