using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using StructureHelper.Content.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static AssGen.Assets;

namespace NaturiumMod.Content.Items.Cards.Crafted
{
    public class StimPack : BaseCardSuper
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/StimPack"; 

        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.buffType = ModContent.BuffType<StimPackBuff>();
            Item.buffTime = 60 * 30;
        }

        public override bool CanUseItem(Player player)
        {
            return !player.HasBuff(Item.buffType);
        }

        public override bool? UseItem(Player player)
        {
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 180);
            return true;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.BattlePotion, 1),
            new(ModContent.ItemType<SpellEssence>(), 5)
            ], ModContent.TileType<FusionAltarTile>());
            recipe.Register();
        }
    }
    public class StimPackBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/Crafted/StimPackBuff";

        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
            Main.buffNoSave[Type] = false;
            Main.pvpBuff[Type] = false;
        }
        public override void Update(Player player, ref int buffIndex)
        {
            var modPlayer = player.GetModPlayer<StimPackPlayer>();

            modPlayer.stimActive = true;
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return;

            modPlayer.stimTickCounter++;
            if (modPlayer.stimTickCounter >= 60)
            {
                modPlayer.stimTickCounter = 0;

                // Calculate 2% of max life (statLifeMax2 is the effective max life including buffs)
                int damage = (int)Math.Floor((player.statLifeMax2 * 0.02f) + 2);
                if (damage < 1) damage = 1;

                // Prevent the tick from reducing the player below 1 HP here:
                int effectiveDamage = Math.Min(damage, Math.Max(0, player.statLife - 1));

                if (effectiveDamage > 0)
                {
                    // Use NetworkText to avoid the obsolete string overload
                    var reason = PlayerDeathReason.ByCustomReason(NetworkText.FromLiteral($"{player.name} overdosed on a StimPack."));
                    player.Hurt(reason, effectiveDamage, 0);

                }

            }
        }
        public override void Update(NPC npc, ref int buffIndex)
        {
        }
    }
    public class StimPackPlayer : ModPlayer
    {
        public bool stimActive;
        public int stimTickCounter;

        public override void ResetEffects()
        {
            stimActive = false;
        }
        private static float GetStimMultiplier(int baseDamage)
        {
            if (baseDamage <= 0) return 1f;
            int bonus = (int)Math.Floor(baseDamage * 0.33f);
            if (bonus > 15) bonus = 15;
            if (bonus <= 0) return 1f;
            return (baseDamage + bonus) / (float)baseDamage;
        }
        public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
        {
            if (!stimActive) return;

            int baseDamage = item.damage;
            float mult = GetStimMultiplier(baseDamage);

            damage *= mult;
        }
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
            if (!stimActive) return;

            int baseDamage = proj.damage;
            float mult = GetStimMultiplier(baseDamage);

            modifiers.SourceDamage *= mult;
        }
    }

}
