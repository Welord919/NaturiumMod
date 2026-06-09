using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.ApophisItems;
public class ApophisSword : ModItem
{
  
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/ApophisSword";

    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;

        Item.damage = 16; 
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 4.5f;
        Item.crit = 4;

        Item.useTime = 30;
        Item.useAnimation = 30;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item20; 

        Item.shoot = ModContent.ProjectileType<ApophisProj>();
        Item.shootSpeed = 8f;

        Item.value = Item.buyPrice(0, 0, 80, 0);
        Item.rare = ItemRarityID.Blue;

        Item.noMelee = false;
        Item.noUseGraphic = false;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.EnchantedSword, 1),
            new(ItemID.Amber, 15),
            new(ItemID.GoldBar, 10)
        ], TileID.Anvils);
        recipe.Register();
    }
}
public class ApophisProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/ApophisProj";

    public override void SetDefaults()
    {
        Projectile.width = 14;
        Projectile.height = 14;
        Main.projFrames[Projectile.type] = 4;

        Projectile.aiStyle = 0;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Generic;

        Projectile.penetrate = 1;
        Projectile.timeLeft = 120;

        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;

        Projectile.light = 0.5f;
        Projectile.extraUpdates = 1;

        Projectile.alpha = 200;
    }

    public override void AI()
    {
        Projectile.velocity *= 0.99f;

        Projectile.frameCounter++;
        if (Projectile.frameCounter >= 10)
        {
            Projectile.frameCounter = 0;
            Projectile.frame++;
            if (Projectile.frame >= Main.projFrames[Projectile.type])
                Projectile.frame = 0;
        }

        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

        Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
        d.noGravity = true;
        d.scale = 0.5f;
        d.velocity *= 0.2f;
    }
}
