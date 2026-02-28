using Terraria;
using Terraria.ModLoader;
using static NaturiumMod.Content.NPCs.ManyGlobalNPC;

public class AntiGravDebuff : ModBuff
{
    public override string Texture => "NaturiumMod/Assets/Buffs/BarkionsBuff";
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;
        Main.buffNoSave[Type] = true;
    }

    public override void Update(NPC npc, ref int buffIndex)
    {
        npc.velocity.Y -= 0.15f;
        npc.noGravity = true;
        npc.GetGlobalNPC<AntiGravityGlobalNPC>().hadAntiGravity = true;
    }

}
