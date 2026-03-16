using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards
{
    public static class CardRarityHelper
    {
        public enum Rarity
        {
            Common,
            Rare,
            CommonSP,
            SuperRare,
            UltraRare
        }

        public static void AnnounceCard(Player player, string cardName, Rarity rarity)
        {
            Color color = Color.White;
            SoundStyle sound = SoundID.MenuTick;

            switch (rarity)
            {
                case Rarity.Common:
                    color = Color.Gray;
                    break;

                case Rarity.Rare:
                    color = Color.LightBlue;
                    sound = SoundID.Unlock;
                    break;

                case Rarity.CommonSP:
                    color = Color.LightGray;
                    sound = SoundID.Item9;
                    break;

                case Rarity.SuperRare:
                    color = Color.Orange;
                    sound = SoundID.Item20;
                    PlaySuperRareEffect(player);
                    break;

                case Rarity.UltraRare:
                    color = Color.IndianRed;
                    sound = SoundID.ResearchComplete;
                    PlayUltraRareEffect(player);
                    break;
            }

            Main.NewText($"You pulled: {cardName} ({rarity})!", color);
            SoundEngine.PlaySound(sound, player.Center);
        }
        private static void PlaySuperRareEffect(Player player)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust.NewDust(
                    player.Center,
                    10, 10,
                    DustID.GemTopaz,
                    Main.rand.NextFloat(-2, 2),
                    Main.rand.NextFloat(-2, 2),
                    150,
                    Color.Orange,
                    1.5f
                );
            }

            // Floating text
            CombatText.NewText(player.getRect(), Color.Orange, "Nice!", true);
        }
        private static void PlayUltraRareEffect(Player player)
        {
            for (int i = 0; i < 40; i++)
            {
                Dust.NewDust(
                    player.Center,
                    20, 20,
                    DustID.GemDiamond,
                    Main.rand.NextFloat(-2, 2),
                    Main.rand.NextFloat(-2, 2),
                    200,
                    Color.IndianRed,
                    1.5f
                );
            }
            CombatText.NewText(player.getRect(), Color.IndianRed, "Amazing!", true);
        }
    }
}