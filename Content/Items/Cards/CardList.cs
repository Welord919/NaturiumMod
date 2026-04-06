using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Crafted;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.Rares;
using NaturiumMod.Content.Items.Cards.LOB.ShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.Cards.NPCDrop;
using NaturiumMod.Content.Items.Cards.PSA;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Cards.CardPools;
using static NaturiumMod.Content.Items.Cards.CardRarityHelper;

namespace NaturiumMod.Content.Items.Cards
{
    // Card List: This class serves as a centralized repository for all card item types, categorized by rarity.
    public static class CardPools
    {
        public static readonly List<CardEntry> AllCards = new()
        {
        // Card packs and PSA case (Cases are handled in PSACase)
        new CardEntry(ModContent.ItemType<PackLOB_Common>(), CardRarityHelper.Rarity.Common, "Card"),
        new CardEntry(ModContent.ItemType<PackLOB_Rare>(), CardRarityHelper.Rarity.Rare, "Card"),
        new CardEntry(ModContent.ItemType<PackLOB_Super>(), CardRarityHelper.Rarity.SuperRare, "Card"),
        new CardEntry(ModContent.ItemType<PackLOB_Ultra>(), CardRarityHelper.Rarity.UltraRare, "Card"),
        new CardEntry(ModContent.ItemType<PSACase>(), CardRarityHelper.Rarity.Common, "Card"),
        // Commons
        new CardEntry(ModContent.ItemType<Firegrass>(), CardRarityHelper.Rarity.Common, "Card", "Plant", "Fire"),
        new CardEntry(ModContent.ItemType<AquaMador>(), CardRarityHelper.Rarity.Common, "Card", "Spellcaster", "Water"),
        new CardEntry(ModContent.ItemType<CelticGuardian>(), CardRarityHelper.Rarity.Common, "Card", "Warrior", "Earth"),
        new CardEntry(ModContent.ItemType<SilverFang>(), CardRarityHelper.Rarity.Common, "Card", "Beast", "Earth"),
        new CardEntry(ModContent.ItemType<FlameManipulator>(), CardRarityHelper.Rarity.Common, "Card", "Spellcaster", "Fire"),
        new CardEntry(ModContent.ItemType<Armaill>(), CardRarityHelper.Rarity.Common, "Card", "Warrior", "Earth"),
        new CardEntry(ModContent.ItemType<DarkworldThorns>(), CardRarityHelper.Rarity.Common, "Card", "Plant", "Dark"),
        new CardEntry(ModContent.ItemType<Dissolverock>(), CardRarityHelper.Rarity.Common, "Card", "Rock", "Earth"),
        new CardEntry(ModContent.ItemType<Hinotama>(), CardRarityHelper.Rarity.Common, "Card", "Pyro", "Fire"),
        new CardEntry(ModContent.ItemType<LesserDragon>(), CardRarityHelper.Rarity.Common, "Card", "Dragon", "Wind"),
        new CardEntry(ModContent.ItemType<MonsterEgg>(), CardRarityHelper.Rarity.Common, "Card", "Warrior", "Earth"),
        new CardEntry(ModContent.ItemType<OneEyedSD>(), CardRarityHelper.Rarity.Common, "Card", "Dragon", "Wind"),
        new CardEntry(ModContent.ItemType<SkullServant>(), CardRarityHelper.Rarity.Common, "Card", "Zombie", "Dark"),
        new CardEntry(ModContent.ItemType<SteelOgre>(), CardRarityHelper.Rarity.Common, "Card", "Machine", "Earth"),
        new CardEntry(ModContent.ItemType<MWarrior1>(), CardRarityHelper.Rarity.Common, "Card", "Warrior", "Earth"),
        new CardEntry(ModContent.ItemType<MWarrior2>(), CardRarityHelper.Rarity.Common, "Card", "Warrior", "Earth"),
        new CardEntry(ModContent.ItemType<MysticalSheep2>(), CardRarityHelper.Rarity.Common, "Card", "Beast", "Earth"),
        new CardEntry(ModContent.ItemType<GiantSoldier>(), CardRarityHelper.Rarity.Common, "Card", "Rock", "Earth"),

        // Rares
        new CardEntry(ModContent.ItemType<Swords>(), CardRarityHelper.Rarity.Rare, "Card", "Spell"),
        new CardEntry(ModContent.ItemType<ManEaterBug>(), CardRarityHelper.Rarity.Rare, "Card", "Bug", "Earth"),
        new CardEntry(ModContent.ItemType<Masaki>(), CardRarityHelper.Rarity.Rare, "Card", "Warrior", "Earth"),
        new CardEntry(ModContent.ItemType<CurseofDragon>(), CardRarityHelper.Rarity.Rare, "Card", "Dragon", "Dark"),

        // Short Prints
        new CardEntry(ModContent.ItemType<PetiteDragon>(), CardRarityHelper.Rarity.ShortPrint, "Card", "Dragon", "Wind"),
        new CardEntry(ModContent.ItemType<PetiteAngel>(), CardRarityHelper.Rarity.ShortPrint, "Card", "Fairy", "Light"),
        new CardEntry(ModContent.ItemType<Polymerization>(), CardRarityHelper.Rarity.ShortPrint, "Card", "Spell", "Fusion"),

        // Super Rares
        new CardEntry(ModContent.ItemType<TriHornedDragon>(), CardRarityHelper.Rarity.SuperRare, "Card", "Dragon", "Dark"),
        new CardEntry(ModContent.ItemType<Gaia>(), CardRarityHelper.Rarity.SuperRare, "Card", "Warrior", "Earth"),
        new CardEntry(ModContent.ItemType<Exodia>(), CardRarityHelper.Rarity.SuperRare, "Card", "Spellcaster", "Dark"),

        // Super Short Prints
        new CardEntry(ModContent.ItemType<PotofGreed>(), CardRarityHelper.Rarity.SuperShortPrint, "Card", "Spell"),
        new CardEntry(ModContent.ItemType<MonsterReborn>(), CardRarityHelper.Rarity.SuperShortPrint, "Card", "Spell"),

        // Exodia Pieces
        new CardEntry(ModContent.ItemType<LeftLeg>(), CardRarityHelper.Rarity.Exodia, "Card", "Spellcaster", "Dark"),
        new CardEntry(ModContent.ItemType<LeftArm>(), CardRarityHelper.Rarity.Exodia, "Card", "Spellcaster", "Dark"),
        new CardEntry(ModContent.ItemType<RightArm>(), CardRarityHelper.Rarity.Exodia, "Card", "Spellcaster", "Dark"),
        new CardEntry(ModContent.ItemType<RightLeg>(), CardRarityHelper.Rarity.Exodia, "Card", "Spellcaster", "Dark"),

        // Ultra Rares
        new CardEntry(ModContent.ItemType<BEWD>(), CardRarityHelper.Rarity.UltraRare, "Card", "Dragon", "Light"),
        new CardEntry(ModContent.ItemType<REBD>(), CardRarityHelper.Rarity.UltraRare, "Card", "Dragon", "Dark", "Fire"),
        new CardEntry(ModContent.ItemType<DarkMagician>(), CardRarityHelper.Rarity.UltraRare, "Card", "Spellcaster", "Dark"),

        // Fusion cards
        new CardEntry(ModContent.ItemType<FlameSwordsman>(), CardRarityHelper.Rarity.Fusion, "Card", "Warrior", "Fire", "Fusion"),
        new CardEntry(ModContent.ItemType<DarkfireDragon>(), CardRarityHelper.Rarity.Fusion, "Card", "Warrior", "Dark", "Fire", "Fusion"),
        new CardEntry(ModContent.ItemType<Charubin>(), CardRarityHelper.Rarity.Fusion, "Card", "Warrior", "Fire", "Fusion"),
        new CardEntry(ModContent.ItemType<Dragoness>(), CardRarityHelper.Rarity.Fusion, "Card", "Warrior", "Earth", "Fusion"),
        new CardEntry(ModContent.ItemType<FlameGhost>(), CardRarityHelper.Rarity.Fusion, "Card", "Zombie", "Dark", "Fusion"),
        new CardEntry(ModContent.ItemType<FlowerWolf>(), CardRarityHelper.Rarity.Fusion, "Card", "Beast", "Earth", "Fusion"),
        new CardEntry(ModContent.ItemType<Fusionist>(), CardRarityHelper.Rarity.Fusion, "Card", "Beast", "Wind", "Fusion"),
        new CardEntry(ModContent.ItemType<GaiaChampion>(), CardRarityHelper.Rarity.Fusion, "Card", "Dragon", "Wind", "Fusion"),
        new CardEntry(ModContent.ItemType<Karbonala>(), CardRarityHelper.Rarity.Fusion, "Card", "Warrior", "Earth", "Fusion"),
        new CardEntry(ModContent.ItemType<MetalDragon>(), CardRarityHelper.Rarity.Fusion, "Card", "Machine", "Wind", "Fusion"),

        // Dropped cards
        new CardEntry(ModContent.ItemType<BalloonLizardCard>(), CardRarityHelper.Rarity.Dropped, "Card", "Reptile", "Earth"),
        new CardEntry(ModContent.ItemType<PlaguespreaderCard>(), CardRarityHelper.Rarity.Dropped, "Card", "Zombie", "Dark"),
        new CardEntry(ModContent.ItemType<SpiritReaperCard>(), CardRarityHelper.Rarity.Dropped, "Card", "Zombie", "Dark"),

        //Crafted cards
        new CardEntry(ModContent.ItemType<StimPack>(), CardRarityHelper.Rarity.Crafted, "Card", "Spell")
        };

        // Fast dictionary for lookups (built on first access)
        private static Dictionary<int, CardEntry> _entryCache;
        private static void EnsureCache()
        {
            if (_entryCache != null) return;
            _entryCache = AllCards.ToDictionary(e => e.ItemType, e => e);
        }

        public static bool TryGetEntry(int itemType, out CardEntry entry)
        {
            EnsureCache();
            return _entryCache.TryGetValue(itemType, out entry);
        }

        public static IReadOnlyList<int> GetPool(Rarity rarity)
        {
            return AllCards.Where(e => e.Rarity == rarity).Select(e => e.ItemType).ToArray();
        }

    }
    public readonly struct CardEntry
    {
        public int ItemType { get; }
        public Rarity Rarity { get; }
        public string[] Tags { get; }

        public CardEntry(int itemType, Rarity rarity, params string[] tags)
        {
            ItemType = itemType;
            Rarity = rarity;
            Tags = tags ?? Array.Empty<string>();
        }
    }

    public static class CardRarityHelper
    {
        public enum Rarity
        {
            Common,
            Rare,
            ShortPrint,
            SuperRare,
            Exodia,
            SuperShortPrint,
            UltraRare,
            Fusion,
            Crafted,
            Dropped
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

                case Rarity.Exodia:
                    color = Color.Orange;
                    sound = SoundID.Item20;
                    PlaySuperRareEffect(player);
                    break;

                case Rarity.SuperShortPrint:
                    color = Color.DarkOrange;
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
            foreach (var entry in CardPools.AllCards)
            {
                CardRarityRegistry.Register(entry.ItemType, entry.Rarity);
            }
        }
    }

    public static class CardTagHelper
    {
        private static readonly HashSet<string> EssenceAttributes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Fire", "Water", "Earth", "Wind", "Light", "Dark", "Spell", "Trap"
    };

        public static string GetCardAttribute(int itemType)
        {
            // 1) Prefer CardPools entries
            if (CardPools.TryGetEntry(itemType, out var entry) && entry.Tags != null && entry.Tags.Length > 0)
            {
                foreach (var tag in entry.Tags)
                    if (EssenceAttributes.Contains(tag))
                        return tag;
            }

            // 2) Fallback to WeaponTag registry
            if (WeaponTag.ItemTags.TryGetValue(itemType, out var tags))
            {
                foreach (var tag in tags)
                    if (EssenceAttributes.Contains(tag))
                        return tag;
            }

            return null;
        }
    }
    public static class ExtractionRegistry
    {
        public static readonly Dictionary<int, (int essenceType, int amount)> ExtractionMap = new();

        public static void RegisterExtraction(int cardType, int essenceType, int amount)
            => ExtractionMap[cardType] = (essenceType, amount);

        public static bool TryGetExtraction(int cardType, out (int essenceType, int amount) value)
            => ExtractionMap.TryGetValue(cardType, out value);
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

