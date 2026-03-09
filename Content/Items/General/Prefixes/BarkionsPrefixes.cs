using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.General.Prefixes
{
    // This class serves as an example for declaring item 'prefixes', or 'modifiers' in other words.
    public class BarkionsBlessing : ModPrefix
    {
        public virtual float Power => 1f;
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;
        public override float RollChance(Item item)
        {
            return 4f;
        }
        // Determines if it can roll at all.
        // Use this to control if a prefix can be rolled or not.
        public override bool CanRoll(Item item)
        {
            return true;
        }

        // Use this function to modify these stats for items which have this prefix:
        // Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            knockbackMult *= 1f + 0.05f * Power;
            scaleMult *= 1f + 0.05f * Power;
            critBonus += (int)(5f * Power);
        }
    }
    public class BarkionsCurse : ModPrefix
    {
        public virtual float Power => 1f;
        public override PrefixCategory Category => PrefixCategory.AnyWeapon;
        public override float RollChance(Item item)
        {
            return 5f;
        }
        // Determines if it can roll at all.
        // Use this to control if a prefix can be rolled or not.
        public override bool CanRoll(Item item)
        {
            return true;
        }

        // Use this function to modify these stats for items which have this prefix:
        // Damage Multiplier, Knockback Multiplier, Use Time Multiplier, Scale Multiplier (Size), Shoot Speed Multiplier, Mana Multiplier (Mana cost), Crit Bonus.
        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            knockbackMult *= 1f - 0.05f * Power;
            scaleMult *= 1f - 0.05f * Power;
            critBonus -= (int)(5f * Power);
        }
    }
}