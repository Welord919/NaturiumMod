using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.BuffsDebuffs
{
    public class BleedDebuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteDragonBuff";
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;          // It's a debuff
            Main.buffNoSave[Type] = true;      // Doesn't persist on reload
            Main.buffNoTimeDisplay[Type] = false;
            BuffID.Sets.IsATagBuff[Type] = true; // Allows minions to benefit
        }
    }
    public class BleedDebuffNPC : GlobalNPC
    {
        public override bool InstancePerEntity => true;

        public bool hasBleed;

        public override void PostAI(NPC npc)
        {
            if (npc.HasBuff(ModContent.BuffType<BleedDebuff>()))
            {
                hasBleed = true;
            }
        }

        public override void UpdateLifeRegen(NPC npc, ref int damage)
        {
            if (hasBleed)
            {
                // 1% max HP damage per second
                int percentDamage = (int)(npc.lifeMax * 0.01f);
                if (percentDamage < 1)
                    percentDamage = 1;

                // Terraria regen is per second * 2
                npc.lifeRegen -= percentDamage * 2;

                // Ensures the damage actually applies
                if (damage < percentDamage)
                    damage = percentDamage;
            }
        }

        public override void ResetEffects(NPC npc)
        {
            // FIXED: check for BleedDebuff, not DecayDebuff
            if (!npc.HasBuff(ModContent.BuffType<BleedDebuff>()))
            {
                hasBleed = false;
            }
        }

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
            if (hasBleed)
            {
                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(
                        npc.position,
                        npc.width,
                        npc.height,
                        DustID.Blood,
                        0f,
                        1.5f,
                        100,
                        default,
                        1.1f
                    );

                    Main.dust[dust].velocity *= 0.3f;
                    Main.dust[dust].noGravity = false;
                }

                drawColor = Color.Lerp(drawColor, new Color(200, 40, 40), 0.45f);
            }
        }
    }
}
