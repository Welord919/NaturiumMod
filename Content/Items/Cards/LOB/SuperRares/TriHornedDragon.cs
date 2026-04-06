using Microsoft.Xna.Framework;
using NaturiumMod.Content.BuffsDebuffs;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.SuperRares
{
    public class TriHornedDragon : BaseCardSuper
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/TriHornedDragon";
        private int burstsFired = 0;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.consumable = false;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.damage = 28;
            Item.knockBack = 3.5f;
            Item.UseSound = SoundID.Item1;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 20);
            burstsFired++;
            if (burstsFired >= 3)
            {
                burstsFired = 0; 
                Item.stack--;   
                if (Item.stack <= 0)
                    Item.TurnToAir();
            }

            float baseSpeed = Item.shootSpeed == 0 ? 8f : Item.shootSpeed;
            float spread = MathHelper.ToRadians(10f);

            Vector2 direction = Vector2.Normalize(Main.MouseWorld - player.Center);

            for (int i = 0; i < 3; i++)
            {
                float angle = MathHelper.Lerp(-spread, spread, i / 2f);
                Vector2 vel = direction.RotatedBy(angle) * baseSpeed;

                Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    player.Center,
                    vel,
                    ModContent.ProjectileType<DragonHorn>(),
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI
                );
            }
            return true;
        }
    }
    public class DragonHorn : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/TriHornedDragonHorn";
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 120;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
        }

        public override void AI()
        {
            // Simple rotation and dust
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            if (Main.rand.NextBool(6))
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Blood, Projectile.velocity.X * 0.1f, Projectile.velocity.Y * 0.1f);
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {

            ApplyBleed(target);
        }
        private void ApplyBleed(NPC target)
        {
            target.AddBuff(ModContent.BuffType<BleedDebuff>(), 600);

            var g = target.GetGlobalNPC<BleedDebuffNPC>();
            g.hasBleed = true;
        }
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            // small bounce or die
            Projectile.Kill();
            return false;
        }
    }
    

}
