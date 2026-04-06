using NaturiumMod.Content.Items.PreHardmode.Accessories.LizardBalloon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.NPCDrop
{
    public class BalloonLizardCard : BaseCardSuper
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/NPCDrop/BalloonLizardCard";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.rare = ItemRarityID.Lime;
            Item.value = Item.buyPrice(silver: 25);
            Item.buffType = ModContent.BuffType<BalloonLizardBuff>();
            Item.buffTime = 60 * 60 * 5;
        }
    }
    public class BalloonLizardBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/NPCDrop/BalloonLizardBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense += 4;
            player.jumpBoost = true;
            player.jumpSpeedBoost += 1.2f;
            player.GetJumpState<LizardSandstormJump>().Enable();
        }
    }
}
