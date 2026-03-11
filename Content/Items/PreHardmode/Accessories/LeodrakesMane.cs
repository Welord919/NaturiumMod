using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.General.Projectiles;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Accessories;
public class LeodrakesMane : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/LeodrakesMane";
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 1);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<LeodrakesManePlayer>().leodrakeManeEquipped = true;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<CharmBase>(), 1),
            new(ModContent.ItemType<CameliaPetal>(), 10),
            new(ModContent.ItemType<RoseIcon>(), 3)
        ], TileID.Anvils);
        recipe.Register();

    }
}
public class LeodrakesManePlayer : ModPlayer
{
    public bool leodrakeManeEquipped;
    private int leodrakeCooldown = 0;

    public override void ResetEffects()
    {
        leodrakeManeEquipped = false;
    }

    public override void PostUpdate()
    {
        if (leodrakeCooldown > 0)
            leodrakeCooldown--;
    }

    public override void OnHurt(Player.HurtInfo hurtInfo)
    {
        if (!leodrakeManeEquipped)
            return;

        if (leodrakeCooldown > 0)
            return;

        leodrakeCooldown = 60; // 1 second cooldown

        // Sound
        SoundEngine.PlaySound(SoundID.Roar with { Volume = 0.8f, Pitch = 0.3f }, Player.Center);

        // Fire 5 projectiles in a circle
        int amount = 5;
        float speed = 8f;

        for (int i = 0; i < amount; i++)
        {
            float angle = MathHelper.TwoPi * i / amount;
            Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;

            Projectile.NewProjectile(
                Player.GetSource_OnHurt(Player),
                Player.Center,
                velocity,
                ModContent.ProjectileType<LeodrakesManeProj>(),
                20,       // damage
                2f,       // knockback
                Player.whoAmI
            );
        }
    }
}