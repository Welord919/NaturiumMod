using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint
{
    public class PetiteAngel : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteAngel";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item44;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;
            Item.damage = 8;
            Item.knockBack = 1f;

            Item.buffType = ModContent.BuffType<PetiteAngelBuff>();

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 50);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(Item.buffType, 60 * 40);
            return true;
        }
    }

    public class PetiteAngelBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteAngelBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PetiteAngelPlayer>().petiteAngelActive = true;
        }
    }

    public class PetiteAngelPlayer : ModPlayer
    {
        public bool petiteAngelActive;

        public override void ResetEffects()
        {
            petiteAngelActive = false;
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (petiteAngelActive && item.DamageType == ModContent.GetInstance<CardDamage>())
            {
                damage *= 1.05f; // +5% card damage
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (petiteAngelActive && proj.DamageType == ModContent.GetInstance<CardDamage>())
            {
                modifiers.SourceDamage *= 1.05f; // +5% card projectile damage
            }
        }
    }
}