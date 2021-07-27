namespace AtomicTorch.CBND.CoreMod.Characters.Mobs
{
    using AtomicTorch.CBND.CoreMod.CharacterSkeletons;
    using AtomicTorch.CBND.CoreMod.Items.Food;
    using AtomicTorch.CBND.CoreMod.Items.Generic;
	using AtomicTorch.CBND.CoreMod.Items.Medical;
	using AtomicTorch.CBND.CoreMod.Items.Equipment;
	using AtomicTorch.CBND.CoreMod.Items.Equipment.Assault;
	using AtomicTorch.CBND.CoreMod.Items.Devices;
    using AtomicTorch.CBND.CoreMod.Items.Weapons.MobWeapons;
    using AtomicTorch.CBND.CoreMod.Items.Weapons.Ranged;
    using AtomicTorch.CBND.CoreMod.Items.Ammo;
	using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.CoreMod.Objects;
    using AtomicTorch.CBND.CoreMod.Skills;
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.CBND.CoreMod.Stats;
    using AtomicTorch.CBND.CoreMod.Systems.Droplists;
    using AtomicTorch.CBND.GameApi.Data.World;


    public class MobNPC_BA_Shotgunner : ProtoCharacterNPCBA
    {
		public override string Name => "Shotgunner";
		
		public override ObjectMaterial ObjectMaterial => ObjectMaterial.HardTissues;
        
		public override float CharacterWorldHeight => 1.5f;
		
		public override float CharacterWorldWeaponOffsetRanged => 0.4f;
        
        public override double StatMoveSpeed => 3.4;

        public override double StatMoveSpeedRunMultiplier => 1.3;
		
		public override bool AiIsRunAwayFromHeavyVehicles => false;
		
		public override double MobKillExperienceMultiplier => 1.0;
        
		public override double StatDefaultHealthMax => 240;
		
		public override double StatHealthRegenerationPerSecond => 40.0 / 60.0; // (stimpack alike) 
		
        public override bool IsAvailableInCompletionist => false;

        public override double RetreatDistance => 5;

        public override double EnemyToCloseDistance => 6;

        public override double EnemyToFarDistance => 15;

        protected override void FillDefaultEffects(Effects effects)
        {
            base.FillDefaultEffects(effects);

            effects.AddValue(this, StatName.DefenseImpact,     0.6);
            effects.AddValue(this, StatName.DefenseKinetic,    0.8);
			effects.AddValue(this, StatName.DefenseExplosion, 0.6);
			effects.AddValue(this, StatName.DefenseChemical,   0.3);

        }
		
	   protected override void PrepareProtoCharacterMob(
            out ProtoCharacterSkeleton skeleton,
            ref double scale,
            DropItemsList lootDroplist)
        {
            skeleton = GetProtoEntity<CharacterSkeletons.NPC_BA_Shotgunner>();

            // primary loot
			lootDroplist.Add(
				new DropItemsList(outputs: 1, outputsRandom: 1)
						
					.Add<ItemAmmo12gaSlugs>(count: 12, countRandom: 18, weight: 15)
					.Add<ItemCigarettes>(count: 1, countRandom: 1, weight: 6)
					.Add<ItemHemostatic>(count: 1,	countRandom: 1,	weight: 5)
					.Add<ItemMeatJerky>(count: 1, weight: 1)
					.Add<ItemMedkit>(count: 1, weight: 1));
					
			// extra loot from skill
            lootDroplist.Add(
                condition: SkillSearching.ServerRollExtraLoot,
                nestedList:
                new DropItemsList(outputs: 1, outputsRandom: 1)
                
					.Add<ItemShotgunDoublebarreled>(count: 1, weight: 1)
					.Add<ItemAssaultArmor>(count: 1, probability: 1 / 5.0));
        }

        protected override void ServerInitializeCharacterMob(ServerInitializeData data)
        {
            base.ServerInitializeCharacterMob(data);

            var weaponProto = GetProtoEntity<ItemWeaponMobShotgunDoublebarreled>();
            data.PrivateState.WeaponState.SharedSetWeaponProtoOnly(weaponProto);
            data.PublicState.SharedSetCurrentWeaponProtoOnly(weaponProto);

        }

		protected override void ServerOnAggro(ICharacter characterMob, ICharacter characterToAggro)
		{
            // cannot auto-aggro
        }
		
	}
}