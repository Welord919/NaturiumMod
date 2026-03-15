using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards;
[AutoloadEquip(EquipType.Body)]

public class KaibasCloak : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Cards/KaibasCloak";
    public override void SetDefaults()
    {
        Item.width = 18;
        Item.height = 14;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightPurple;
        Item.value = Item.buyPrice(gold: 50);
    }
    public override void Load()
    {
        if (Main.netMode == NetmodeID.Server)
        {
            return;
        }
        EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);
    }
    public override void SetStaticDefaults()
    {
        ArmorIDs.Body.Sets.HidesHands[Item.bodySlot] = false;
    }
    public override void SetMatch(bool male, ref int equipSlot, ref bool robes)
    {
        robes = true;
        equipSlot = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);
    }
    public override void UpdateEquip(Player player)
    {
        player.GetModPlayer<KaibaPlayer>().KaibasCloakEquipped = true;
        var boost = player.GetModPlayer<WeaponBoostPlayer>();
        boost.activeBoosts["Dragon"] = true;
    }
}
public class KaibaPlayer : ModPlayer
{
    public bool KaibasCloakEquipped;

    public override void ResetEffects()
    {
        KaibasCloakEquipped = false;
    }
    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        // Only apply to CardDamage projectiles
        if (proj.DamageType == ModContent.GetInstance<CardDamage>())
        {
            // If Kaiba's Cloak is equipped, apply +10% damage
            if (KaibasCloakEquipped)
            {
                modifiers.SourceDamage *= 1.10f; // +10% damage
            }
        }
    }
    public override void ModifyWeaponDamage(Item item, ref StatModifier damage)
    {
        // Only apply to CardDamage items
        if (item.DamageType == ModContent.GetInstance<CardDamage>())
        {
            if (KaibasCloakEquipped)
            {
                damage *= 1.10f; // +10% damage
            }
        }
    }


}


