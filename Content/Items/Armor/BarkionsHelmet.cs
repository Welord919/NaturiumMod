using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Materials.PreHardmodeMaterials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Armor;

[AutoloadEquip(EquipType.Head)]
public class BarkionsHelmet : ModItem
{
    public override void SetStaticDefaults() { }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.value = 1500;
        Item.rare = ItemRarityID.Blue;
        Item.defense = 4;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetKnockback(DamageClass.Ranged) += 0.1f;
    }

    public override void UpdateArmorSet(Player player)
    {
        player.setBonus = "Summons a Cosmobeet to fight with you";
        //player.GetDamage(DamageClass.Generic) += 0.05f;

        if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.CosmoMinionProj>()] < 1)
        {
            Projectile.NewProjectile(player.GetSource_Misc("SetBonus"), player.Center, new Vector2(0, 0), ModContent.ProjectileType<Projectiles.CosmoMinionProj>(), 11, 2f, player.whoAmI);
        }
    }

    public override bool IsArmorSet(Item head, Item body, Item legs)
    {
        return body.type == Mod.Find<ModItem>("BarkionsChestplate").Type && legs.type == Mod.Find<ModItem>("BarkionsLeggings").Type;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, (ModContent.ItemType<BarkionsBark>(), 75), TileID.Anvils);
        recipe.Register();
    }
}
