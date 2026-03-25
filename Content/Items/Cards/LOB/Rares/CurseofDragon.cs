using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Rares;
public class CurseofDragon : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/CurseofDragon";

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
        Item.rare = ItemRarityID.Green;
        Item.noUseGraphic = true;
        Item.noMelee = true;
        Item.value = Item.buyPrice(silver: 25);
        Item.buffType = ModContent.BuffType<CurseOfDragonBuff>();
        Item.buffTime = 60 * 30;
    }
    public override bool? UseItem(Player player)
    {
        player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
        return true;
    }
    public class CurseOfDragonBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/CurseofDragonBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Triple movement speed
            player.moveSpeed *= 1.5f;
            player.runAcceleration *= 1.5f;
        }
    }

}
