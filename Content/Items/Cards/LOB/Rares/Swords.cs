using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Rares
{
    public class Swords : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/Swords";
        private int usesLeft = 3;

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item8;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.damage = 50;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
            Item.shoot = ModContent.ProjectileType<SwordsProj>();
            Item.shootSpeed = 0f;

            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(silver: 75);
        }
        public override bool CanUseItem(Player player)
        {
            if (usesLeft <= 0)
                return false;

            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
                return false;

            return true;
        }
        public override bool? UseItem(Player player)
        {
            usesLeft--;
            if (usesLeft <= 0)
            {
                Item.stack--;
                usesLeft = 3;
            }
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 30);
            Vector2 pos = Main.MouseWorld;
            Projectile.NewProjectile(
                player.GetSource_ItemUse(Item),
                pos,
                Vector2.Zero,
                Item.shoot,
                Item.damage,
                6f,
                player.whoAmI
            );
            return true;
        }

    }
    public class SwordsProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/SwordsProj";

        private const int FORM_TIME = 20;
        private bool slammed = false;
        private bool stuck = false;
        private int stuckTimer = 0;

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 80;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 600;
            Projectile.ignoreWater = true;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Fade in
            if (!slammed)
            {
                if (Projectile.ai[0] < FORM_TIME)
                {
                    Projectile.alpha = (int)MathHelper.Lerp(255, 0, Projectile.ai[0] / FORM_TIME);
                    Projectile.ai[0]++;

                    Projectile.position = Main.MouseWorld - new Vector2(Projectile.width / 2f, Projectile.height);
                    return;
                }

                // Slam
                slammed = true;
                Projectile.friendly = true;
                Projectile.velocity = new Vector2(0, 18f);
                SoundEngine.PlaySound(SoundID.Item71, Projectile.position);
            }

            // Detect ground impact
            if (!stuck && Projectile.velocity.Y > 0 &&
                Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
            {
                stuck = true;
                Projectile.velocity = Vector2.Zero;
                Projectile.friendly = false;
                Projectile.tileCollide = false;
            }

            // Stuck phase
            if (stuck)
            {
                stuckTimer++;

                // Dust
                if (Main.rand.NextBool(3))
                {
                    int d = Dust.NewDust(
                        Projectile.position + new Vector2(-14f, 0f),
                        Projectile.width,
                        Projectile.height,
                        DustID.GoldCoin
                    );
                    Main.dust[d].noGravity = true;
                    Main.dust[d].scale = 1.1f;
                    Main.dust[d].velocity *= 0.2f;
                }

                // Root enemies touching the sword
                RootAndDamageEnemies();

                // Fade-out after 3 seconds
                if (stuckTimer >= 180)
                {
                    Projectile.alpha += 15;
                    if (Projectile.alpha >= 255)
                        Projectile.Kill();
                }
            }

        }
        private int damageTimer = 0;
        private void RootAndDamageEnemies()
        {
            damageTimer++;

            // Only damage once every 0.5 seconds (30 ticks)
            bool canDamageNow = damageTimer >= 30;

            Rectangle hitbox = Projectile.Hitbox;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                    continue;

                if (npc.Hitbox.Intersects(hitbox))
                {
                    // Root the enemy
                    npc.velocity = Vector2.Zero;
                    npc.netUpdate = true;

                    if (canDamageNow)
                    {
                        npc.SimpleStrikeNPC(Projectile.damage, 0);
                    }
                }
            }

            // Reset timer only after a damage tick
            if (canDamageNow)
                damageTimer = 0;
        }



        public class RootedDebuff : ModBuff
        {
            public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/DecayDebuff";
            public override void SetStaticDefaults()
            {
                Main.debuff[Type] = true;
                Main.buffNoSave[Type] = true;
                Main.buffNoTimeDisplay[Type] = false;
            }
        }

        public override bool? CanDamage()
        {
            return slammed && !stuck && Projectile.velocity.Y > 0;
        }
    }

}