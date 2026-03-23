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
    public class MetalDragon : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/MetalDragon";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Orange;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        public override bool? UseItem(Player player)
        {
            // Lava swimming
            player.AddBuff(ModContent.BuffType<MetalDragonBuff>(), 60 * 60);

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
