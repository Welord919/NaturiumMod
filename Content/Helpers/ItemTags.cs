using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.Rares;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.PostHardmode.Weapons;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
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
            //AddTag(ModContent.ProjectileType<ApophisProj>(), "Apophis");
            //AddTag(ModContent.ProjectileType<MillenniumEye>(), "Apophis");


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

            //Dragons
            AddTags(ModContent.ItemType<BEWD>(), "Card", "Dragon");
            AddTags(ModContent.ItemType<PetiteDragon>(), "Card", "Dragon");
            AddTags(ModContent.ItemType<REBD>(), "Card", "Dragon");
            AddTags(ModContent.ItemType<CurseofDragon>(), "Card", "Dragon");
            AddTags(ModContent.ItemType<TriHornedDragon>(), "Card", "Dragon");

            //Warriors
            AddTags(ModContent.ItemType<CelticGuardian>(), "Card", "Warrior");
            AddTags(ModContent.ItemType<Gaia>(), "Card", "Warrior");
            AddTags(ModContent.ItemType<Masaki>(), "Card", "Warrior");

            //Bugs
            AddTags(ModContent.ItemType<ManEaterBug>(), "Card", "Bug");

            //Beasts
            AddTags(ModContent.ItemType<SilverFang>(), "Card", "Beast");

            //Spellcasters
            AddTags(ModContent.ItemType<DarkMagician>(), "Card", "Spellcaster");
            AddTags(ModContent.ItemType<LeftLeg>(), "Card", "Spellcaster");
            AddTags(ModContent.ItemType<FlameManipulator>(), "Card", "Spellcaster", "Fire");
            AddTags(ModContent.ItemType<AquaMador>(), "Card", "Spellcaster", "Fire");

            //Plants
            AddTags(ModContent.ItemType<Firegrass>(), "Card", "Plant", "Fire");

            //Fairy
            AddTags(ModContent.ItemType<PetiteAngel>(), "Card", "Fairy");

            //Fusion 
            AddTags(ModContent.ItemType<FlameSwordsman>(), "Card", "Warrior", "Fire", "Fusion");
            AddTags(ModContent.ItemType<DarkfireDragon>(), "Card", "Warrior", "Fire", "Fusion");

            //Spells
            AddTags(ModContent.ItemType<Swords>(), "Card", "Spell");
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
            if (source is EntitySource_ItemUse itemSource)
            {
                int itemType = itemSource.Item.type;

                if (WeaponTag.ItemTags.TryGetValue(itemType, out var tags))
                {
                    if (!ProjTags.ContainsKey(projectile.type))
                        ProjTags[projectile.type] = new HashSet<string>();

                    foreach (var t in tags)
                        ProjTags[projectile.type].Add(t);
                }
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
        public override void PostUpdateEquips()
        {
            //Barkion boosts
            if (activeBoosts.TryGetValue("Barkion", out bool barkionActive) && barkionActive)
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 1f;
                Player.GetKnockback(DamageClass.Melee) += 0.1f;
                Player.GetCritChance(DamageClass.Ranged) += 5;
                Player.manaRegenBonus += 2;
                Player.GetDamage(DamageClass.Summon) += 0.1f;
            }
            //Leodrake boosts
            if (activeBoosts.TryGetValue("Leodrake", out bool leodrakeActive) && leodrakeActive)
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 2f;
                Player.GetAttackSpeed(DamageClass.Melee) += 0.10f;
                Player.manaCost -= 0.25f;
            }
            //Exterio boosts
            if (activeBoosts.TryGetValue("Exterio", out bool exterioActive) && exterioActive)
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 3f;
                Player.GetDamage(DamageClass.Generic) += 0.15f;
            }
            //Apophis boosts
            if (activeBoosts.TryGetValue("Apophis", out bool apophisActive) && apophisActive)
            {
                Player.GetDamage(DamageClass.Generic) += 0.10f;
            }
            //Nibiru boosts
            if (activeBoosts.TryGetValue("Nibiru", out bool nibiruActive) && nibiruActive)
            {
                Player.GetArmorPenetration(DamageClass.Generic) += 3f;
                Player.GetCritChance(DamageClass.Ranged) += 10;
                Player.GetDamage(DamageClass.Magic) += 0.1f;
                Player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.1f;
            }
            //Dragon boosts
            if (activeBoosts.TryGetValue("Dragon", out bool dragonActive) && dragonActive)
            {

            }
            //Warrior boosts
            if (activeBoosts.TryGetValue("Warrior", out bool warriorActive) && warriorActive)
            {
            }
        }

    }

}
