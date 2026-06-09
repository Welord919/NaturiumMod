using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PostHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Hardmode.ApophisItems;

public class ApophisFangblade : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/ApophisFangBlade";

    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 48;

        Item.damage = 26;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 5f;
        Item.crit = 6;

        Item.useTime = 26;
        Item.useAnimation = 26;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item20;

        Item.shoot = ModContent.ProjectileType<ApophisFangProj>();
        Item.shootSpeed = 10f;

        Item.value = Item.buyPrice(0, 2, 50, 0);
        Item.rare = ItemRarityID.LightRed;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<ApophisSword>(), 1),
            new(ModContent.ItemType<PoisonBulb>(), 6),
            new(ItemID.SpiderFang, 12)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
    public override void ModifyItemScale(Player player, ref float scale)
    {
        scale = 0.5f;
    }
}
public class ApophisFangProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/ApophisLargeProj";

    public override void SetDefaults()
    {
        Projectile.CloneDefaults(ModContent.ProjectileType<ApophisProj>());
        Projectile.penetrate = 3;
        Projectile.extraUpdates = 1;

        Main.projFrames[Projectile.type] = 8; 
        
        Projectile.ignoreWater = true;
        Projectile.tileCollide = false;

        Projectile.light = 0.5f;
        Projectile.extraUpdates = 1;

        Projectile.alpha = 50;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Poisoned, 180);
        target.AddBuff(BuffID.Venom, 120);

        // 10% serpent bite
        if (Main.rand.NextFloat() < 0.10f)
        {
            Vector2 biteVel = Main.rand.NextVector2Circular(4f, 4f);
            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                target.Center,
                biteVel,
                ProjectileID.VenomFang,
                Projectile.damage / 2,
                0f,
                Projectile.owner
            );
        }
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

