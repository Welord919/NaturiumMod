using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.ShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.Rares;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.PostHardmode.Weapons;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using NaturiumMod.Content.Items.PreHardmode.MillenniumItems;
using NaturiumMod.Content.Items.PreHardmode.Tools;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.Cards.NPCDrop;

namespace NaturiumMod.Content.Helpers
{
    public class ItemTags : ModSystem
    {
        public override void PostSetupContent()
        {
            //Apophis weapons
            AddTag(ModContent.ItemType<ApophisSword>(), "Apophis");
            AddTag(ModContent.ItemType<DecayBow>(), "Apophis");
            AddTag(ModContent.ItemType<JudgementofAnubis>(), "Apophis");
            AddTag(ModContent.ItemType<MillenniumRod>(), "Apophis");

            //Barkion weapons
            AddTag(ModContent.ItemType<BarkionsBlaster>(), "Barkion");
            AddTag(ModContent.ItemType<BarkionsSS>(), "Barkion");
            AddTag(ModContent.ItemType<BarkionsTB>(), "Barkion");
            AddTag(ModContent.ItemType<RoseWhip>(), "Barkion");
            AddTag(ModContent.ItemType<CosmoItem>(), "Barkion");

            //Leodrake weapons
            AddTag(ModContent.ItemType<LeodrakesLeafstorm>(), "Leodrake");
            AddTag(ModContent.ItemType<LeodrakesYoyo>(), "Leodrake");

            //Exterios weapons
            AddTag(ModContent.ItemType<ExteriosCannon>(), "Exterio");
            AddTag(ModContent.ItemType<ExteriosWhip>(), "Exterio");

            //Nibiru weapons
            AddTag(ModContent.ItemType<NibiruSepter>(), "Nibiru");
            AddTag(ModContent.ItemType<StarsteelStarburst>(), "Nibiru");
            AddTag(ModContent.ItemType<StarsteelPickaxe>(), "Nibiru");
            AddTag(ModContent.ItemType<StarryNight>(), "Nibiru");

            //Card Packs
            AddTags(ModContent.ItemType<PackLOB_Common>(), "Card");
            AddTags(ModContent.ItemType<PackLOB_Rare>(), "Card");
            AddTags(ModContent.ItemType<PackLOB_Super>(), "Card");
            AddTags(ModContent.ItemType<PackLOB_Ultra>(), "Card");

            //Dragons
            AddTags(ModContent.ItemType<BEWD>(), "Card", "Dragon", "Light");
            AddTags(ModContent.ItemType<PetiteDragon>(), "Card", "Dragon", "Wind");
            AddTags(ModContent.ItemType<REBD>(), "Card", "Dragon", "Dark", "Fire");
            AddTags(ModContent.ItemType<CurseofDragon>(), "Card", "Dragon", "Dark");
            AddTags(ModContent.ItemType<TriHornedDragon>(), "Card", "Dragon", "Dark");
            AddTags(ModContent.ItemType<LesserDragon>(), "Card", "Dragon", "Wind");
            AddTags(ModContent.ItemType<OneEyedSD>(), "Card", "Dragon", "Wind");

            //Warriors
            AddTags(ModContent.ItemType<CelticGuardian>(), "Card", "Warrior", "Earth");
            AddTags(ModContent.ItemType<Gaia>(), "Card", "Warrior", "Earth");
            AddTags(ModContent.ItemType<Masaki>(), "Card", "Warrior", "Earth");
            AddTags(ModContent.ItemType<MonsterEgg>(), "Card", "Warrior", "Earth");
            AddTags(ModContent.ItemType<MWarrior1>(), "Card", "Warrior", "Earth");
            AddTags(ModContent.ItemType<MWarrior2>(), "Card", "Warrior", "Earth");
            AddTags(ModContent.ItemType<FlameSwordsman>(), "Card", "Warrior", "Fire", "Fusion");
            AddTags(ModContent.ItemType<DarkfireDragon>(), "Card", "Warrior", "Dark","Fire", "Fusion");
            AddTags(ModContent.ItemType<Armaill>(), "Card", "Warrior", "Earth");

            //Bugs
            AddTags(ModContent.ItemType<ManEaterBug>(), "Card", "Bug", "Earth");

            //Beasts
            AddTags(ModContent.ItemType<SilverFang>(), "Card", "Beast", "Earth");
            AddTags(ModContent.ItemType<MysticalSheep2>(), "Card", "Beast", "Earth");

            //Spellcasters
            AddTags(ModContent.ItemType<DarkMagician>(), "Card", "Spellcaster", "Dark");
            AddTags(ModContent.ItemType<FlameManipulator>(), "Card", "Spellcaster", "Fire");
            AddTags(ModContent.ItemType<AquaMador>(), "Card", "Spellcaster", "Water");
            AddTags(ModContent.ItemType<LeftLeg>(), "Card", "Spellcaster", "Dark");
            AddTags(ModContent.ItemType<LeftArm>(), "Card", "Spellcaster", "Dark");
            AddTags(ModContent.ItemType<RightLeg>(), "Card", "Spellcaster", "Dark");
            AddTags(ModContent.ItemType<RightArm>(), "Card", "Spellcaster", "Dark");
            AddTags(ModContent.ItemType<Exodia>(), "Card", "Spellcaster", "Dark");

            //Plants
            AddTags(ModContent.ItemType<DarkworldThorns>(), "Card", "Plant", "Dark");
            AddTags(ModContent.ItemType<Firegrass>(), "Card", "Plant", "Fire");

            //Rocks
            AddTags(ModContent.ItemType<Dissolverock>(), "Card", "Rock", "Earth");
            AddTags(ModContent.ItemType<GiantSoldier>(), "Card", "Rock", "Earth");

            //Zombies
            AddTags(ModContent.ItemType<SkullServant>(), "Card", "Zombie", "Dark");
            AddTags(ModContent.ItemType<PlaguespreaderCard>(), "Card", "Zombie", "Dark");

            //Machines
            AddTags(ModContent.ItemType<SteelOgre>(), "Card", "Machine", "Earth");

            //Fairy
            AddTags(ModContent.ItemType<PetiteAngel>(), "Card", "Fairy", "Light");

            //Pyro 
            AddTags(ModContent.ItemType<Hinotama>(), "Card", "Pyro", "Fire");

            //Fusion 
            AddTags(ModContent.ItemType<Charubin>(), "Card", "Warrior", "Fire", "Fusion");
            AddTags(ModContent.ItemType<Dragoness>(), "Card", "Warrior", "Earth", "Fusion");
            AddTags(ModContent.ItemType<FlameGhost>(), "Card", "Zombie", "Dark", "Fusion");
            AddTags(ModContent.ItemType<FlowerWolf>(), "Card", "Beast", "Earth", "Fusion");
            AddTags(ModContent.ItemType<Fusionist>(), "Card", "Beast", "Wind", "Fusion");
            AddTags(ModContent.ItemType<GaiaChampion>(), "Card", "Dragon", "Wind", "Fusion");
            AddTags(ModContent.ItemType<Karbonala>(), "Card", "Warrior", "Earth", "Fusion");
            AddTags(ModContent.ItemType<MetalDragon>(), "Card", "Machine", "Wind", "Fusion");

            //Spells
            AddTags(ModContent.ItemType<Swords>(), "Card", "Spell");
            AddTags(ModContent.ItemType<Polymerization>(), "Card", "Spell");
        }

        private void AddTag(int itemType, string tag)
        {
            var item = ContentSamples.ItemsByType[itemType];
            if (!WeaponTag.ItemTags.ContainsKey(itemType))
                WeaponTag.ItemTags[itemType] = new HashSet<string>();

            WeaponTag.ItemTags[itemType].Add(tag);

        }
        private void AddTags(int itemType, params string[] tags)
        {
            foreach (var tag in tags)
                AddTag(itemType, tag);
        }

    }
    public static class CardTagHelper
    {
        private static readonly HashSet<string> EssenceAttributes = new()
        {
            "Fire", "Water", "Earth", "Wind", "Light", "Dark"
        };

        public static string GetCardAttribute(int itemType)
        {
            if (!WeaponTag.ItemTags.TryGetValue(itemType, out var tags))
                return null;


            foreach (var tag in tags)
            {
                if (EssenceAttributes.Contains(tag))
                    return tag;
            }

            return null;
        }
    }
    public class WeaponTag : GlobalItem
    {
        // Static registry: itemType → set of tags
        public static Dictionary<int, HashSet<string>> ItemTags = new();

        public override bool InstancePerEntity => true;

        public HashSet<string> GetTags(int type)
        {
            if (ItemTags.TryGetValue(type, out var set))
                return set;

            return new HashSet<string>();
        }
    }
    public class WeaponTagProj : GlobalProjectile
    {
        // Static registry: projType → set of tags
        public static Dictionary<int, HashSet<string>> ProjTags = new();

        public override bool InstancePerEntity => true;

        public HashSet<string> GetTags(int type)
        {
            if (ProjTags.TryGetValue(type, out var set))
                return set;

            return new HashSet<string>();
        }
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            int itemType = -1;

            // Covers most modded + vanilla weapons
            if (source is EntitySource_ItemUse itemSource)
                itemType = itemSource.Item.type;

            // Covers guns, bows, flamethrowers, etc.
            else if (source is EntitySource_ItemUse_WithAmmo ammoSource)
                itemType = ammoSource.Item.type;

            if (itemType == -1)
                return;

            // Copy tags from item → projectile
            if (WeaponTag.ItemTags.TryGetValue(itemType, out var tags))
            {
                if (!ProjTags.ContainsKey(projectile.type))
                    ProjTags[projectile.type] = new HashSet<string>();

                foreach (var t in tags)
                    ProjTags[projectile.type].Add(t);
            }
        }
    }
    public class WeaponBoostPlayer : ModPlayer
    {
        // Dictionary of active boosts
        public Dictionary<string, bool> activeBoosts = new();
        public override void ResetEffects()
        {
            // Reset all boosts to false each tick
            foreach (var key in activeBoosts.Keys.ToList())
                activeBoosts[key] = false;
        }
        private bool PlayerHoldingTag(string tag)
        {
            Item held = Player.HeldItem;

            if (held == null || held.IsAir)
                return false;

            if (WeaponTag.ItemTags.TryGetValue(held.type, out var tags))
                return tags.Contains(tag);

            return false;
        }

        public override void PostUpdateEquips()
        {
            // Barkion boosts
            if (activeBoosts.TryGetValue("Barkion", out bool barkionActive)
                && barkionActive
                && PlayerHoldingTag("Barkion"))
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 1f;
                Player.GetKnockback(DamageClass.Melee) += 0.1f;
                Player.GetCritChance(DamageClass.Ranged) += 5;
                Player.manaRegenBonus += 2;
                Player.GetDamage(DamageClass.Summon) += 0.1f;
            }

            // Leodrake boosts
            if (activeBoosts.TryGetValue("Leodrake", out bool leodrakeActive)
                && leodrakeActive
                && PlayerHoldingTag("Leodrake"))
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 2f;
                Player.GetAttackSpeed(DamageClass.Melee) += 0.10f;
                Player.manaCost -= 0.25f;
            }

            // Exterio boosts
            if (activeBoosts.TryGetValue("Exterio", out bool exterioActive)
                && exterioActive
                && PlayerHoldingTag("Exterio"))
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 3f;
                Player.GetDamage(DamageClass.Generic) += 0.15f;
            }

            // Apophis boosts
            if (activeBoosts.TryGetValue("Apophis", out bool apophisActive)
                && apophisActive
                && PlayerHoldingTag("Apophis"))
            {
                Player.GetDamage(DamageClass.Generic) += 0.10f;
            }

            // Nibiru boosts
            if (activeBoosts.TryGetValue("Nibiru", out bool nibiruActive)
                && nibiruActive
                && PlayerHoldingTag("Nibiru"))
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 3f;
                Player.GetCritChance(DamageClass.Ranged) += 10;
                Player.GetDamage(DamageClass.Magic) += 0.1f;
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.1f;
            }

            // Fusionist boost
            if (activeBoosts.TryGetValue("Fusion", out bool fusionActive)
                && fusionActive
                && PlayerHoldingTag("Fusion"))
            {
                Player.GetDamage(ModContent.GetInstance<CardDamage>()) *= 1.10f;

            }
        }
    }
}
