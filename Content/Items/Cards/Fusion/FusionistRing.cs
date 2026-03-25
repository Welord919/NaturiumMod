using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion
{ 
    public class FusionistRing : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FusionistRing";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FusionistRingPlayer>().fusionistRingActive = true;
            var boost = player.GetModPlayer<WeaponBoostPlayer>();
            boost.activeBoosts["Fusion"] = true;
        }
        public override void UpdateInventory(Player player)
        {
            player.blockRange += 1;
            player.moveSpeed += 0.10f;
            player.runAcceleration *= 1.03f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<Fusionist>(), 1),
        new(ModContent.ItemType<NaturiumBar>(), 5),
        new(ModContent.ItemType<CharmBase>(), 1)
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
public class FusionistRingPlayer : ModPlayer
{
    public bool fusionistRingActive;

    public override void ResetEffects()
    {
        fusionistRingActive = false;
    }

    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        if (fusionistRingActive && item.DamageType == ModContent.GetInstance<CardDamage>())
        {
            damage *= 1.05f; // +5% card damage
        }
    }

    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (fusionistRingActive && proj.DamageType == ModContent.GetInstance<CardDamage>())
        {
            modifiers.SourceDamage *= 1.05f; // +10% fusion monster damage
        }
    }
}

