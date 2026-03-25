using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.MillenniumItems
{
    public class MillenniumKey : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumKey";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 20;
            Item.useTime = 20;

            Item.noMelee = true;
            Item.mana = 40;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 3);
            Item.shootSpeed = 0f;
        }

        public override bool CanUseItem(Player player)
        {
            // Left click = buff
            if (player.altFunctionUse != 2)
            {
                player.AddBuff(ModContent.BuffType<MillenniumKeyBuff>(), 600);
                Terraria.Audio.SoundEngine.PlaySound(SoundID.Item100, player.Center);

                return true;
            }

            return false;
        }

    }
    public class MillenniumKeyBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumKeyBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.detectCreature = true;
            player.findTreasure = true;
            player.dangerSense = true;
        }
    }
}
