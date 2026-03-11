using Microsoft.Xna.Framework;
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
        }

        private void AddTag(int itemType, string tag)
        {
            var item = ContentSamples.ItemsByType[itemType];
            if (!WeaponTag.ItemTags.ContainsKey(itemType))
                WeaponTag.ItemTags[itemType] = new HashSet<string>();

            WeaponTag.ItemTags[itemType].Add(tag);

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
        }

    }

}
