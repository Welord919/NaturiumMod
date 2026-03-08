using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Helpers
{
    public class WeaponTag : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public bool isApophis;
        public bool isDecay;

        public override void SetDefaults(Item item)
        {
            // --- Apophis/Anubis weapons ---
            if (item.type == ModContent.ItemType<ApophisSword>()
             || item.type == ModContent.ItemType<JudgementofAnubis>()
             || item.type == ModContent.ItemType<DecayBow>()
             || item.type == ModContent.ItemType<MillenniumRod>())
            {
                isApophis = true;
            }
        }
    }
    public class WeaponTagProj : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public bool isApophis;
        public bool isDecay;

        public override void SetDefaults(Projectile proj)
        {
            // --- Apophis projectiles ---
            if (proj.type == ModContent.ProjectileType<ApophisProj>()
                || proj.type == ModContent.ProjectileType<DecayArrowProj>()
                || proj.type == ModContent.ProjectileType<MillenniumEye>()
                || proj.type == ModContent.ProjectileType<AnubisSentry>())
            {
                isApophis = true;
            }
        }
    }
}
