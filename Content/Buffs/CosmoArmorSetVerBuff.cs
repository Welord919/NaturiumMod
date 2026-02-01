using Terraria;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.General.Projectiles;

namespace NaturiumMod.Content.Buffs;

public class CosmoArmorSetVerBuff : ModBuff
{
    public override string Texture => "NaturiumMod/Assets/Buffs/CosmoArmorSetVerBuff";

    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = true;        // Buff won't save when you exit the world
        Main.buffNoTimeDisplay[Type] = true; // Time remaining won't display on this buff
    }

    public override void Update(Player player, ref int buffIndex)
    {
        // If the minions exist reset the buff time, otherwise remove the buff from the player
        if (player.ownedProjectileCounts[ModContent.ProjectileType<CosmoArmorSetVer>()] > 0)
        {
            player.buffTime[buffIndex] = 5;
            return;
        }

        player.DelBuff(buffIndex);
        buffIndex--;
    }
}
