using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.NPCs.ManyGlobalNPC;

namespace NaturiumMod.Content.Items.PreHardmode.ApophisItems; 
public class DecayDebuff : ModBuff
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/DecayDebuff";
    public override void SetStaticDefaults()
    {
        Main.debuff[Type] = true;          // It's a debuff
        Main.buffNoSave[Type] = true;      // Doesn't persist on reload
        Main.buffNoTimeDisplay[Type] = false;
        BuffID.Sets.IsATagBuff[Type] = true; // Allows minions to benefit
    }
}
public class DecayGlobalNPC : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public bool hasDecay;

    public override void PostAI(NPC npc)
    {
        if (npc.HasBuff(ModContent.BuffType<DecayDebuff>()))
        {
            hasDecay = true;
        }
    }

    public override void UpdateLifeRegen(NPC npc, ref int damage)
    {
        if (hasDecay)
        {
            int dps = 6;
            npc.lifeRegen -= dps;

            if (damage < dps / 2)
                damage = dps / 2;
        }
    }

    public override void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
    {
        if (hasDecay)
        {
            modifiers.Defense.Flat -= 4;
        }
    }

    public override void ResetEffects(NPC npc)
    {
        if (!npc.HasBuff(ModContent.BuffType<DecayDebuff>()))
        {
            hasDecay = false;
        }
    }

    public override void DrawEffects(NPC npc, ref Color drawColor)
    {
        if (hasDecay)
        {
            // Purple plague dust
            if (Main.rand.NextBool(4))
            {
                int dust = Dust.NewDust(
                    npc.position,
                    npc.width,
                    npc.height,
                    DustID.Poisoned, // base dust type
                    0f, 0f,
                    150,
                    new Color(180, 0, 255), // purple tint
                    1.3f
                );

                Main.dust[dust].noGravity = true;
            }

            // Slight purple tint on the NPC
            drawColor = Color.Lerp(drawColor, new Color(180, 0, 255), 0.35f);
        }
    }
}
public class PlagueResin : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/PlagueResin";
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 999;
        Item.useStyle = ItemUseStyleID.EatFood;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.UseSound = SoundID.Item3;
        Item.consumable = true;
        Item.rare = ItemRarityID.Green;
        Item.value = Item.sellPrice(silver: 5);

        // Gives the buff for 5 minutes
        Item.buffType = ModContent.BuffType<PlagueInfusionBuff>();
        Item.buffTime = 60 * 60 * 5; // 5 minutes
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<PlagueChunk>(), 5),
                new(ItemID.BottledWater, 1)
        ], TileID.ImbuingStation);
        recipe.Register();
    }
}
public class PlagueInfusionBuff : ModBuff
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Apophis/PlagueInfusionBuff";
    public override void SetStaticDefaults()
    {
        Main.buffNoSave[Type] = false;
        Main.debuff[Type] = false;
        Main.buffNoTimeDisplay[Type] = false;
    }
}
public class PlagueInfusionGlobalItem : GlobalItem
{
    public override bool InstancePerEntity => true;

    // Melee weapons that use item hitboxes
    public override void OnHitNPC(Item item, Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (item.DamageType == DamageClass.Melee &&
            player.HasBuff(ModContent.BuffType<PlagueInfusionBuff>()))
        {
            ApplyDecay(target);
        }
    }

    private void ApplyDecay(NPC target)
    {
        target.AddBuff(ModContent.BuffType<DecayDebuff>(), 300);

        var g = target.GetGlobalNPC<DecayGlobalNPC>();
        g.hasDecay = true;
    }
}




