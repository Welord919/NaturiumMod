using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Materials;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Tools
{
    public class MillenniumKey : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Millennium/MillenniumKey";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;

            Item.noMelee = true;
            Item.mana = 40;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 3);
            Item.shootSpeed = 0f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<MillenniumPiece>(), 25),
            new(ItemID.BattlePotion, 1),
            new(ItemID.SpelunkerPotion, 5)
            ], TileID.Anvils);
            recipe.Register();
        }
        // IMPORTANT: Enables right-click functionality
        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            // RIGHT CLICK = SHOW DROP CHANCES
            if (player.altFunctionUse == 2)
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    float boost = player.GetModPlayer<CardDropPlayer>().CardDropBoost;

                    float commonChance = MathHelper.Clamp(PackDropConstants.CommonBase * (1f + boost), 0f, 1f);
                    float rareChance = MathHelper.Clamp(PackDropConstants.RareBase * (1f + boost), 0f, 1f);
                    float superChance = MathHelper.Clamp(PackDropConstants.SuperBase * (1f + boost), 0f, 1f);
                    float ultraChance = MathHelper.Clamp(PackDropConstants.UltraBase * (1f + boost), 0f, 1f);

                    Main.NewText($"Common Pack Chance: {commonChance * 100f:0.00}%", Color.LightGreen);
                    Main.NewText($"Rare Pack Chance: {rareChance * 100f:0.00}%", Color.CornflowerBlue);
                    Main.NewText($"Super Pack Chance: {superChance * 100f:0.00}%", Color.Gold);
                    Main.NewText($"Ultra Pack Chance: {ultraChance * 100f:0.00}%", Color.MediumPurple);
                }

                return false; // don't consume mana or play animation
            }

            // LEFT CLICK = BUFF
            player.AddBuff(ModContent.BuffType<MillenniumKeyBuff>(), 600);
            player.AddBuff(BuffID.Slow, 180);
            Terraria.Audio.SoundEngine.PlaySound(SoundID.Item100, player.Center);
            return true;
        }

        public override bool CanRightClick() => true;

        
    }

    public class MillenniumKeyBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Millennium/MillenniumKeyBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.detectCreature = true;
            player.findTreasure = true;
            player.dangerSense = true;
        }
    }
}
