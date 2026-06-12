using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories
{
    public class VenomRing : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/VenomRing";

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 6);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<VenomRingPlayer>();
            modPlayer.venomRingActive = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<CharmBase>(), 1),
                new(ModContent.ItemType<PoisonBulb>(), 3),
            ], TileID.TinkerersWorkbench);
            recipe.Register();


        }
    }

    public class VenomRingPlayer : ModPlayer
    {
        public bool venomRingActive;

        public override void ResetEffects()
        {
            venomRingActive = false;
        }
    }
    public class VenomRingGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (!player.GetModPlayer<VenomRingPlayer>().venomRingActive)
                return;

            // Apply Venom
            target.AddBuff(BuffID.Venom, 180);

            // Bonus damage vs poisoned targets
            if (target.HasBuff(BuffID.Venom) || target.HasBuff(BuffID.Poisoned))
                hit.SourceDamage *= (int)1.10f;
        }
    }
}
