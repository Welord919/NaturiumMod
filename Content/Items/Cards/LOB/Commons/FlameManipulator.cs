using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ModLoader;
namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class FlameManipulator : BaseCardCommon
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/FlameMani";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = ModContent.BuffType<FireDamageBuff>();
            Item.buffTime = 60 * 40;
        }
        protected override void OnCardUse(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
            player.AddBuff(ModContent.BuffType<FireDamageBuff>(), 60 * 30);
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
            {
                return false;
            }
            return base.CanUseItem(player);
        }
    }
    public class FireDamageBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/FireDamageBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<FlameManipulatorPlayer>().flameManipulatorActive = true;
        }
        public class FlameManipulatorPlayer : ModPlayer
        {
            public bool flameManipulatorActive;

            public override void ResetEffects()
            {
                flameManipulatorActive = false;
            }

            // ------------------------------------------------------------
            // 1. FIRE DAMAGE BOOST (ITEM DAMAGE)
            // ------------------------------------------------------------
            public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
            {
                if (!flameManipulatorActive)
                    return;

                // Fire-tagged modded cards
                if (WeaponTag.ItemTags.TryGetValue(item.type, out var tags) &&
                    tags.Contains("Fire"))
                {
                    damage *= 1.05f;
                }

                // Vanilla + modded fire weapons
                if (FireWeaponRegistry.FireItems.Contains(item.type))
                {
                    damage *= 1.05f;
                }
            }

            // ------------------------------------------------------------
            // 2. FIRE DAMAGE BOOST (PROJECTILE DAMAGE)
            // ------------------------------------------------------------
            public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
            {
                if (!flameManipulatorActive)
                    return;

                // Fire-tagged modded projectiles
                if (WeaponTagProj.ProjTags.TryGetValue(proj.type, out var tags) &&
                    tags.Contains("Fire"))
                {
                    modifiers.SourceDamage *= 1.05f;
                }

                // Vanilla + modded fire projectiles
                if (FireWeaponRegistry.FireProjectiles.Contains(proj.type))
                {
                    modifiers.SourceDamage *= 1.05f;
                }
            }
        }
    }
}
