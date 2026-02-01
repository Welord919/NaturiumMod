using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.General.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Armor;

[AutoloadEquip(EquipType.Head)]
public class BarkionsHelmet : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Armor/BarkionsHelmet";

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.value = 1500;
        Item.rare = ItemRarityID.Blue;
        Item.defense = 4;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetKnockback(DamageClass.Ranged) += 0.1f;
    }

    public override bool IsArmorSet(Item head, Item body, Item legs) =>
        body.type == Mod.Find<ModItem>("BarkionsChestplate").Type && legs.type == Mod.Find<ModItem>("BarkionsLeggings").Type;

    public override void UpdateArmorSet(Player player)
    {
        
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [new(ModContent.ItemType<BarkionsBark>(), 75)], TileID.Anvils);
        recipe.Register();
    }
}
