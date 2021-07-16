using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

//*
namespace ExabytesProtogenMod.Items
{
	public class RamStick : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("4GB RAM stick");
			Tooltip.SetDefault("Tasty! (If you're a protogen)");
		}
		public override void SetDefaults()
		{
			item.width = 20;
			item.height = 20;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useTurn = true;
			item.maxStack = 30;
			item.consumable = true;
			item.UseSound = SoundID.Item2;
			item.buffTime = 18000;
			item.buffType = BuffID.WellFed;
		}
		public override bool CanUseItem(Player player)
		{
			var modPlayer = player.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
            if (modPlayer.race is Common.Races.Protogens.Protogen)
            {
				return !player.HasBuff(BuffID.WellFed);
			}
            else
            {
				Main.NewText("If you ate this, you would probably die. Don't.");
            }
			return modPlayer.race is Common.Races.Protogens.Protogen && !player.HasBuff(BuffID.WellFed);
		}
	}
	class RamDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (((npc.type == NPCID.Probe || npc.type == NPCID.MartianTurret || npc.type == NPCID.DeadlySphere || npc.type == NPCID.MartianDrone) && Main.rand.Next(50) == 0) || (npc.type == NPCID.TheDestroyer || npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism || npc.type == NPCID.SkeletronPrime || npc.type == NPCID.PrimeCannon || npc.type == NPCID.PrimeSaw || npc.type == NPCID.PrimeVice || npc.type == NPCID.PrimeLaser || npc.type == NPCID.MartianSaucer) && Main.rand.Next(4) == 0)
            {
				Item.NewItem(npc.getRect(), ModContent.ItemType<RamStick>());
            }
        }
    }
}
//*/