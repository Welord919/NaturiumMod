using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PostHardmode.Accessories;

namespace NaturiumMod.Content.Items.PreHardmode.Accessories;

internal class LeodrakesMedallion : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/LeodrakesMedallion";

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.rare = ItemRarityID.Green;
        Item.accessory = true;
        Item.value = 100000;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        if (player.HeldItem.type != ModContent.ItemType<LeodrakesLeafstorm>() && player.HeldItem.type != ModContent.ItemType<LeodrakesYoyo>())
        {
            return;
        }

        player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
        player.GetKnockback(DamageClass.Ranged) += 0.4f;
        player.manaCost -= 0.2f;
        player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 1f;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<CharmBase>(), 1),
            new(ModContent.ItemType<NaturiumBar>(), 15),
            new(ModContent.ItemType<RoseIcon>(), 1)
        ], TileID.Anvils);
        recipe.Register();

    }
}
