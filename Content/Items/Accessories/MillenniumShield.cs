using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Accessories.CraftingTrees.BaseGameCombos;
using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace NaturiumMod.Content.Items.Accessories;
[AutoloadEquip(EquipType.Shield)]
public class MillenniumShield : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Millennium/MillenniumShield";
    
    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 28;
        Item.value = Item.buyPrice(10);
        Item.rare = ItemRarityID.Yellow;
        Item.accessory = true;
        Item.damage = 30;
        Item.knockBack = 6f;
        Item.defense = 4;
        Item.value = Item.buyPrice(gold: 5);

    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<MillenniumPiece>(), 15),
            new(ItemID.EoCShield, 1),
            new(ItemID.CobaltShield, 1)
        ], TileID.TinkerersWorkbench);
        recipe.Register();
    }
    public override bool CanEquipAccessory(Player player, int slot, bool modded)
    {
        for (int i = 0; i < player.armor.Length; i++)
        {
            Item item = player.armor[i];
            if (item != null && !item.IsAir)
            {
                if ( item.type == ModContent.ItemType<UnboundMillenniumShield>() || item.type == ModContent.ItemType<MillenniumShieldPlated>() || item.type == ModContent.ItemType<MillenniumShield>())
                {
                    return false;
                }
            }
        }

        return base.CanEquipAccessory(player, slot, modded);
    }
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var dash = player.GetModPlayer<MillenniumDash>();

        dash.DashAccessoryEquipped = true;
        dash.DashCooldown = 60;
        dash.DashDuration = 30;
        dash.DashVelocity = 10f;
        dash.DashDamage = 30;
        dash.DashKnockback = 6f;
        dash.DashIFrames = 30;

        player.noKnockback = true;
    }
    public class MillenniumDash : ModPlayer
    {
        // Direction constants
        public const int DashDown = 0;
        public const int DashUp = 1;
        public const int DashRight = 2;
        public const int DashLeft = 3;

        // PARAMETERS (set by shields)
        public bool DashAccessoryEquipped;
        public int DashCooldown = 60;
        public int DashDuration = 30;
        public float DashVelocity = 10f;
        public int DashDamage = 30;
        public float DashKnockback = 6f;
        public int DashIFrames = 30;

        // Runtime state
        public int DashDir = -1;
        public int DashDelay = 0;
        public int DashTimer = 0;

        private bool[] npcHitThisDash = new bool[Main.maxNPCs];

        public override void ResetEffects()
        {
            DashAccessoryEquipped = false;

            // Reset direction detection
            if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15)
                DashDir = DashDown;
            else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15)
                DashDir = DashUp;
            else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15 && Player.doubleTapCardinalTimer[DashLeft] == 0)
                DashDir = DashRight;
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15 && Player.doubleTapCardinalTimer[DashRight] == 0)
                DashDir = DashLeft;
            else
                DashDir = -1;
        }

        public override void PreUpdateMovement()
        {
            if (CanUseDash() && DashDir != -1 && DashDelay == 0)
            {
                Vector2 newVelocity = Player.velocity;

                switch (DashDir)
                {
                    case DashUp when Player.velocity.Y > -DashVelocity:
                    case DashDown when Player.velocity.Y < DashVelocity:
                        float yDir = DashDir == DashDown ? 1 : -1.3f;
                        newVelocity.Y = yDir * DashVelocity;
                        break;

                    case DashLeft when Player.velocity.X > -DashVelocity:
                    case DashRight when Player.velocity.X < DashVelocity:
                        float xDir = DashDir == DashRight ? 1 : -1;
                        newVelocity.X = xDir * DashVelocity;
                        break;

                    default:
                        return;
                }

                DashDelay = DashCooldown;
                DashTimer = DashDuration;
                Player.velocity = newVelocity;

                for (int i = 0; i < npcHitThisDash.Length; i++)
                    npcHitThisDash[i] = false;

                Player.immune = true;
                Player.immuneTime = DashIFrames;
                Player.immuneNoBlink = true;
            }

            if (DashDelay > 0)
                DashDelay--;

            if (DashTimer > 0)
            {
                Player.eocDash = DashTimer;
                Player.armorEffectDrawShadowEOCShield = true;

                Player.immune = true;
                Player.immuneNoBlink = true;

                DoDashDamage();

                DashTimer--;
            }
            else
            {
                Player.immuneNoBlink = false;
            }
        }

        private void DoDashDamage()
        {
            Rectangle hitbox = Player.Hitbox;

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];

                if (!npc.active || npc.friendly || npc.dontTakeDamage)
                    continue;

                if (npcHitThisDash[i])
                    continue;

                if (hitbox.Intersects(npc.Hitbox))
                {
                    Player.ApplyDamageToNPC(npc, DashDamage, DashKnockback, Player.direction, false);
                    npcHitThisDash[i] = true;
                }
            }
        }

        private bool CanUseDash()
        {
            return DashAccessoryEquipped
                && Player.dashType == DashID.None
                && !Player.setSolar
                && !Player.mount.Active;
        }
    }

}

