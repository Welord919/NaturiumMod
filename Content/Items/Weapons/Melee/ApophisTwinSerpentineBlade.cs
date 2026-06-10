using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons.Melee;

public class ApophisTwinSerpentineBlade : ModItem
{
    private int chargeTimer;

    public override string Texture => "NaturiumMod/Assets/Items/Weapons/ApophisTwinSerpentineBlade";

    public override void SetDefaults()
    {
        Item.width = 54;
        Item.height = 54;

        Item.damage = 38;
        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 5.5f;
        Item.crit = 8;

        Item.useTime = 24;
        Item.useAnimation = 24;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.autoReuse = true;
        Item.UseSound = SoundID.Item20;

        Item.shoot = ModContent.ProjectileType<ApophisSerpentProj>();
        Item.shootSpeed = 12f;

        Item.value = Item.buyPrice(0, 20, 0, 0);

        Item.rare = ItemRarityID.Pink;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<ApophisFangblade>(), 1),
            new(ItemID.MythrilBar, 18),
            new(ItemID.CursedFlame, 12)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
    public override bool Shoot(Player player, Terraria.DataStructures.EntitySource_ItemUse_WithAmmo source,
        Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        // LEFT CLICK ONLY
        if (player.altFunctionUse != 2)
        {
            Vector2 v1 = velocity.RotatedBy(MathHelper.ToRadians(10));
            Vector2 v2 = velocity.RotatedBy(MathHelper.ToRadians(-10));

            Projectile.NewProjectile(source, position, v1, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, v2, type, damage, knockback, player.whoAmI);
        }

        return false;
    }
    public override void ModifyItemScale(Player player, ref float scale)
    {
        scale = 0.5f;
    }
    public class ApophisSerpentProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/ApophisLargeProj";

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.penetrate = 3;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.extraUpdates = 1;
            Main.projFrames[Projectile.type] = 8;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;

            Projectile.alpha = 50;
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            NPC target = Projectile.FindTargetWithinRange(500f);
            if (target != null)
            {
                Vector2 desired = Projectile.DirectionTo(target.Center) * 10f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, 0.08f);
            }

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.CursedTorch);
            d.noGravity = true;
            d.scale = 1.3f;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.ShadowFlame, 180);
            target.AddBuff(BuffID.Venom, 180);

            if (Main.rand.NextFloat() < 0.20f)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    target.Center,
                    Vector2.Zero,
                    ProjectileID.CursedFlameFriendly,
                    Projectile.damage,
                    0f,
                    Projectile.owner
                );
            }
        }
    }
}
