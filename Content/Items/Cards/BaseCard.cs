using NaturiumMod.Content.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Cards.CardRarityHelper;

namespace NaturiumMod.Content.Items.Cards
{
    public abstract class BaseCardCommon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/StimPack";
        public virtual string[] CardTags => new[] { "Card" };
        public virtual Rarity CardRarity => Rarity.Common;

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
            Item.useTurn = true;
            Item.autoReuse = false;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ItemRarityID.Blue;
            Item.value = 200;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
        }

        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()) && Item.consumable)
                return false;
            return base.CanUseItem(player);
        }

        public override bool? UseItem(Player player)
        {
            OnCardUse(player);
            return true;
        }

        protected virtual void OnCardUse(Player player) { }

        public override bool AltFunctionUse(Player player) => false;
    }


    public abstract class BaseCardRare : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/StimPack";
        public virtual string[] CardTags => new[] { "Card" };
        public virtual Rarity CardRarity => Rarity.Rare;
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
            Item.rare = ItemRarityID.Green;
            Item.value = 500;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
            {
                return false;
            }
            return base.CanUseItem(player);
        }
        public override bool? UseItem(Player player)
        {
            OnCardUse(player);
            return true;
        }
        protected virtual void OnCardUse(Player player)
        {
        }
        public override bool AltFunctionUse(Player player) => false;
    }
    public abstract class BaseCardSuper : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/StimPack";
        public virtual string[] CardTags => new[] { "Card" };
        public virtual Rarity CardRarity => Rarity.SuperRare;
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
            Item.rare = ItemRarityID.Orange;
            Item.value = 1000;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
            {
                return false;
            }
            return base.CanUseItem(player);
        }
        public override bool? UseItem(Player player)
        {
            OnCardUse(player);
            return true;
        }
        protected virtual void OnCardUse(Player player)
        {
        }
        public override bool AltFunctionUse(Player player) => false;
    }
    public abstract class BaseCardUltra : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/StimPack";
        public virtual string[] CardTags => new[] { "Card" };
        public virtual Rarity CardRarity => Rarity.UltraRare;
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
            Item.rare = ItemRarityID.LightRed;
            Item.value = 2000;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
            {
                return false;
            }
            return base.CanUseItem(player);
        }
        public override bool? UseItem(Player player)
        {
            OnCardUse(player);
            return true;
        }
        protected virtual void OnCardUse(Player player)
        {
        }
        public override bool AltFunctionUse(Player player) => false;
    }
    public abstract class BaseCardFusion : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/StimPack";
        public virtual string[] CardTags => new[] { "Card", "Fusion" };
        public virtual Rarity CardRarity => Rarity.Fusion;
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
            Item.rare = ItemRarityID.LightPurple;
            Item.value = 5000;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
            {
                return false;
            }
            return base.CanUseItem(player);
        }
        public override bool? UseItem(Player player)
        {
            OnCardUse(player);
            return true;
        }
        protected virtual void OnCardUse(Player player)
        {
        }
        public override bool AltFunctionUse(Player player) => false;
    }
}
