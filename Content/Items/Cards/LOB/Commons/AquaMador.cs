using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class AquaMador : BaseCardCommon
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/aquamador";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = ModContent.BuffType<AquaMadorBuff>();
            Item.buffTime = 60 * 30;
        }
        protected override void OnCardUse(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
            player.AddBuff(ModContent.BuffType<AquaMadorBuff>(), 60 * 30);
            player.AddBuff(BuffID.Gills, 60 * 30);

            var mp = player.GetModPlayer<AquaMadorPlayer>();
            mp.currentOverHealth = 50;
        }
    }
    public class AquaMadorBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/AquaMadorBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var mp = player.GetModPlayer<AquaMadorPlayer>();
            mp.aquaMadorActive = true;
        }
    }

    public class AquaMadorPlayer : ModPlayer
    {
        public bool aquaMadorActive;
        public int currentOverHealth; // 0–50
        private int decayTimer;

        public override void ResetEffects()
        {
            aquaMadorActive = false;
        }

        public override void UpdateDead()
        {
            currentOverHealth = 0;
        }
        public override void PostUpdate()
        {
            if (Player.wet && !Player.honeyWet && !Player.lavaWet)
            {
                // Enable full merfolk behavior manually (1.4.3 style)
                Player.merman = true;        // transformation visuals
                Player.ignoreWater = true;   // swim freely
                Player.gills = true;         // breathe underwater
            }
        }


        public override void PostUpdateMiscEffects()
        {
            if (!aquaMadorActive)
            {
                currentOverHealth = 0;
                return;
            }

            // +5 defense
            Player.statDefense += 5;

            // Overhealth decay: 1 per second
            if (currentOverHealth > 0)
            {
                decayTimer++;
                if (decayTimer >= 60)
                {
                    decayTimer = 0;
                    currentOverHealth--;

                    if (currentOverHealth <= 0)
                    {
                        currentOverHealth = 0;
                        Player.ClearBuff(ModContent.BuffType<AquaMadorBuff>());
                    }
                }
            }
        }

        // ⭐ THIS is the missing piece — inject overhealth into max life
        public override void PostUpdateEquips()
        {
            if (aquaMadorActive && currentOverHealth > 0)
            {
                Player.statLifeMax2 += currentOverHealth;
            }
        }
    }
}