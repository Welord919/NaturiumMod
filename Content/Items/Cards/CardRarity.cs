using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.NoEffect;
using NaturiumMod.Content.Items.Cards.LOB.Rares;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Cards.CardRarityHelper;

namespace NaturiumMod.Content.Items.Cards
{
    public static class CardRarityHelper
    {
        public enum Rarity
        {
            Common,
            Rare,
            ShortPrint,
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

                case Rarity.ShortPrint:
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
    public class CardRarityLoader : ModSystem
    {
        public override void PostSetupContent()
        {
            // Commons
            CardRarityRegistry.Register(ModContent.ItemType<Firegrass>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<AquaMador>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<CelticGuardian>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<SilverFang>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<FlameManipulator>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<Armaill>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<DarkworldThorns>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<Dissolverock>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<Hinotama>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<LesserDragon>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<MonsterEgg>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<OneEyedSD>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<SkullServant>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<SteelOgre>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<MWarrior1>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<MWarrior2>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<MysticalSheep2>(), Rarity.Common);
            CardRarityRegistry.Register(ModContent.ItemType<GiantSoldier>(), Rarity.Common);


            // Short Prints
            CardRarityRegistry.Register(ModContent.ItemType<PetiteDragon>(), Rarity.ShortPrint);
            CardRarityRegistry.Register(ModContent.ItemType<PetiteAngel>(), Rarity.ShortPrint);

            // Rares
            CardRarityRegistry.Register(ModContent.ItemType<Swords>(), Rarity.Rare);
            CardRarityRegistry.Register(ModContent.ItemType<ManEaterBug>(), Rarity.Rare);
            CardRarityRegistry.Register(ModContent.ItemType<Masaki>(), Rarity.Rare);
            CardRarityRegistry.Register(ModContent.ItemType<CurseofDragon>(), Rarity.Rare);

            // Super Rares
            CardRarityRegistry.Register(ModContent.ItemType<TriHornedDragon>(), Rarity.SuperRare);
            CardRarityRegistry.Register(ModContent.ItemType<Gaia>(), Rarity.SuperRare);
            CardRarityRegistry.Register(ModContent.ItemType<LeftLeg>(), Rarity.SuperRare);
            CardRarityRegistry.Register(ModContent.ItemType<RightLeg>(), Rarity.SuperRare);
            CardRarityRegistry.Register(ModContent.ItemType<LeftArm>(), Rarity.SuperRare);
            CardRarityRegistry.Register(ModContent.ItemType<RightArm>(), Rarity.SuperRare);
            CardRarityRegistry.Register(ModContent.ItemType<Exodia>(), Rarity.SuperRare);

            // Ultra Rares
            CardRarityRegistry.Register(ModContent.ItemType<BEWD>(), Rarity.UltraRare);
            CardRarityRegistry.Register(ModContent.ItemType<REBD>(), Rarity.UltraRare);
            CardRarityRegistry.Register(ModContent.ItemType<DarkMagician>(), Rarity.UltraRare);
        }
    }
    // ---------------------------
    //  RARITY REGISTRY
    // ---------------------------
    public static class CardRarityRegistry
    {
        public static Dictionary<int, Rarity> CardRarities = new();

        public static void Register(int itemType, Rarity rarity)
        {
            CardRarities[itemType] = rarity;
        }
    }
}