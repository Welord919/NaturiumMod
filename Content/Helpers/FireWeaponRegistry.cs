using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Helpers
{
    public static class FireWeaponRegistry
    {
        public static readonly HashSet<int> FireItems = new()
    {
        ItemID.Flamelash,
        ItemID.Flamethrower,
        ItemID.HelFire,
        ItemID.MoltenFury,
        ItemID.FieryGreatsword,
        ItemID.FlowerofFire,
        ItemID.InfernoFork,
        ItemID.MeteorStaff,
        ItemID.MoltenPickaxe,
        ItemID.MoltenHamaxe,
        ItemID.MoltenQuiver,
        ItemID.MoltenSkullRose,
        ItemID.MagmaStone,
        ItemID.Flamarang,
        ItemID.DD2FlameburstTowerT1Popper,
        ItemID.DD2FlameburstTowerT2Popper,
        ItemID.DD2FlameburstTowerT3Popper,
    };

        public static readonly HashSet<int> FireProjectiles = new()
    {
        ProjectileID.Flamelash,
        ProjectileID.FlamethrowerTrap,
        ProjectileID.Fireball,
        ProjectileID.Flames,
        ProjectileID.FireArrow,
        ProjectileID.HellfireArrow,
        ProjectileID.InfernoFriendlyBlast,
        ProjectileID.InfernoHostileBlast,
        ProjectileID.Meteor1,
        ProjectileID.Meteor2,
        ProjectileID.Meteor3,
        ProjectileID.Flamarang,
        ProjectileID.DD2FlameBurstTowerT1Shot,
        ProjectileID.DD2FlameBurstTowerT2Shot,
        ProjectileID.DD2FlameBurstTowerT3Shot,
    };
        public static void RegisterModdedFireItem(int itemType)
        {
            FireItems.Add(itemType);
        }

        // Add modded fire projectiles here
        public static void RegisterModdedFireProjectile(int projType)
        {
            FireProjectiles.Add(projType);
        }
    }
    public class ModdedFlameWeaponSystem : ModSystem
    {
        public override void PostSetupContent()
        {
            // Register modded fire items
            FireWeaponRegistry.RegisterModdedFireItem(ModContent.ItemType<FlameSwordsman>());
            FireWeaponRegistry.RegisterModdedFireItem(ModContent.ItemType<DarkfireDragon>());
            FireWeaponRegistry.RegisterModdedFireItem(ModContent.ItemType<REBD>());

            // Register modded fire projectiles
            FireWeaponRegistry.RegisterModdedFireProjectile(ModContent.ProjectileType<RedEyesFireball>());
            FireWeaponRegistry.RegisterModdedFireProjectile(ModContent.ProjectileType<RedEyesExplosion>());
        }
    }
    }