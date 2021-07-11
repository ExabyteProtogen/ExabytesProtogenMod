using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using System.Collections.Generic;

namespace ExabytesProtogenMod
{
	public class AirSupportDrone : ModProjectile
	{
		//protected float spacingMult = 1f;
		protected float viewDist = 400f;
		// Bullet stats
		protected float bulletShootCool = 8f; // This'll get buffed as the game progresses
		protected float bulletShootSpeed = 25f;
		int bulletDamage = 20; // So will this
		protected int shoot = ProjectileID.Bullet;
		// Rocket stats
		protected float rocketShootCool = 120f;
		protected float rocketShootSpeed = 15f;
		int rocketDamage = 400;
		protected int rocket = ProjectileID.RocketI;
		float rocketTimer = 0f;
		// Grenade stats
		protected float grenadeShootCool = 120f;
		protected float grenadeShootSpeed = 10f;
		int grenadeDamage = 275;
		protected int grenade = ProjectileID.Grenade;
		float grenadeTimer = 0f;
		Vector3 lightColor = new Vector3(255,255,255);
		float brightness;
		bool defenderMode = false;

		public override void SetStaticDefaults()
        {
			DisplayName.SetDefault("Example Minion");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[projectile.type] = 8;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SetDefaults()
        {
			projectile.width = 50;
			projectile.height = 50;
			projectile.scale = 0.75f;
			// Only controls if it deals damage to enemies on contact (more on that later)
			projectile.friendly = false;
			// Only determines the damage type
			projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player
			projectile.minionSlots = 0f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			projectile.penetrate = -1;
			projectile.tileCollide = false;
		}

		public virtual void CreateDust()
		{
		}

		public virtual void SelectFrame()
		{
		}

		public override void AI()
		{

			Player player = Main.player[projectile.owner];
            if (player.statLife > (player.statLifeMax2 / 0.75f))
            {
				defenderMode = false;
            }
			else
            {
				defenderMode = true;
            }
            //float spacing = (float)projectile.width * spacingMult;

            #region Progression
            if (NPC.downedMoonlord)
            {
				shoot = ProjectileID.MoonlordBullet;
				rocket = ProjectileID.RocketIII;
				rocketDamage = 500;
				bulletDamage = 120;
				bulletShootCool = 4;
            }
            else if (NPC.downedAncientCultist)
            {
				bulletDamage = 75;
				bulletShootCool = 4;
				rocketDamage = 500;
				shoot = ProjectileID.CrystalBullet;
				rocket = ProjectileID.RocketIII;
            }
            else if (NPC.downedGolemBoss)
            {
				bulletDamage = 75;
				bulletShootCool = 4;
				shoot = ProjectileID.BulletHighVelocity;
			}
            else if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
            {
				bulletDamage = 40;
				bulletShootCool = 6;
				shoot = ProjectileID.BulletHighVelocity;
            }
            else if (Main.hardMode)
            {
				bulletDamage = 35;
				bulletShootCool = 6;
            }
			else if (NPC.downedQueenBee)
            {
				bulletDamage = 40;
            }
            #endregion
            #region Active check
            // This is the "active check", makes sure the minion is alive while the player is alive, and despawns if not
            if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<AirSupportBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<AirSupportBuff>()))
			{
				projectile.timeLeft = 2;
			}
            #endregion
			#region Find target
			// Starting search distance
			float distanceFromTarget = viewDist;
			Vector2 targetCenter = projectile.position;
			bool foundTarget = false;

			if (!foundTarget)
			{
                if (defenderMode)
                {
					// This code is required either way, used for finding a target
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC npc = Main.npc[i];
						if (npc.CanBeChasedBy())
						{
							float between = Vector2.Distance(npc.Center, projectile.Center);
							bool closest = Vector2.Distance(projectile.Center, targetCenter) > between;
							bool inRange = between < distanceFromTarget;
							bool lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
							if (((closest && inRange) || !foundTarget) && lineOfSight)
							{
								distanceFromTarget = between;
								targetCenter = npc.Center;
								foundTarget = true;
							}
						}
					}
				}
				else
                {
					List<NPC> enemiesInRange = new List<NPC>();
					float between = 0f;
					bool inRange = false;
					bool lineOfSight = false;
					NPC toughestEnemy = new NPC();
					for (int i = 0; i < Main.maxNPCs; i++)
					{
						NPC npc = Main.npc[i];
						if (npc.CanBeChasedBy())
						{
							between = Vector2.Distance(npc.Center, player.Center);
							inRange = between < distanceFromTarget;
							lineOfSight = Collision.CanHitLine(projectile.position, projectile.width, projectile.height, npc.position, npc.width, npc.height);
							if (inRange && lineOfSight)
							{
                                enemiesInRange.Add(npc);
							}
						}
					}
                    if (enemiesInRange.Count != 0)
                    {
						int maxHealth = 0;
						for (int i = 0; i < enemiesInRange.Count; i++)
						{
							NPC enemy = enemiesInRange[i];
							if (enemy.lifeMax > maxHealth)
							{
								maxHealth = enemy.lifeMax;
								toughestEnemy = enemy;
							}
						}
						distanceFromTarget = between;
						targetCenter = toughestEnemy.Center;
						foundTarget = true;
					}
				}
			}

			// friendly needs to be set to true so the minion can deal contact damage
			// friendly needs to be set to false so it doesn't damage things like target dummies while idling
			// Both things depend on if it has a target or not, so it's just one assignment here
			// You don't need this assignment if your minion is shooting things instead of dealing contact damage
			projectile.friendly = false;
			#endregion
			#region General behavior
			Vector2 targetPosition;
			Vector2 vectorToTargetPosition;
			float distanceToTargetPosition;

			if (defenderMode)
			{
				targetPosition = player.Center;
				targetPosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

				// Teleport to player if distance is too big
				vectorToTargetPosition = targetPosition - projectile.Center;
				distanceToTargetPosition = vectorToTargetPosition.Length();
				if (Main.myPlayer == player.whoAmI && distanceToTargetPosition > 2000f)
				{
					// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
					// and then set netUpdate to true
					projectile.position = targetPosition;
					projectile.velocity *= 0.1f;
					projectile.netUpdate = true;
				}
			}
			else
			{
                if (foundTarget)
                {
					targetPosition = targetCenter;
					targetPosition.Y -= 160f; // Go up 48 coordinates (three tiles from the center of the player)

					// Teleport to player if distance is too big
					vectorToTargetPosition = targetPosition - projectile.Center;
					distanceToTargetPosition = vectorToTargetPosition.Length();
				}
                else
                {
					targetPosition = player.Center;
					targetPosition.Y -= 48f; // Go up 48 coordinates (three tiles from the center of the player)

					// Teleport to player if distance is too big
					vectorToTargetPosition = targetPosition - projectile.Center;
					distanceToTargetPosition = vectorToTargetPosition.Length();
					if (Main.myPlayer == player.whoAmI && distanceToTargetPosition > 2000f)
					{
						// Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
						// and then set netUpdate to true
						projectile.position = targetPosition;
						projectile.velocity *= 0.1f;
						projectile.netUpdate = true;
					}
				}
			}

			// If your minion is flying, you want to do this independently of any conditions
			float overlapVelocity = 0.04f;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				// Fix overlap with other minions
				Projectile other = Main.projectile[i];
				if (i != projectile.whoAmI && other.active && other.owner == projectile.owner && Math.Abs(projectile.position.X - other.position.X) + Math.Abs(projectile.position.Y - other.position.Y) < projectile.width)
				{
					if (projectile.position.X < other.position.X) projectile.velocity.X -= overlapVelocity;
					else projectile.velocity.X += overlapVelocity;

					if (projectile.position.Y < other.position.Y) projectile.velocity.Y -= overlapVelocity;
					else projectile.velocity.Y += overlapVelocity;
				}
			}
			#endregion
			#region Attack behaviour

			// Default movement parameters (here for attacking)
			float speed = 16f;
			float inertia = 15f;

			if (foundTarget)
			{
				Vector2 targetPos = targetCenter;
				if (projectile.ai[1] > 0f)
				{
					projectile.ai[1] += 1f;
				}
				if (projectile.ai[1] > bulletShootCool)
				{
					projectile.ai[1] = 0f;
					projectile.netUpdate = true;
				}
				if (projectile.ai[1] == 0f)
				{
					projectile.ai[1] = 1f;
					if (Main.myPlayer == projectile.owner)
					{
						Vector2 shootVel = targetPos - projectile.Center;
						if (shootVel == Vector2.Zero)
						{
							shootVel = new Vector2(0f, 1f);
						}
						shootVel.Normalize();
						shootVel *= bulletShootSpeed;
						int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, shoot, bulletDamage, projectile.knockBack, Main.myPlayer, 0f, 0f);
						Main.PlaySound(SoundID.Item40, projectile.Center);
						Main.projectile[proj].timeLeft = 300;
						Main.projectile[proj].netUpdate = true;
						Main.projectile[proj].hostile = false;
						projectile.netUpdate = true;
					}
				}
                if (NPC.downedPlantBoss)
                {
					if (rocketTimer > 0f)
					{
						rocketTimer += 1f;
					}
					if (rocketTimer > rocketShootCool)
					{
						rocketTimer = 0f;
						projectile.netUpdate = true;
					}
					if (rocketTimer == 0f)
					{
						rocketTimer = 1f;
						if (Main.myPlayer == projectile.owner)
						{
							Vector2 shootVel = targetPos - projectile.Center;
							if (shootVel == Vector2.Zero)
							{
								shootVel = new Vector2(0f, 1f);
							}
							shootVel.Normalize();
							shootVel *= rocketShootSpeed;
							int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, rocket, rocketDamage, projectile.knockBack, Main.myPlayer, 0f, 0f);
							Main.PlaySound(SoundID.Item92, projectile.Center);
							Main.projectile[proj].timeLeft = 300;
							Main.projectile[proj].netUpdate = true;
							Main.projectile[proj].hostile = false;
							projectile.netUpdate = true;
						}
					}
				}
                else
                {
                    if (Main.hardMode)
                    {
						if (grenadeTimer > 0f)
						{
							grenadeTimer += 1f;
						}
						if (grenadeTimer > rocketShootCool)
						{
							grenadeTimer = 0f;
							projectile.netUpdate = true;
						}
						if (grenadeTimer == 0f)
						{
							grenadeTimer = 1f;
							if (Main.myPlayer == projectile.owner)
							{
								Vector2 shootVel = targetPos - projectile.Center;
								if (shootVel == Vector2.Zero)
								{
									shootVel = new Vector2(0f, 1f);
								}
								shootVel.Normalize();
								shootVel *= grenadeShootSpeed;
								int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, shootVel.X, shootVel.Y, grenade, grenadeDamage, projectile.knockBack, Main.myPlayer, 0f, 0f);
								Main.PlaySound(SoundID.Item61, projectile.Center);
								Main.projectile[proj].timeLeft = 300;
								Main.projectile[proj].netUpdate = true;
								projectile.netUpdate = true;
							}
						}
					}
                }
			}
            #endregion
            #region Movement
            if (defenderMode)
            {
				if (distanceToTargetPosition > 600f)
				{
					// Speed up the minion if it's away from the player
					speed = 24f;
					inertia = 20f;
				}
				else
				{
					// Slow down the minion if closer to the player
					speed = 16f;
					inertia = 30f;
				}
				if (distanceToTargetPosition > 40f)
				{
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToTargetPosition.Normalize();
					vectorToTargetPosition *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToTargetPosition) / inertia;
				}
				else if (projectile.velocity == Vector2.Zero)
				{
					// If there is a case where it's not moving at all, give it a little "poke"
					projectile.velocity.X = -0.15f;
					projectile.velocity.Y = -0.05f;
				}
			}
            else
            {
				if (distanceToTargetPosition > 600f)
				{
					// Speed up the minion if it's away from the player
					speed = 24f;
					inertia = 30f;
				}
				else
				{
					// Slow down the minion if closer to the player
					speed = 8f;
					inertia = 40f;
				}
				if (distanceToTargetPosition > 40f)
				{
					// The immediate range around the player (when it passively floats about)

					// This is a simple movement formula using the two parameters and its desired direction to create a "homing" movement
					vectorToTargetPosition.Normalize();
					vectorToTargetPosition *= speed;
					projectile.velocity = (projectile.velocity * (inertia - 1) + vectorToTargetPosition) / inertia;
				}
				else if (projectile.velocity == Vector2.Zero)
				{
					// If there is a case where it's not moving at all, give it a little "poke"
					projectile.velocity.X = -0.15f;
					projectile.velocity.Y = -0.05f;
				}
			}
            #endregion
            #region Animation and VFX
            //*
			int frameSpeed = 7;
			projectile.frameCounter++;
			if (projectile.frameCounter >= frameSpeed)
			{
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type])
				{
					projectile.frame = 0;
				}
                if (projectile.frame <= 2)
                {
					lightColor = new Vector3(255, 0, 0);
					brightness = 0.001f;
				}
                else if (projectile.frame >= 5 && projectile.frame <= 6)
                {
					lightColor = new Vector3(0, 255, 0);
					brightness = 0.001f;
				}
                else
                {
					brightness = 0;
				}
			}
			//*/
            projectile.rotation = projectile.velocity.X * 0.05f;
			Lighting.AddLight(projectile.Center, lightColor * brightness);
            if (foundTarget)
            {
				Vector2 ShootVel = targetCenter - projectile.Center;
				projectile.spriteDirection = Math.Sign(ShootVel.X);
			}
            else
            {
				projectile.spriteDirection = projectile.direction;
			}
			#endregion
		}

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			fallThrough = true;
			return true;
		}
	}
}