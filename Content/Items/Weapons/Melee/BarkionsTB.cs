using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Materials;

namespace NaturiumMod.Content.Items.Weapons.Melee;

public class BarkionsTB : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Weapons/BarkionsTB";

    public override void SetDefaults()
    {
        Item.DamageType = DamageClass.Melee;
        Item.damage = 23;
        Item.width = 40;
        Item.height = 50;
        Item.useTime = 33;
        Item.useAnimation = 30;
        Item.knockBack = 4.5f;
        Item.crit = 8;
        Item.UseSound = SoundID.Item1;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.value = Item.buyPrice(0, 3, 0, 0);
        Item.rare = ItemRarityID.Green;
        Item.autoReuse = true;

        Item.shootSpeed = 12f;
        Item.shoot = ProjectileID.ChlorophyteOrb;

    }

    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Poisoned, 240);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 15),
            new(ItemID.BladeofGrass, 1)
        ], TileID.Anvils);
        recipe.Register();
    }
}
