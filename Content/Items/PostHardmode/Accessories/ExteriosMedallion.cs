using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PostHardmode.Weapons;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Accessories;
using NaturiumMod.Content.Items.PostHardmode.Materials;
using NaturiumMod.Content.ModPlayers;

namespace NaturiumMod.Content.Items.PostHardmode.Accessories;

internal class ExteriosMedallion : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Accessories/ExteriosMedallion";

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.rare = ItemRarityID.Green;
        Item.accessory = true;
        Item.value = 100000;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {

        player.GetModPlayer<ExteriosPlayer>().hasExteriosMedallion = true;

        if (player.HeldItem.type == ModContent.ItemType<ExteriosCannon>())
        {
            player.GetArmorPenetration(DamageClass.Generic) += 3f;
            player.GetDamage(DamageClass.Melee) += 0.1f;
            player.GetDamage(DamageClass.Ranged) += 0.1f;
            player.GetDamage(DamageClass.Magic) += 0.1f;
            player.GetDamage(DamageClass.Summon) += 0.1f;
        }

    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<CharmBase>(), 1),
            new(ModContent.ItemType<InfusedNaturiumBar>(), 15)
        ], TileID.MythrilAnvil);
        recipe.Register();

        
    }
}
