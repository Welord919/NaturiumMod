using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PostHardmode.Accessories;

namespace NaturiumMod.Content.Items.PreHardmode.Accessories;

internal class BarkionsMedallion : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/BarkionsMedallion";

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.rare = ItemRarityID.Green;
        Item.accessory = true;
        Item.value = 100000;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Generic) += 0.05f;

        if (!BarkionItemTags.IsBarkionItem(player.HeldItem))
        {
            return;
        }

        player.GetArmorPenetration(DamageClass.Generic) += 1f;
        player.GetKnockback(DamageClass.Melee) += 0.1f;
        player.GetAttackSpeed(DamageClass.Ranged) += 0.15f;
        player.manaCost -= 0.2f;
        player.GetDamage(DamageClass.Summon) += 0.1f;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [new(ModContent.ItemType<LeodrakesMedallion>(), 1)], TileID.Anvils);
        recipe.Register();

        recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [new(ModContent.ItemType<ExteriosMedallion>(), 1)], TileID.Anvils);
        recipe.Register();
    }
}
