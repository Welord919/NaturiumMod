using NaturiumMod.Content.Items.PreHardmode.Accessories;
using NaturiumMod.Content.Items.PreHardmode.IceBarrier;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Helpers
{
    public static class IceWeaponRegistry
    {
        public static readonly HashSet<int> IceItems = new()
        {
            ItemID.IceBlade,
            ItemID.Frostbrand,
            ItemID.IceBoomerang,
            ItemID.FrostStaff,
            ItemID.FlowerofFrost,
            ItemID.FrostDaggerfish,
            ItemID.FrostHelmet,
            ItemID.FrostBreastplate,
            ItemID.FrostLeggings,
            ItemID.FrostCore,
            ItemID.IceBow,
            ItemID.IceSickle,
            ItemID.StaffoftheFrostHydra,
            ItemID.SnowballCannon,
            ItemID.SnowballLauncher,
            ItemID.WandofFrosting,
        };

        public static readonly HashSet<int> IceProjectiles = new()
        {
            ProjectileID.IceBolt,
            ProjectileID.FrostBoltSword,
            ProjectileID.FrostBlastFriendly,
            ProjectileID.FrostDaggerfish,
            ProjectileID.FrostArrow,
            ProjectileID.FrostBeam,
            ProjectileID.IceSickle,
            ProjectileID.FrostHydra,
            ProjectileID.IceSpike,
            ProjectileID.SnowBallFriendly,
            ProjectileID.WandOfFrostingFrost,
        };

        public static void RegisterModdedIceItem(int itemType)
        {
            IceItems.Add(itemType);
        }

        public static void RegisterModdedIceProjectile(int projType)
        {
            IceProjectiles.Add(projType);
        }
    }
    public class ModdedIceWeaponSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            IceWeaponRegistry.RegisterModdedIceItem(ModContent.ItemType<TrishulaWeapon>());
            IceWeaponRegistry.RegisterModdedIceProjectile(ModContent.ProjectileType<TrishulaProj>());
            IceWeaponRegistry.RegisterModdedIceItem(ModContent.ItemType<IceBarrierWhip>());
            IceWeaponRegistry.RegisterModdedIceProjectile(ModContent.ProjectileType<IceBarrierWhipProj>());
            IceWeaponRegistry.RegisterModdedIceItem(ModContent.ItemType<TripleWOF>());
        }
    }

}
