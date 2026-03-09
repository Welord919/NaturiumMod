using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons.Melee
{
    public class PlaguespreaderArm : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/PlaguespreaderArm";
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 23;
            Item.useAnimation = 23;

            Item.damage = 18; // slightly stronger than Zombie Arm
            Item.knockBack = 5f;
            Item.DamageType = DamageClass.Melee;

            Item.value = Item.buyPrice(silver: 50);
            Item.rare = ItemRarityID.Blue;

            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;

            // Longer reach
            Item.scale = 1.35f;
        }

        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Apply decay debuff
            target.AddBuff(ModContent.BuffType<DecayDebuff>(), 300);

            // If the hit killed the NPC → grant speed buff
            if (target.life <= 0 && !target.friendly && target.damage > 0)
            {
                player.AddBuff(BuffID.Swiftness, 600);
            }
        }
    }
}