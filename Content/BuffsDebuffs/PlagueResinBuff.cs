using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.BuffsDebuffs
{
    public class PlagueInfusionBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/BuffsDebuffs/PlagueInfusionBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoSave[Type] = false;
            Main.debuff[Type] = false;
            Main.buffNoTimeDisplay[Type] = false;
        }
    }
    public class PlagueInfusionGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;

        // Melee weapons that use item hitboxes
        public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (item.DamageType == DamageClass.Melee &&
                player.HasBuff(ModContent.BuffType<PlagueInfusionBuff>()))
            {
                ApplyDecay(target);
            }
        }

        private void ApplyDecay(NPC target)
        {
            target.AddBuff(ModContent.BuffType<DecayDebuff>(), 300);

            var g = target.GetGlobalNPC<DecayGlobalNPC>();
            g.hasDecay = true;
        }
    }
}
