using NaturiumMod.Content.Items.Cards.LOB.Commons;
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
    public class MetalDragon : BaseCardFusion
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/MetalDragon";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = ModContent.BuffType<MetalDragonBuff>();
            Item.buffTime = 60 * 60 * 2;
        }
        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
            return true;
        }
    }
    public class MetalDragonBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/MetalDragonBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<MetalDragonPlayer>().metalDragonActive = true;
        }
    }
    public class MetalDragonPlayer : ModPlayer
    {
        public bool metalDragonActive;

        public override void ResetEffects()
        {
            metalDragonActive = false;
        }

        public override void PostUpdate()
        {
            if (metalDragonActive)
            {
                // +5 defense
                Player.statDefense += 5;

                // 5% damage reduction
                Player.endurance += 0.05f;
            }
        }
    }

}
