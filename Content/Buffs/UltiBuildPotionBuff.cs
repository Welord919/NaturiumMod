using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Buffs;

public class MktPotionBuff : ModBuff
{
    public override string Texture => "NaturiumMod/Assets/Buffs/UltiBuildPotionBuff";

    public override void Update(Player player, ref int buffIndex)
    {
        player.blockRange += 1;
        player.tileSpeed += 0.25f;
        player.wallSpeed += 0.25f;
        player.pickSpeed -= 0.25f;

    }
}
