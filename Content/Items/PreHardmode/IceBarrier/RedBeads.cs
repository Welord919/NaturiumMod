using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.IceBarrier
{
    public class RedBeads : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/RedBeads";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 30);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FrostburnMinionPlayer>().frostburnMinions = true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
        new(ModContent.ItemType<WaterEssence>(), 3),
        new(ItemID.Ruby, 13),
        new(ModContent.ItemType<IceBarrierCore>(), 25),
        new(ItemID.Rope, 50)
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
        
    }
    public class FrostburnMinionPlayer : ModPlayer
    {
        public bool frostburnMinions;

        public override void ResetEffects()
        {
            frostburnMinions = false;
        }
    }
    public class FrostburnMinionGlobalProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player owner = Main.player[projectile.owner];

            // Only minions & sentries
            if (!projectile.minion && !ProjectileID.Sets.SentryShot[projectile.type])
                return;

            // Only if accessory is equipped
            if (!owner.GetModPlayer<FrostburnMinionPlayer>().frostburnMinions)
                return;

            // Apply Frostburn for 3 seconds
            target.AddBuff(BuffID.Frostburn, 180);
        }
    }

}
