using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint
{
    public class PetiteDragon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteDragon";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.noMelee = true;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
            Item.damage = 8;
            Item.knockBack = 1f;

            Item.buffType = ModContent.BuffType<PetiteDragonBuff>();
            Item.shoot = ModContent.ProjectileType<PetiteDragonMinion>();

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 50);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(Item.buffType, 2);
            return true;
        }
    }

    public class PetiteDragonBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteDragonBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PetiteDragonMinion>()] > 0)
                player.buffTime[buffIndex] = 60 * 120;
            else
                player.DelBuff(buffIndex);
        }
    }
    public class PetiteDragonMinion : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteDragonMinion2";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionShot[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 26;
            Projectile.height = 26;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>(); 
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.HasBuff(ModContent.BuffType<PetiteDragonBuff>()))
            {
                Projectile.Kill();
                return;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 6)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= 3)
                    Projectile.frame = 0;
            }
            if (Projectile.velocity.X > 0.1f)
                Projectile.spriteDirection = -1;
            else if (Projectile.velocity.X < -0.1f)
                Projectile.spriteDirection = 1;

            Projectile.direction = Projectile.spriteDirection;


            Vector2 idlePos = player.Center + new Vector2(40 * Projectile.minionPos, -70);
            Vector2 toIdle = idlePos - Projectile.Center;
            Projectile.velocity = Vector2.Lerp(Projectile.velocity, toIdle.SafeNormalize(Vector2.Zero) * 6f, 0.05f);

            NPC target = FindTarget(player);
            if (target != null)
            {
                Vector2 shootDir = (target.Center - Projectile.Center).SafeNormalize(Vector2.Zero);
                Projectile.ai[0]++;

                if (Projectile.ai[0] >= 40)
                {
                    Projectile.ai[0] = 0;
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        shootDir * 8f,
                        ModContent.ProjectileType<PetiteDragonShot>(),
                        Projectile.damage,
                        0f,
                        player.whoAmI
                    );
                    SoundEngine.PlaySound(SoundID.Item25, Projectile.position);

                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center,
                        shootDir * 8f,
                        ModContent.ProjectileType<PetiteDragonShot>(),
                        Projectile.damage,
                        0f,
                        player.whoAmI
                    );

                }
            }
        }

        private NPC FindTarget(Player player)
        {
            NPC chosen = null;
            float dist = 500f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, Projectile.Center) < dist)
                {
                    dist = Vector2.Distance(npc.Center, Projectile.Center);
                    chosen = npc;
                }
            }

            return chosen;
        }
    }
    public class PetiteDragonShot : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteLightProj";
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.light = 0.4f;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame);
            Main.dust[d].scale = 0.6f;
            Main.dust[d].velocity *= 0.2f;
            Main.dust[d].noGravity = true;
            Main.dust[d].fadeIn = 0.1f;
            Main.dust[d].alpha = 200; // fades out fast
        }

    }

}