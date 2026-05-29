using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards
{
    // =========================================================
    //  UNIVERSAL BASE CLASS (handles 95% of all card behavior)
    // =========================================================
    public abstract class BaseCard : ModItem, ICard
    {
        // ---- Metadata ----
        public virtual Rarity CardRarity => Rarity.Common;
        public virtual string CardCategory => "Card";
        public virtual string CardSubtype => "";

        // Single-attribute fallback
        public virtual string CardAttribute => "";

        // Multi-attribute support
        public virtual IEnumerable<string> CardAttributes =>
            string.IsNullOrWhiteSpace(CardAttribute)
            ? new[] { "" }
            : new[] { CardAttribute };

        // ---- Registration ----
        public override void SetStaticDefaults()
        {
            CardPools.AllCards.Add(
                new CardEntry(Type, CardCategory, CardSubtype, CardAttributes)
            );
        }

        // ---- Default card behavior ----
        public override void SetDefaults()
        {
            Item.useTurn = true;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 999;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.DamageType = ModContent.GetInstance<CardDamage>();

            ItemTags.AddTagToItem(Type, "Card");
        }

        public override bool CanUseItem(Player player)
            => !player.HasBuff(ModContent.BuffType<SummoningSickness>());

        public override bool? UseItem(Player player)
        {
            OnCardUse(player);
            return true;
        }

        protected virtual void OnCardUse(Player player) { }

        public override bool AltFunctionUse(Player player) => false;
    }

    // =========================================================
    //  RARITY-SPECIFIC BASE CLASSES (only override rarity + stats)
    // =========================================================

    public abstract class BaseCardCommon : BaseCard
    {
        public override Rarity CardRarity => Rarity.Common;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.White;
            Item.value = 25;
        }
    }

    public abstract class BaseCardRare : BaseCard
    {
        public override Rarity CardRarity => Rarity.Rare;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Green;
            Item.value = 500;
        }
    }
    public abstract class BaseCardShortPrint : BaseCardRare
    {
        public override Rarity CardRarity => Rarity.ShortPrint;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Blue; 
            Item.value = 750;  
        }
    }
    public abstract class BaseCardSuper : BaseCard
    {
        public override Rarity CardRarity => Rarity.SuperRare;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = 1000;
        }
    }
    public abstract class BaseCardSuperShortPrint : BaseCardSuper
    {
        public override Rarity CardRarity => Rarity.SuperShortPrint;

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Orange;
            Item.value = 1500;    
        }
    }
    public abstract class BaseCardUltra : BaseCard
    {
        public override Rarity CardRarity => Rarity.UltraRare;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.LightRed;
            Item.value = 2000;
        }
    }

    public abstract class BaseCardFusion : BaseCard
    {
        public override Rarity CardRarity => Rarity.Fusion;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.LightPurple;
            Item.value = 5000;
            ItemTags.AddTagToItem(Type, "Fusion");
        }
    }

    // =========================================================
    //  SPECIAL CASES
    // =========================================================
    public abstract class BaseCardCrafted : BaseCard
    {
        public override Rarity CardRarity => Rarity.Crafted;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Cyan;
            Item.value = 1500;
        }
    }
    public abstract class MRUltra : BaseCardUltra
    {
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.consumable = false;
        }

        protected bool TryApplyMonsterReborn(Player player)
        {
            int rebornBuff = ModContent.BuffType<RebornBuff>();

            if (player.HasBuff(rebornBuff))
            {
                player.ClearBuff(rebornBuff);

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

                return true;
            }

            return false;
        }

        protected void ConsumeCard(Player player, int amount)
        {
            for (int i = 0; i < player.inventory.Length && amount > 0; i++)
            {
                if (player.inventory[i].type == Type)
                {
                    int take = Math.Min(player.inventory[i].stack, amount);
                    player.inventory[i].stack -= take;
                    amount -= take;

                    if (player.inventory[i].stack <= 0)
                        player.inventory[i].TurnToAir();
                }
            }
        }
    }

    // No-effect cards
    public abstract class NoEffectCommon : BaseCardCommon
    {
        public override string CardAttribute => ""; // single
        public override IEnumerable<string> CardAttributes => new[] { "" };
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = 25;
            // No use behavior
            Item.useStyle = ItemUseStyleID.None;
            Item.useTime = 0;
            Item.useAnimation = 0;
            Item.noUseGraphic = false;
            Item.consumable = false;
        }
    }

    public abstract class ExodiaPiece : BaseCard
    {
        public override Rarity CardRarity => Rarity.Exodia;
        public override string CardSubtype => "Spellcaster";
        public override IEnumerable<string> CardAttributes => new[] { "Dark" };
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Yellow;
            Item.value = 1000;
            // No use behavior
            Item.useStyle = ItemUseStyleID.None;
            Item.useTime = 0;
            Item.useAnimation = 0;
            Item.noUseGraphic = false;
            Item.consumable = false;
        }
    }
    public abstract class NoEffectFusionBase : BaseCardFusion
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/NoEffects/LeftLeg";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.value = 1500;
            // No use behavior
            Item.useStyle = ItemUseStyleID.None;
            Item.useTime = 0;
            Item.useAnimation = 0;
            Item.noUseGraphic = false;
            Item.consumable = false;
        }
    }

}
