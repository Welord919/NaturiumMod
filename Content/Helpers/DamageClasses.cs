using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.Localization;


namespace NaturiumMod.Content.Helpers
{
    public class CardDamage : DamageClass
    {
        public override void SetStaticDefaults()
        {
          
        }

        public override StatInheritanceData GetModifierInheritance(DamageClass damageClass)
        {
            // Cards inherit generic damage bonuses
            if (damageClass == Generic)
                return StatInheritanceData.Full;

            return StatInheritanceData.None;
        }

        public override bool GetEffectInheritance(DamageClass damageClass)
        {
            // Cards inherit generic crit, knockback, armor pen, etc.
            return damageClass == Generic;
        }
    }

}
