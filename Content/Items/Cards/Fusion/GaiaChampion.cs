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
    public class GaiaChampion : BaseCardFusion
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/GaiaChampion";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = ModContent.BuffType<GaiaChampionBuff>();
            Item.buffTime = 60 * 45;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 240);
            return true;
        }
        public class GaiaChampionBuff : ModBuff
        {
            public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/GaiaChampionBuff";

            public override void SetStaticDefaults()
            {
                Main.buffNoTimeDisplay[Type] = false;
                Main.debuff[Type] = false;
            }

            public override void Update(Player player, ref int buffIndex)
            {
                player.moveSpeed *= 1.5f;
                player.runAcceleration *= 1.5f;
            }
        }

    }
}
