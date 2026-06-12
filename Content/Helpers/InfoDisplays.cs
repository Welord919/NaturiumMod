using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Accessories.CraftingTrees;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.NPCs.TownNPCS;
using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Helpers
{
    public class MinionInfoDisplay : InfoDisplay
    {
        public override string Texture => "NaturiumMod/Assets/UI/ExampleInfoDisplay";

        public override bool Active()
        {
            Player player = Main.LocalPlayer;

            // Only show if the accessory is equipped
            return player.GetModPlayer<MinionInfoPlayer>().minionDisplayEquipped;
        }
        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            Player player = Main.LocalPlayer;

            int current = (int)player.slotsMinions;
            int max = player.maxMinions;

            displayColor = Color.White;

            return $"{current}/{max} Minions";
        }
    }
    public class MinionInfoPlayer : ModPlayer
    {
        public bool minionDisplayEquipped;

        public override void ResetEffects()
        {
            minionDisplayEquipped = false;
        }
    }
    // ============================================================
    // INFODISPLAY #1 — CARD BOOST + LUCK
    // ============================================================
    public class MillenniumScarabInfo_CardBoost : InfoDisplay
    {
        public override string Texture => "NaturiumMod/Assets/UI/ExampleInfoDisplay";

        public override bool Active()
        {
            return Main.LocalPlayer.GetModPlayer<MillenniumScarabPlayer>().ScarabEquipped;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            Player player = Main.LocalPlayer;
            var mp = player.GetModPlayer<MillenniumScarabPlayer>();

            displayColor = Color.Gold;

            float boost = mp.CardDropBoost * 100f;
            float luck = player.luck * 100f;

            return $"Boost {boost:0.#}% | Luck {luck:0.#}%";
        }
    }

    // ============================================================
    // INFODISPLAY #2 — CARD KILLS + DROP CHANCES
    // ============================================================
    public class MillenniumScarabInfo_CardKills : InfoDisplay
    {
        public override string Texture => "NaturiumMod/Assets/UI/ExampleInfoDisplay";

        public override bool Active()
        {
            return Main.LocalPlayer.GetModPlayer<MillenniumScarabPlayer>().ScarabEquipped;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            Player player = Main.LocalPlayer;
            var mp = player.GetModPlayer<MillenniumScarabPlayer>();

            displayColor = Color.LightPink;

            int kills = CardQuestWorld.totalCardDamageKills;
            string chances = ComputeCardChances(mp.CardDropBoost);

            return $"Card Kills {kills} | {chances}";
        }

        private string ComputeCardChances(float boost)
        {
            float common = MathHelper.Clamp(PackDropConstants.CommonBase * (1f + boost), 0f, 1f);
            float rare = MathHelper.Clamp(PackDropConstants.RareBase * (1f + boost), 0f, 1f);
            float super = MathHelper.Clamp(PackDropConstants.SuperBase * (1f + boost), 0f, 1f);
            float ultra = MathHelper.Clamp(PackDropConstants.UltraBase * (1f + boost), 0f, 1f);

            return $"{common * 100:0.##}/{rare * 100:0.##}/{super * 100:0.###}/{ultra * 100:0.###}%";
        }
    }


    // ============================================================
    // INFODISPLAY #3 — MINION + SENTRY SLOTS
    // ============================================================
    public class MillenniumScarabInfo_Minions : InfoDisplay
    {
        public override string Texture => "NaturiumMod/Assets/UI/ExampleInfoDisplay";

        public override bool Active()
        {
            return Main.LocalPlayer.GetModPlayer<MillenniumScarabPlayer>().ScarabEquipped;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            Player player = Main.LocalPlayer;

            displayColor = Color.LightGreen;

            int minionCurrent = (int)player.slotsMinions;
            int minionMax = player.maxMinions;

            int sentryCurrent = 0;
            foreach (Projectile proj in Main.projectile)
            {
                if (proj.active && proj.owner == player.whoAmI && proj.sentry)
                    sentryCurrent++;
            }

            int sentryMax = player.maxTurrets;

            return $"{minionCurrent}/{minionMax} Minions | {sentryCurrent}/{sentryMax} Sentries";
        }
    }
}
