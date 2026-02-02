using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PostHardmode.Materials;

namespace NaturiumMod.Content.Items.PostHardmode.Weapons;

public class ExteriosCannon : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Weapons/ExteriosCannon";

    public override void SetDefaults()
    {
        Item.Size = new(62, 32);
        Item.scale = 1.0f;
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(0, 8, 0, 0);

        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Shoot;
        Item.autoReuse = true;
        Item.UseSound = SoundID.Item36;

        Item.DamageType = DamageClass.Ranged;
        Item.damage = 40;
        Item.knockBack = 6.5f;
        Item.noMelee = true;

        Item.shoot = ProjectileID.PurificationPowder;
        Item.shoot = Mod.Find<ModProjectile>("ExteriosFangProj").Type;
        Item.shootSpeed = 55f;
        Item.useAmmo = Mod.Find<ModItem>("ExteriosFang").Type;
    }

    public override bool CanConsumeAmmo(Item ammo, Player player)
    {
        float chance = 0.10f; // base 10%

        // If the player has the medallion equipped, add 15%
        if (player.GetModPlayer<ExteriosPlayer>().hasExteriosMedallion)
            chance += 0.15f;

        // Roll the ammo-save chance
        if (Main.rand.NextFloat() < chance)
            return false; // do NOT consume ammo

        return true; // consume ammo normally
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<InfusedNaturiumBar>(), 15),
            new(ModContent.ItemType<InfusedNaturiumGunParts>(), 1)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
}
