using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PostHardmode.Accessories;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
//using NaturiumMod.Content.ModPlayers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
        //player.GetModPlayer<LeodrakePlayer>().hasLeodrakeMedallion = true;

        if (player.HeldItem.type != ModContent.ItemType<LeodrakesLeafstorm>()
            && player.HeldItem.type != ModContent.ItemType<LeodrakesYoyo>())
        {
            return;
        }
        player.GetArmorPenetration(DamageClass.Generic) += 2f;
        player.GetAttackSpeed(DamageClass.Melee) += 0.2f;
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
