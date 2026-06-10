using NaturiumMod.Content.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards
{
    
    public class SummoningSickness : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/SummoningSickness";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = true; // red debuff
        }
    }
    public class SummoningSicknessPlayer : ModPlayer
    {
        public bool hasSummoningSickness;

        public override void ResetEffects()
        {
            hasSummoningSickness = false;
        }

        public override void PostUpdateBuffs()
        {
            if (Player.HasBuff(ModContent.BuffType<SummoningSickness>()))
                hasSummoningSickness = true;
        }

        public override bool CanUseItem(Item item)
        {
            if (WeaponTag.ItemTags.TryGetValue(item.type, out var tags) && tags.Contains("Card"))
            {
                int index = Player.FindBuffIndex(ModContent.BuffType<SummoningSickness>());
                if (index != -1 && Player.buffTime[index] > 0)
                    return false;
            }

            return base.CanUseItem(item);
        }

    }

}
