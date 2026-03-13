using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Cards
{
    public class AquaMador : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Cards/aquamador";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Blue;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.value = Item.buyPrice(silver: 25);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<AquaMadorBuff>(), 60 * 20);
            var mp = player.GetModPlayer<AquaMadorPlayer>();
            mp.currentOverHealth = 50;
            return true;
        }
    }

    public class AquaMadorBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Cards/AquaMadorBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var mp = player.GetModPlayer<AquaMadorPlayer>();
            mp.aquaMadorActive = true;
        }
    }

    public class AquaMadorPlayer : ModPlayer
    {
        public bool aquaMadorActive;
        public int currentOverHealth; // 0–50
        private int decayTimer;

        public override void ResetEffects()
        {
            aquaMadorActive = false;
        }

        public override void UpdateDead()
        {
            currentOverHealth = 0;
        }
        public override void PostUpdate()
        {
            if (Player.wet && !Player.honeyWet && !Player.lavaWet)
            {
                Player.AddBuff(BuffID.Merfolk, 2);

                // Enable actual merfolk physics
                Player.merman = true;

            }
        }
        public override void PostUpdateMiscEffects()
        {
            if (!aquaMadorActive)
            {
                currentOverHealth = 0;
                return;
            }

            // +5 defense
            Player.statDefense += 5;

            // Overhealth decay: 1 per second
            if (currentOverHealth > 0)
            {
                decayTimer++;
                if (decayTimer >= 60)
                {
                    decayTimer = 0;
                    currentOverHealth--;

                    if (currentOverHealth <= 0)
                    {
                        currentOverHealth = 0;
                        Player.ClearBuff(ModContent.BuffType<AquaMadorBuff>());
                    }
                }
            }
        }

        // ⭐ THIS is the missing piece — inject overhealth into max life
        public override void PostUpdateEquips()
        {
            if (aquaMadorActive && currentOverHealth > 0)
            {
                Player.statLifeMax2 += currentOverHealth;
            }
        }
    }
}