using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.Weapons.PreHardmode;

namespace NaturiumMod.Content.Items.Accessories.PreHardmodeAccessories;

internal class BarkionsMedallion : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Accessories/PreHardmode/BarkionsMedallion";

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

        if (player.HeldItem.type != ModContent.ItemType<BarkionsSS>()
            && player.HeldItem.type != ModContent.ItemType<RoseWhip>()
            && player.HeldItem.type != ModContent.ItemType<CosmoItem>()
            && player.HeldItem.type != ModContent.ItemType<BarkionsTB>())
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
        recipe = RecipeUtils.GetNewRecipe(recipe, (ModContent.ItemType<LeodrakesMedallion>(), 1), TileID.Anvils);
        recipe.Register();

        recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, (ModContent.ItemType<ExteriosMedallion>(), 1), TileID.Anvils);
        recipe.Register();
    }
}
