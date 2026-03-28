using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.NPCDrop
{
    public class BalloonLizardCard : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/NPCDrop/BalloonLizardCard";

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
            Item.rare = ItemRarityID.Lime;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(silver: 20);
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
        }
    }

}
