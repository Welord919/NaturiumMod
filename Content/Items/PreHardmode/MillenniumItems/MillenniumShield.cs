using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PostHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.MillenniumItems;
[AutoloadEquip(EquipType.Shield)]
public class MillenniumShield : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumShield";
    
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
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<ExampleDashPlayer>().DashAccessoryEquipped = true;
        player.noKnockback = true;


    }
    public class ExampleDashPlayer : ModPlayer
    {
        public const int DashDown = 0;
        public const int DashUp = 1;
        public const int DashRight = 2;
        public const int DashLeft = 3;

        public const int DashCooldown = 60;
        public const int DashDuration = 30;
        public const float DashVelocity = 10f;

        public int DashDir = -1;
        public bool DashAccessoryEquipped;
        public int DashDelay = 0;
        public int DashTimer = 0;

        private bool[] npcHitThisDash = new bool[Main.maxNPCs];

        public override void ResetEffects()
        {
            DashAccessoryEquipped = false;

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
                Player.immuneTime = 30;
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
                    int damage = 30;
                    float knockback = 6f;

                    Player.ApplyDamageToNPC(npc, damage, knockback, Player.direction, false);
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
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<MillenniumPiece>(), 15),
            new(ItemID.EoCShield, 1),
            new(ItemID.CobaltShield, 1),
            new(ItemID.Amber, 5)
        ], TileID.TinkerersWorkbench);
        recipe.Register();
    }
}

