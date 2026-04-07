using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class GoblinsSecretRemedy : BaseCardRare
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/GoblinsSecretRemedy";
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.UseSound = SoundID.Item3;
        }

        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(BuffID.PotionSickness)) return false;
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>())) return false;
            return true;
        }

        public override bool? UseItem(Player player)
        {
            player.statLife += 75;
            if (player.statLife > player.statLifeMax2)
                player.statLife = player.statLifeMax2;

            player.HealEffect(75, true);
            player.AddBuff(BuffID.PotionSickness, 30 * 60);
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 60 * 10);

            return true;
        }
    }
}
