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
    public class FlameGhost : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Fusion/NoEffects/FlameGhost";

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
            Item.rare = ItemRarityID.LightPurple;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = 2000;
        }

        public override bool? UseItem(Player player)
        {
            // Fire/lava immunity
            player.AddBuff(BuffID.ObsidianSkin, 60 * 120);

            // Lava swimming
            player.AddBuff(ModContent.BuffType<FlameGhostBuff>(), 60 * 120);

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
