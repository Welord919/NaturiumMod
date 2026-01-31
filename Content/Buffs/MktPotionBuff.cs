using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Buffs;

public class MktPotionBuff : ModBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.statDefense += 6;
        player.lifeRegen += (int)1.8;
    }
}
