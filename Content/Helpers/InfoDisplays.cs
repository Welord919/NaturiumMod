using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Helpers
{
    public class MinionInfoDisplay : InfoDisplay
    {
        public override string Texture => "NaturiumMod/Assets/UI/ExampleInfoDisplay";

        public override bool Active()
        {
            Player player = Main.LocalPlayer;

            // Only show if the accessory is equipped
            return player.GetModPlayer<MinionInfoPlayer>().minionDisplayEquipped;
        }


        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            Player player = Main.LocalPlayer;

            int current = (int)player.slotsMinions;
            int max = player.maxMinions;

            displayColor = Color.White;

            return $"{current}/{max} Minions";
        }
    }
    public class MinionInfoPlayer : ModPlayer
    {
        public bool minionDisplayEquipped;

        public override void ResetEffects()
        {
            minionDisplayEquipped = false;
        }
    }
}
