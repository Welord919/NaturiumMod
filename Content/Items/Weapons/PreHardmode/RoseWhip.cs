using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.Projectiles;
using NaturiumMod.Content.Items.Materials.PreHardmode;

namespace NaturiumMod.Content.Items.Weapons.PreHardmode;

public class RoseWhip : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/PreHardmode/RoseWhip";

    public override void SetDefaults()
    {
        Item.DefaultToWhip(ModContent.ProjectileType<RoseWhipProj>(), 29, 6f, 4);
        Item.shootSpeed = 4;
        Item.rare = ItemRarityID.Green;
        Item.channel = true;

        Item.DamageType = DamageClass.Summon;
        Item.width = 40;
        Item.height = 40;

        Item.value = Item.buyPrice(gold: 1);
        Item.rare = ItemRarityID.Green;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<BarkionsBark>(), 36), (ItemID.Vine, 7)], TileID.Anvils);
        recipe.Register();
    }

    public override bool MeleePrefix()
    {
        return true;
    }
}
