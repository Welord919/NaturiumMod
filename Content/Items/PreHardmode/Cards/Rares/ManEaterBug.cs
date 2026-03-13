
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Cards.Rares
{
    // ============================
    //  MAN-EATER BUG ITEM
    // ============================
    public class ManEaterBug : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Cards/ManEaterBug";

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
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 50);
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
            player.AddBuff(ModContent.BuffType<ManEaterBugGuard>(), 60 * 60);
            return true;
        }
    }

    // ============================
    //  MAN-EATER BUG BUFF
    // ============================
    public class ManEaterBugGuard : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Cards/ManEaterBugBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = false;
            Main.debuff[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<BugPlayer>().bugGuardActive = true;
        }
    }

    public class BugPlayer : ModPlayer
    {
        public bool bugGuardActive;

        public override void ResetEffects()
        {
            bugGuardActive = false;
        }

        public override void PostHurt(Player.HurtInfo info)
        {
            if (!bugGuardActive)
                return;

            // Undo the damage AFTER the hit fully resolves
            Player.statLife += info.Damage;
            if (Player.statLife > Player.statLifeMax2)
                Player.statLife = Player.statLifeMax2;

            int reflectDamage = (int)(info.Damage * 1.5);

            NPC attackerNPC = null;

            // NPC melee
            int npcIndex = info.DamageSource.SourceNPCIndex;
            if (npcIndex >= 0 && npcIndex < Main.maxNPCs)
            {
                NPC npc = Main.npc[npcIndex];
                if (npc != null && npc.active)
                    attackerNPC = npc;
            }

            // NPC projectile
            int projIndex = info.DamageSource.SourceProjectileLocalIndex;
            if (attackerNPC == null && projIndex >= 0 && projIndex < Main.maxProjectiles)
            {
                Projectile proj = Main.projectile[projIndex];
                if (proj != null && proj.active && proj.owner >= 0 && proj.owner < Main.maxNPCs)
                {
                    NPC owner = Main.npc[proj.owner];
                    if (owner != null && owner.active)
                        attackerNPC = owner;
                }
            }

            if (attackerNPC != null)
                attackerNPC.SimpleStrikeNPC(reflectDamage, 0);

            Player.ClearBuff(ModContent.BuffType<ManEaterBugGuard>());
            bugGuardActive = false;
        }

    }
}