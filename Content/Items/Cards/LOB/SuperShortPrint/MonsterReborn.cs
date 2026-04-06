using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Cards.LOB.Rares.CurseofDragon;

namespace NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint
{

    public class MonsterReborn : BaseCardSuper
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/MonsterReborn";

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = ModContent.BuffType<RebornBuff>();
            Item.buffTime = 60 * 60 * 10;
            Item.rare = ItemRarityID.Green;
        }
        public override bool? UseItem(Player player)
        {
            CombatText.NewText(player.getRect(), Color.Cyan, "Your next card is protected!");
            return true;
        }
        public static bool TryPreventCardConsumption(Player player)
        {
            int rebornBuff = ModContent.BuffType<RebornBuff>();
            if (player.HasBuff(rebornBuff))
            {
                player.ClearBuff(rebornBuff);
                CombatText.NewText(player.getRect(), Color.LightBlue, "Monster Reborn protected your card!");
                return false;
            }
            return true;
        }
    }
    public class RebornBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/RebornBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.debuff[Type] = false;
        }
    }

}