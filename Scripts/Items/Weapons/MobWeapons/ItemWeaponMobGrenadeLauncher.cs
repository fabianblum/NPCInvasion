namespace AtomicTorch.CBND.CoreMod.Items.Weapons.MobWeapons
{
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects;
    using AtomicTorch.CBND.CoreMod.CharacterStatusEffects.Debuffs;
    using AtomicTorch.CBND.CoreMod.Items.Ammo;
    using AtomicTorch.CBND.CoreMod.Items.Weapons;
    using AtomicTorch.CBND.CoreMod.Systems.Weapons;
    using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.GameApi.Data.Weapons;
    using AtomicTorch.CBND.GameApi.Data.World;
    using AtomicTorch.CBND.GameApi.Data.State;
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.GameEngine.Common.Helpers;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Explosives;
    using AtomicTorch.CBND.CoreMod.Damage;

    public class ItemWeaponMobGrenadeLauncher : ProtoItemMobNPCWeaponRanged
    {
        public override DamageStatsComparisonPreset DamageStatsComparisonPreset
            => DamageStatsComparisonPresets.PresetRangedGrenades;

        public override ushort AmmoCapacity => 0;

        public override ushort VirtualAmmoCapacity => 10;

        public override double AmmoReloadDuration => 2.5; // lower
    
        //public override double DamageRadius => 2.1;

        public override string Description => "Single grenade launcher for NPCs.";
		
		public override double CharacterAnimationAimingRecoilDuration => 0.6;

        public override double CharacterAnimationAimingRecoilPower => 1.2;

        public override double DamageApplyDelay => 0.3;

        public override double DamageMultiplier => 1.1;

        public override double FireAnimationDuration => 0.1;
		
		public override uint DurabilityMax => 0;

        public override bool IsLoopedAttackAnimation => false;

        public override double FireInterval => 3;
		
		public override double SpecialEffectProbability => 0.2;

        public override string Name => "Grenade launcher";

        public override void SharedOnHit(
            WeaponFinalCache weaponCache,
            IWorldObject damagedObject,
            double damage,
            WeaponHitData hitData,
            out bool isDamageStop)
        {
            base.SharedOnHit(weaponCache,
                             damagedObject,
                             damage,
                             hitData,
                             out isDamageStop);
        }
        public override double SharedUpdateAndGetFirePatternCurrentSpreadAngleDeg(WeaponState state)
        {
            // angle variation within x degrees
            return 25 * (RandomHelper.NextDouble() - 0.5);
        }

        protected override WeaponFireTracePreset PrepareFireTracePreset()
        {
            return WeaponFireTracePresets.Grenade;
        }

        protected override void PrepareMuzzleFlashDescription(MuzzleFlashDescription description)
        {
            description.Set(MuzzleFlashPresets.GrenadeLauncher)
                       .Set(textureScreenOffset: (20, 11));
        }

        protected override void PrepareProtoWeaponRanged(
			out IEnumerable<IProtoItemAmmo> compatibleAmmoProtos,
			ref DamageDescription overrideDamageDescription)
        {
            compatibleAmmoProtos = GetAmmoOfType<IAmmoGrenadeForGrenadeLauncher>(); ;

            var damageDistribution = new DamageDistribution()
                                     .Set(DamageType.Explosion, 1.0);

            overrideDamageDescription = new DamageDescription(
                damageValue: 75,
                armorPiercingCoef: 0.8,
                finalDamageMultiplier: 1,
                rangeMax: 10,
                damageDistribution: damageDistribution);
        }

        protected override void ServerOnSpecialEffect(ICharacter damagedCharacter, double damage)
        {
            // 60% chance to add heat
            if (RandomHelper.RollWithProbability(0.60))
            {
                damagedCharacter.ServerAddStatusEffect<StatusEffectHeat>(intensity: 0.7);
            }
        }

        protected override ReadOnlySoundPreset<WeaponSound> PrepareSoundPresetWeapon()
        {
            return WeaponsSoundPresets.WeaponGrenadeLauncher;
        }

    }
}