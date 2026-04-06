using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.Fusion
{
    public class FlameGhost : BaseCardFusion
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FlameGhost";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = ModContent.BuffType<FlameGhostBuff>();
            Item.buffTime = 60 * 60 * 4;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
            return true;
        }
    }
    public class FlameGhostBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/FlameGhostBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Swim in lava like water
            player.lavaImmune = true;
            player.ignoreWater = false;
            player.accFlipper = true;
            player.gills = true;     

            // Smooth movement in lava
            if (player.lavaWet)
            {
                player.gravity = 0.3f;
                player.maxFallSpeed = 2f;
                player.velocity.Y *= 0.9f;
            }
        }
    }

}
