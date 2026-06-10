using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Helpers
{
    public class ModPlayers : ModPlayer
    {
        public bool hasUltiBuild;

        public override void ResetEffects()
        {
            hasUltiBuild = false;
        }
    }
    public class ExteriosPlayer : ModPlayer
    {
        public bool hasExteriosMedallion;

        public override void ResetEffects()
        {
            hasExteriosMedallion = false;
        }
    }
    public class StarsteelPlayer : ModPlayer
    {
        public int starsteelCooldown;

        public override void ResetEffects()
        {
            if (starsteelCooldown > 0)
                starsteelCooldown--;
        }
    }
    public class StarsteelGunPlayer : ModPlayer
    {
        public int starburstCooldown;
        public int burstShots;
        public int fireTimer;

        public override void ResetEffects()
        {
            if (starburstCooldown > 0) starburstCooldown--;
            if (fireTimer > 0) fireTimer--;
        }
    }

    public class FabledBladePlayer : ModPlayer
    {
        public float percentBuff = 0f; // 0% → 10% or 15%
        public bool usingAndwraith = false;
        public bool usingLeviathan = false;

        public override void ResetEffects()
        {
            usingAndwraith = false;
            usingLeviathan = false;
        }

        public override void OnHitByNPC(NPC npc, Player.HurtInfo hurtInfo)
        {
            percentBuff = 0f;
        }

        public override void OnHitByProjectile(Projectile proj, Player.HurtInfo hurtInfo)
        {
            percentBuff = 0f;
        }

        public override void PostUpdate()
        {
            if (percentBuff > 0f)
            {
                Player.GetDamage(DamageClass.Melee) += percentBuff;

                // Light visual scales with percent
                float strength = 0.2f + percentBuff * 2f;
                Lighting.AddLight(Player.Center, new Color(255, 80, 60).ToVector3() * strength);
            }
        }
    }
    public class FabledBladeGlobalNPC : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            Player player = Main.player[npc.lastInteraction];
            if (player == null || !player.active)
                return;

            var mp = player.GetModPlayer<FabledBladePlayer>();

            if (!mp.usingAndwraith && !mp.usingLeviathan)
                return;

            // Increment amounts
            float increment = mp.usingAndwraith ? 0.01f : 0.02f; // 1% or 2%
            float max = mp.usingAndwraith ? 0.10f : 0.15f;       // 10% or 15%

            mp.percentBuff += increment;

            if (mp.percentBuff > max)
                mp.percentBuff = max;

            // Show % as whole number
            int shownPercent = (int)(mp.percentBuff * 100f);

            CombatText.NewText(player.Hitbox, Color.OrangeRed,
                $"{shownPercent}% Fabled Power");
        }
    }
}
