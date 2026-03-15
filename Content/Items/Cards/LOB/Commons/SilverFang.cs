using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class SilverFang : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/Silverfang";

        public override void SetDefaults()
        {
            Item.damage = 2;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Roar;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Blue;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.knockBack = 7f;
            Item.shoot = ModContent.ProjectileType<SilverFangShout>();
            Item.shootSpeed = 0f;
            Item.value = Item.buyPrice(silver: 25);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);

            int proj = Projectile.NewProjectile(
                player.GetSource_ItemUse(Item),
                player.Center,
                Vector2.Zero,
                ModContent.ProjectileType<SilverFangShout>(),
                Item.damage,                // damage
                Item.knockBack,   // knockback
                player.whoAmI
            );

            // Store knockback manually
            Main.projectile[proj].ai[1] = Item.knockBack;

            return true;
        }

    }
    // ============================================================
    //  SHOUT PROJECTILE — Expanding Knockback Wave
    // ============================================================
    public class SilverFangShout : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/Blank";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 20;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            float radius = Projectile.ai[0] += 20f;
            float kb = Projectile.ai[1]; // <-- stored knockback

        for (int i = 0; i < 20; i++)
            {
                Vector2 offset = Main.rand.NextVector2Circular(radius, radius);
                Dust.NewDustPerfect(Projectile.Center + offset, DustID.Smoke, Vector2.Zero, 150, Color.White, 1.2f);
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                    continue;

                if (Vector2.Distance(Projectile.Center, npc.Center) <= radius)
                {
                    int direction = npc.Center.X < Projectile.Center.X ? -1 : 1;

                    // Apply damage + base-game knockback
                    npc.SimpleStrikeNPC(Projectile.damage, direction);

                    // Apply knockback manually (vanilla formula)
                    npc.velocity.X += direction * kb * npc.knockBackResist;
                }
            }
        }

    }
}