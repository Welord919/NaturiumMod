using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Buffs;

public class MktPotionBuff : ModBuff
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.endurance += 0.05f;
        player.lifeRegen += 1 * 2;
    }
}
