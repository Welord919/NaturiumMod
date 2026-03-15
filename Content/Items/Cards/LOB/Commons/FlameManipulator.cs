using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

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
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<FireDamageBuff>(), 60 * 30);
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

            public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
            {
                if (!flameManipulatorActive)
                    return;

                // Buff your custom fire-tagged cards
                if (WeaponTag.ItemTags.TryGetValue(item.type, out var tags) && tags.Contains("Fire"))
                {
                    damage *= 1.10f; // +10% fire card damage
                }

                // Buff vanilla fire weapons (we detect them by checking if they apply OnFire!)
                if (item.buffType == BuffID.OnFire || item.shoot == ProjectileID.Flames)
                {
                    damage *= 1.05f;
                }
            }
        }
    }
}
