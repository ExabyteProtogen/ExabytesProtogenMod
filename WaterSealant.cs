using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ExabytesProtogenMod
{
	public class WaterSealantBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Waterproofed");
			Description.SetDefault("You'll no longer short-circuit in water!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}
	}
    public class WaterSealant : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Water Sealant");
			Tooltip.SetDefault("Apply this stuff and you won't get shocked in water!\nYou can use it as a non-protogen, but I don't know why you would...");
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
			item.UseSound = SoundID.Item3;
        }
        public override bool CanUseItem(Player player)
        {
			int buff = mod.BuffType("WaterSealantBuff");
			return !player.HasBuff(buff);
        }
        public override bool UseItem(Player player)
        {
			player.AddBuff(mod.BuffType("WaterSealantBuff"), 18000);
			return true;
        }
		public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bottle);
			recipe.AddIngredient(ItemID.Gel, 5);
			recipe.AddIngredient(ItemID.Waterleaf, 2);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
    }
}