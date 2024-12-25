using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Buffs;

public class BarkionsBuff : ModBuff
{
    public float MultiplicativeDamageBonus { get; private set; }

    public override void SetStaticDefaults()
    {
        Main.buffNoTimeDisplay[Type] = true;
        Main.debuff[Type] = false;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        player.GetDamage(DamageClass.Generic) *= 1 + (MultiplicativeDamageBonus / 100f);
    }
}