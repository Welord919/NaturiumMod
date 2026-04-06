using NaturiumMod.Content.Items.PreHardmode.ApophisItems;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Rares;
public class CurseofDragon : BaseCardRare
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/CurseofDragon";

    public override void SetDefaults()
    {
        base.SetDefaults();
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
            player.moveSpeed *= 1.1f;
            player.runAcceleration *= 1.1f;
        }
    }

}
