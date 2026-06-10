using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.BuffsDebuffs
{
    public class DecayDebuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/BuffsDebuffs/DecayDebuff";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;          // It's a debuff
            Main.buffNoSave[Type] = true;      // Doesn't persist on reload
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true; // Allows minions to benefit
        }
    }
    public class DecayGlobalNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool hasDecay;

        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<DecayDebuff>()))
            {
                hasDecay = true;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (hasDecay)
            {
                int dps = 6;
                npc.lifeRegen -= dps;

                if (damage < dps / 2)
                    damage = dps / 2;
            }
        }

        public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
        {
            if (hasDecay)
            {
                modifiers.Defense.Flat -= 4;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            if (!npc.HasBuff(ModContent.BuffType<DecayDebuff>()))
            {
                hasDecay = false;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (hasDecay)
            {
                // Purple plague dust
                if (Main.rand.NextBool(4))
                {
                    int dust = Dust.NewDust(
                        npc.position,
                        npc.width,
                        npc.height,
                        DustID.Poisoned, // base dust type
                        0f, 0f,
                        150,
                        new Color(180, 0, 255), // purple tint
                        1.3f
                    );

                    Main.dust[dust].noGravity = true;
                }

                // Slight purple tint on the NPC
                drawColor = Color.Lerp(drawColor, new Color(180, 0, 255), 0.35f);
            }
        }
    }
}
