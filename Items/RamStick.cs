using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

/*
namespace ExabytesProtogenMod
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
		public override bool UseItem(Player player)
		{
			player.AddBuff(BuffID.WellFed, 18000);
			return true;
		}
	}
	class RamDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if ((npc.type == NPCID.Probe || npc.type == NPCID.MartianTurret) && Main.rand.Next(3) == 0)
            {
				Item.NewItem(npc.getRect(), ModContent.ItemType<RamStick>());
            }
        }
    }
}
//*/