using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class PetiteDragon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteDragon";

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
            player.AddBuff(ModContent.BuffType<PetiteDragonBuff>(), 60 * 30);
            return true;
        }
    }

    public class PetiteDragonBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PetiteDragonBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<PetiteDragonPlayer>().petiteDragonActive = true;
        }
    }

    public class PetiteDragonPlayer : ModPlayer
    {
        public bool petiteDragonActive;

        public override void ResetEffects()
        {
            petiteDragonActive = false;
        }

        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (petiteDragonActive && item.DamageType == ModContent.GetInstance<CardDamage>())
            {
                damage *= 1.05f; // +5% card damage
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (petiteDragonActive && proj.DamageType == ModContent.GetInstance<CardDamage>())
            {
                modifiers.SourceDamage *= 1.05f; // +5% card projectile damage
            }
        }
    }
}