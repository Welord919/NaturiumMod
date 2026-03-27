using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.NPCDrop;
using NaturiumMod.Content.Items.Cards.PSA;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace NaturiumMod.Content.Items.Cards.PSA
{
    public class PSACase : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/PSA/PSACase";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 26;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 1);
            Item.maxStack = 999;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<NaturiumBar>(), 2),
                new(ItemID.Glass, 8)
            ], TileID.WorkBenches);
            recipe.Register();
        }
    }
    public class PSAPolish : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/PSA/PSAPolish";

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(copper: 25);
            Item.maxStack = 999;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(5);
            recipe.AddIngredient(ItemID.Gel, 10);
            recipe.AddIngredient(ItemID.TatteredCloth, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.Register();

            Recipe recipe2 = CreateRecipe(7);
            recipe2.AddIngredient(ItemID.PinkGel, 1);
            recipe2.AddIngredient(ItemID.TatteredCloth, 1);
            recipe2.AddTile(TileID.WorkBenches);
            recipe2.Register();
        }
    }

    // ============================================================
    //  BASE GRADED CARD
    // ============================================================
    public abstract class BaseGradedCard : ModItem
    {
        public float grade;
        public abstract int OriginalCardType { get; }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 40;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 10);

            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 10;
            Item.useAnimation = 10;
        }
        public override bool ConsumeItem(Player player) => false;

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {

            Item cursor = Main.mouseItem;

            // Must be holding polish in cursor
            if (cursor.type != ModContent.ItemType<PSAPolish>())
            {
                Main.NewText("Hold Card Polish in your cursor to polish this card.", Color.Gray);
                return;
            }

            // Cannot polish PSA 10
            if (grade >= 10f)
            {
                SoundEngine.PlaySound(SoundID.MenuClose, player.position); // nope sound
                Main.NewText("This card is already GEM MINT. It cannot be polished further.", Color.Red);
                return;
            }


            float oldGrade = grade;

            // 52% upgrade, 48% downgrade
            bool upgrade = Main.rand.NextFloat() < 0.52f;

            // Determine max delta based on current grade
            float maxDelta = GetDeltaRange(oldGrade);

            // Random delta between 0.1 and maxDelta
            float delta = (float)Math.Round(Main.rand.NextFloat(0.1f, maxDelta), 1);

            // Weighted PSA roll influences direction
            float weighted = (float)typeof(BaseGradedCard)
                .GetMethod("GenerateWeightedPSA", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .Invoke(this, null);

            float bias = MathHelper.Clamp((weighted - oldGrade) * 0.3f, -1f, 1f);

            float newGrade = upgrade
                ? oldGrade + delta + bias
                : oldGrade - delta - bias;

            // Clamp between 0 and 10
            newGrade = MathHelper.Clamp(newGrade, 0f, 10f);

            // Ensure it ALWAYS changes
            if (Math.Abs(newGrade - oldGrade) < 0.001f)
            {
                newGrade += upgrade ? 0.1f : -0.1f;
                newGrade = MathHelper.Clamp(newGrade, 0f, 10f);
            }

            grade = (float)Math.Round(newGrade, 1);

            if (grade >= 10f)
            {
                SoundEngine.PlaySound(SoundID.Item4, player.position);
            }


            // Consume 1 polish
            cursor.stack--;
            if (cursor.stack <= 0)
                cursor.TurnToAir();

            // Color based on NEW grade
            Color msgColor = GetPSAColor(grade);

            Main.NewText(
                $"Card polished! PSA {oldGrade:0.0} → PSA {grade:0.0}",
                msgColor
            );
        }
        private Color GetPSAColor(float grade)
        {
            if (grade >= 10f)
                return Color.Red;
            if (grade >= 9f)
                return Color.Orange;
            if (grade >= 8.5f)
                return Color.Green;
            if (grade >= 7f)
                return Color.LightBlue;
            if (grade >= 1f)
                return Color.White;
            return Color.Gray;
        }

        private float GetDeltaRange(float grade)
        {
            if (grade < 4f)
                return 3f;
            if (grade < 7f)
                return 2.5f;
            if (grade < 8.5f)
                return 1.0f;
            if (grade < 9.2f)
                return 0.4f;
            if (grade < 9.6f)
                return 0.2f;
            return 0.1f;

        }

        public override bool AltFunctionUse(Player player) => true;

        public override bool? UseItem(Player player)
        {
            var zoomSystem = ModContent.GetInstance<CardZoomSystem>();

            // Right-click = PSA reveal
            if (player.altFunctionUse == 2)
            {
                string cardName = Lang.GetItemNameValue(OriginalCardType);
                Main.NewText($"{cardName} — PSA {grade:0.0}", Microsoft.Xna.Framework.Color.Cyan);
                return true;
            }

            // Left-click = toggle zoom panel
            if (CardZoomUI.Visible)
            {
                zoomSystem.HideCardZoom();
            }
            else
            {
                ShowZoomPanel();
            }

            return true;
        }

        // ============================================================
        //  ZOOM PANEL
        // ============================================================
        private void ShowZoomPanel()
        {
            ModItem original = ModContent.GetModItem(OriginalCardType);

            string className = original.GetType().Name;
            if (className.EndsWith("Card"))
                className = className.Substring(0, className.Length - 4);

            string path = $"NaturiumMod/Assets/Items/Cards/Zoom/{className}Zoom";

            var tex = ModContent.Request<Texture2D>(path, ReLogic.Content.AssetRequestMode.ImmediateLoad);

            string cardName = Lang.GetItemNameValue(OriginalCardType);
            string psa = $"PSA {grade:0.0}";

            string multiplier;

            if (grade == 0f)
                multiplier = "0.25× Value";
            else if (grade <= 1f)
                multiplier = "0.50× Value";
            else if (grade <= 2f)
                multiplier = "0.75× Value";
            else if (grade >= 10f)
                multiplier = "3× Value";
            else if (grade >= 9f)
                multiplier = "2× Value";
            else
                multiplier = "1× Value";

            Color multColor;

            if (grade == 0f)
                multColor = Color.Gray;          // 0.25x
            else if (grade <= 1f)
                multColor = Color.White;         // 0.50x
            else if (grade <= 2f)
                multColor = Color.LightBlue;     // 0.75x
            else if (grade >= 10f)
                multColor = Color.Red;           // 3x
            else if (grade >= 9f)
                multColor = Color.Orange;        // 2x
            else
                multColor = Color.Green;         // 1x


            ModContent.GetInstance<CardZoomSystem>()
                .ShowCardZoom(tex, cardName, psa, multiplier, multColor, grade >= 10f);
        }


        public override void HoldItem(Player player)
        {
            if (player.HeldItem.type != Item.type)
                ModContent.GetInstance<CardZoomSystem>().HideCardZoom();
        }

        // ============================================================
        //  WEIGHTED PSA GENERATION
        // ============================================================
        private float GenerateWeightedPSA()
        {
            while (true)
            {
                float roll = (float)Math.Round(Main.rand.NextFloat(1f, 10.01f), 1);

                float weight =
                    roll >= 10f ? 0.5f :
                    roll >= 9f ? 0.7f :
                    roll >= 8f ? 0.9f :
                    1f;

                if (Main.rand.NextFloat() < weight)
                    return roll;
            }
        }

        public override void OnCreated(ItemCreationContext context)
        {
            grade = GenerateWeightedPSA();
        }

        // ============================================================
        //  SAVE / LOAD
        // ============================================================
        public override void SaveData(TagCompound tag)
        {
            tag["grade"] = grade;
        }

        public override void LoadData(TagCompound tag)
        {
            grade = tag.GetFloat("grade");
        }

        // ============================================================
        //  VALUE SCALING
        // ============================================================
        public override void UpdateInventory(Player player)
        {
            int baseValue = Item.buyPrice(silver: 10);

            // New low-grade rules
            if (grade == 0f)
            {
                Item.value = (int)(baseValue * 0.25f); // 25%
            }
            else if (grade <= 1f)
            {
                Item.value = (int)(baseValue * 0.50f); // 50%
            }
            else if (grade <= 2f)
            {
                Item.value = (int)(baseValue * 0.75f); // 75%
            }
            // Existing high-grade rules
            else if (grade >= 10f)
            {
                Item.value = (int)(baseValue * grade * 3f);
            }
            else if (grade >= 9f)
            {
                Item.value = (int)(baseValue * grade * 2f);
            }
            else
            {
                Item.value = (int)(baseValue * grade);
            }
        }


        // ============================================================
        //  TOOLTIP
        // ============================================================
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            string cardName = Lang.GetItemNameValue(OriginalCardType);

            TooltipLine nameLine = tooltips.Find(t => t.Name == "ItemName" && t.Mod == "Terraria");

            if (nameLine != null)
            {
                if (grade == 0f)
                    nameLine.OverrideColor = Color.Gray;          // 0.25x
                else if (grade <= 1f)
                    nameLine.OverrideColor = Color.White;         // 0.50x
                else if (grade <= 2f)
                    nameLine.OverrideColor = Color.LightBlue;     // 0.75x
                else if (grade >= 10f)
                    nameLine.OverrideColor = Color.Red;           // 3x
                else if (grade >= 9f)
                    nameLine.OverrideColor = Color.Orange;        // 2x
                else
                    nameLine.OverrideColor = Color.Green;         // 1x
            }

            // Existing tooltip info
            tooltips.Add(new TooltipLine(Mod, "CardName", $"Card: {cardName}"));
            tooltips.Add(new TooltipLine(Mod, "PSAGrade", $"PSA Grade: {grade:0.0}"));

            // Value multiplier text
            if (grade == 0f)
                tooltips.Add(new TooltipLine(Mod, "ValueBonus", "Value Multiplier: 0.25x"));
            else if (grade <= 1f)
                tooltips.Add(new TooltipLine(Mod, "ValueBonus", "Value Multiplier: 0.5x"));
            else if (grade <= 2f)
                tooltips.Add(new TooltipLine(Mod, "ValueBonus", "Value Multiplier: 0.75x"));
            else if (grade >= 10f)
                tooltips.Add(new TooltipLine(Mod, "ValueBonus", "Value Multiplier: 3x (GEM MINT)"));
            else if (grade >= 9f)
                tooltips.Add(new TooltipLine(Mod, "ValueBonus", "Value Multiplier: 2x"));
            else
                tooltips.Add(new TooltipLine(Mod, "ValueBonus", "Value Multiplier: 1x"));
        }
        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            base.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);

            if (grade >= 10f)
                DrawFoil(spriteBatch, position, frame, origin, scale);
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
            base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);

            if (grade >= 10f)
            {
                Texture2D texture = TextureAssets.Item[Item.type].Value;
                Vector2 position = Item.Center - Main.screenPosition;
                Rectangle frame = texture.Frame();
                Vector2 origin = frame.Size() / 2f;

                DrawFoil(spriteBatch, position, frame, origin, scale);
            }
        }
        private void DrawFoil(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Vector2 origin, float scale)
        {
            Texture2D foil = ModContent.Request<Texture2D>("NaturiumMod/Assets/Items/Cards/PSA/FoilOverlay").Value;

            float time = (float)Main.timeForVisualEffects * 0.03f;

            Color foilColor = new Color(
                (byte)(100 + Math.Sin(time) * 25),
                (byte)(100 + Math.Sin(time + 2) * 25),
                (byte)(100 + Math.Sin(time + 4) * 25),
                50
            );

            spriteBatch.Draw(
                foil,
                position,
                frame,
                foilColor,
                0f,
                origin,
                scale,
                SpriteEffects.None,
                0f
            );
        }

    }
}