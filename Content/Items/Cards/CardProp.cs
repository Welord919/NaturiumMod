using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards
{
    // -----------------------------
    //  RARITY ENUM
    // -----------------------------
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

    // -----------------------------
    //  CARD INTERFACE
    // -----------------------------
    public interface ICard
    {
        Rarity CardRarity { get; }
        string CardCategory { get; }
        string CardSubtype { get; }

        // NEW: multi-attribute support
        IEnumerable<string> CardAttributes { get; }

        // OLD: single attribute fallback
        string CardAttribute { get; }
    }

    // -----------------------------
    //  CARD ENTRY (AUTO‑GENERATED)
    // -----------------------------
    public class CardEntry
    {
        public int ItemType;
        public Rarity Rarity;
        public string Category;
        public string Subtype;

        // FIX: rename to avoid collision with SDKManifest.Attributes
        public List<string> CardAttributesList;

        public CardEntry(int type, string category, string subtype, IEnumerable<string> attributes)
        {
            ItemType = type;

            ModItem item = ModContent.GetModItem(type);

            if (item is ICard card)
            {
                Rarity = card.CardRarity;
                Category = card.CardCategory;
                Subtype = card.CardSubtype;

                var attrs = card.CardAttributes?.Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

                if (attrs == null || attrs.Count == 0)
                {
                    if (!string.IsNullOrWhiteSpace(card.CardAttribute))
                        attrs = new List<string> { card.CardAttribute };
                }

                CardAttributesList = attrs ?? new List<string>();
            }
            else
            {
                Rarity = Rarity.Common;
                Category = category;
                Subtype = subtype;
                CardAttributesList = attributes.ToList();
            }
        }
    }

    // -----------------------------
    //  CARD POOLS (AUTO‑POPULATED)
    // -----------------------------
    public static class CardPools
    {
        public static readonly List<CardEntry> AllCards = new();

        private static Dictionary<int, CardEntry> _cache;

        private static void EnsureCache()
        {
            if (_cache == null)
                _cache = AllCards.ToDictionary(e => e.ItemType, e => e);
        }

        public static bool TryGetEntry(int itemType, out CardEntry entry)
        {
            EnsureCache();
            return _cache.TryGetValue(itemType, out entry);
        }

        public static IReadOnlyList<int> GetPool(Rarity rarity)
        {
            return AllCards.Where(e => e.Rarity == rarity).Select(e => e.ItemType).ToArray();
        }
    }

    // -----------------------------
    //  RARITY REGISTRY
    // -----------------------------
    public static class CardRarityRegistry
    {
        public static readonly Dictionary<int, Rarity> CardRarities = new();

        public static void Register(int itemType, Rarity rarity)
        {
            CardRarities[itemType] = rarity;
        }
    }

    // -----------------------------
    //  LOADER (RUNS AFTER ALL CARDS REGISTER)
    // -----------------------------
    public class CardRarityLoader : ModSystem
    {
        public override void PostSetupContent()
        {
            foreach (var entry in CardPools.AllCards)
                CardRarityRegistry.Register(entry.ItemType, entry.Rarity);
        }
    }

    //Annoucements and effects when pulling cards of different rarities

    public static class CardRarityHelper
    {
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
                case Rarity.Exodia:
                case Rarity.SuperShortPrint:
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
    public static class CardUtils
    {
        public static bool TryApplyMonsterReborn(Player player, int cardType)
        {
            int rebornBuff = ModContent.BuffType<RebornBuff>();

            if (player.HasBuff(rebornBuff))
            {
                player.ClearBuff(rebornBuff);

                // Consume Monster Reborn instead of the card
                for (int i = 0; i < player.inventory.Length; i++)
                {
                    if (player.inventory[i].type == ModContent.ItemType<MonsterReborn>())
                    {
                        player.inventory[i].stack--;
                        if (player.inventory[i].stack <= 0)
                            player.inventory[i].TurnToAir();
                        break;
                    }
                }

                return true; // protected
            }

            return false; // NOT protected → consume the card
        }
    }
}
