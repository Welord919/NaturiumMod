using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Cards.LOB.Rares.CurseofDragon;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class FlameManipulator : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/FlameMani";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Blue;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(silver: 25);
            Item.buffType = ModContent.BuffType<FireDamageBuff>();
            Item.buffTime = 60 * 40;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
            return true;
        }
    }
    public class FireDamageBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/FireDamageBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<FlameManipulatorPlayer>().flameManipulatorActive = true;
        }
        public class FlameManipulatorPlayer : ModPlayer
        {
            public bool flameManipulatorActive;

            public override void ResetEffects()
            {
                flameManipulatorActive = false;
            }

            // ------------------------------------------------------------
            // 1. FIRE DAMAGE BOOST (ITEM DAMAGE)
            // ------------------------------------------------------------
            public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
            {
                if (!flameManipulatorActive)
                    return;

                // Fire-tagged modded cards
                if (WeaponTag.ItemTags.TryGetValue(item.type, out var tags) &&
                    tags.Contains("Fire"))
                {
                    damage *= 1.05f;
                }

                // Vanilla + modded fire weapons
                if (FireWeaponRegistry.FireItems.Contains(item.type))
                {
                    damage *= 1.05f;
                }
            }

            // ------------------------------------------------------------
            // 2. FIRE DAMAGE BOOST (PROJECTILE DAMAGE)
            // ------------------------------------------------------------
            public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
            {
                if (!flameManipulatorActive)
                    return;

                // Fire-tagged modded projectiles
                if (WeaponTagProj.ProjTags.TryGetValue(proj.type, out var tags) &&
                    tags.Contains("Fire"))
                {
                    modifiers.SourceDamage *= 1.05f;
                }

                // Vanilla + modded fire projectiles
                if (FireWeaponRegistry.FireProjectiles.Contains(proj.type))
                {
                    modifiers.SourceDamage *= 1.05f;
                }
            }
        }
    }
}
