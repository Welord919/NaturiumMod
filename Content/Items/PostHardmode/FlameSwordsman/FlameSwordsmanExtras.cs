using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion.FusionCards;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class FlameSwordsmanPlayer : ModPlayer
{
    // Gauntlet effects
    public bool salamandraGauntletEquipped;

    // Sword effects
    public bool flameSwordsmanBladeEquipped;

    // Token system
    public int flameTokens;
    public const int MaxFlameTokens = 20;
    public int flameBurstCooldown;
    public float salamandraMeleeScale = 1f;

    public override void ResetEffects()
    {
        salamandraGauntletEquipped = false;
        salamandraMeleeScale = 1f;
        flameSwordsmanBladeEquipped = false;
    }

    public override void PostUpdate()
    {
        if (flameBurstCooldown > 0)
            flameBurstCooldown--;
    }

    // ============================================================
    // 🔥 Helper: Spawn Dark Fire Dragon (HEAD ONLY)
    // ============================================================
    public void SpawnDarkFireDragon(Player player, int damage, float knockback)
    {
        // Direction toward mouse
        Vector2 dir = Vector2.Normalize(Main.MouseWorld - player.Center);
        if (dir.LengthSquared() < 0.1f)
            dir = player.DirectionTo(Main.MouseWorld);

        Vector2 velocity = dir * 8f; // dragon speed

        int proj = Projectile.NewProjectile(
            player.GetSource_FromThis(),
            player.Center,
            velocity,
            ModContent.ProjectileType<DarkFireDragonHead>(),
            damage,
            knockback,
            player.whoAmI
        );

        // Mark as sword‑spawned (no orb)
        if (proj >= 0 && proj < Main.maxProjectiles)
        {
            if (Main.projectile[proj].ModProjectile is DarkFireDragonHead head)
            {
                head.spawnedBySword = true;
            }
        }
    }


    // ============================================================
    // 🔥 Token System — Called by the sword when hitting an enemy
    // ============================================================
    public void AddFlameToken()
    {
        if (flameTokens < MaxFlameTokens)
            flameTokens++;

        // When max tokens reached AND cooldown is ready
        if (flameTokens >= MaxFlameTokens && flameBurstCooldown == 0)
        {
            flameTokens = 0;
            flameBurstCooldown = 180; // 3 seconds

            Player player = Player;

            // Melee-scaled dragon damage
            int dmg = (int)player.GetTotalDamage(DamageClass.Melee).ApplyTo(80);
            float kb = 4f;

            // Spawn the Dark Fire Dragon
            SpawnDarkFireDragon(player, dmg, kb);
        }
    }

    // Salamandra Gauntlet melee scale boost
    public class SalamandraGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        // Increase melee weapon size
        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            var fs = player.GetModPlayer<FlameSwordsmanPlayer>();

            if (fs.salamandraGauntletEquipped && item.DamageType == DamageClass.Melee)
            {
                scale *= 1.25f; // 25% bigger melee weapons
            }
        }

        // Apply On Fire to melee weapons
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var fs = player.GetModPlayer<FlameSwordsmanPlayer>();

            if (fs.salamandraGauntletEquipped && item.DamageType == DamageClass.Melee)
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
        }
    }
    public class SalamandraGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override void OnHitNPC(Projectile projectile, NPC target, NPC.HitInfo hit, int damageDone)
        {
            var fs = Main.player[projectile.owner].GetModPlayer<FlameSwordsmanPlayer>();

            if (fs.salamandraGauntletEquipped && projectile.DamageType == DamageClass.Melee)
            {
                target.AddBuff(BuffID.OnFire, 180);
            }
        }
    }

}
