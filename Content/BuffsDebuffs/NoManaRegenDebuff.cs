using Terraria;
using Terraria.ModLoader;

public class NoManaRegenDebuff : ModBuff
{
    public override string Texture => "NaturiumMod/Assets/Buffs/NoManaRegenDebuff";
    public override void Update(Player player, ref int buffIndex)
    {
        // Completely disable mana regeneration
        player.manaRegen = 0;
        player.manaRegenDelay = 60;
    }
}
