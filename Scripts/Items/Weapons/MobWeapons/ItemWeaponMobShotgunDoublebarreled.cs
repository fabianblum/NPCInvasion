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
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.GameEngine.Common.Helpers;

    public class ItemWeaponMobShotgunDoublebarreled : ProtoItemMobNPCWeaponRanged
    {
        private static readonly double[] DamageRayAngleOffets
            = { -8, -7.5, -5, -5.5, -3, -3.5, 0, 3, 3.5, 0.2, 5, 5.5, 7.5, 8};
			
        public override ushort AmmoCapacity => 0;

        public override ushort VirtualAmmoCapacity => 10;

        public override double DamageApplyDelay => 0.1;

        public override double AmmoReloadDuration => 2;

        public override double CharacterAnimationAimingRecoilDuration => 0.45;

        public override double CharacterAnimationAimingRecoilPower => 1.3;

        public override string Description => "Old grandpa shotgun. Uses 12ga ammunition.";

        public override uint DurabilityMax => 0;

        public override double FireInterval => 1.8;

        public override string Name => "Double-barreled shotgun";

        public override double SpecialEffectProbability => 0.4;

        public override string CharacterAnimationAimingName => "WeaponRifleAiming";

        public virtual string CharacterAnimationAimingRecoilName => "WeaponRifleShooting";

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
            return 10 * (RandomHelper.NextDouble() - 0.5);
        }

        protected override WeaponFireScatterPreset PrepareFireScatterPreset()
        {
            return new(DamageRayAngleOffets);
        }

        protected override WeaponFirePatternPreset PrepareFirePatternPreset()
        {
            return new(
                initialSequence: new[] { 0.0, 0.0},
                cycledSequence: new[] { 3.0, 3.2});
        }

        protected override WeaponFireTracePreset PrepareFireTracePreset()
        {
            return WeaponFireTracePresets.Pellets;
        }

        protected override void PrepareMuzzleFlashDescription(MuzzleFlashDescription description)
        {
            description.Set(MuzzleFlashPresets.ModernShotgun)
                       .Set(textureScreenOffset: (49, 7));
        }

        protected override void PrepareProtoWeaponRanged(
            out IEnumerable<IProtoItemAmmo> compatibleAmmoProtos,
            ref DamageDescription overrideDamageDescription)
        {
            compatibleAmmoProtos = GetAmmoOfType<IAmmoCaliber12g>();

            var damageDistribution = new DamageDistribution()
                                     .Set(DamageType.Impact, 0.6)
									 .Set(DamageType.Kinetic, 0.4);

            overrideDamageDescription = new DamageDescription(
                damageValue: 35,
                armorPiercingCoef: 0.6,
                finalDamageMultiplier: 1.0,
                rangeMax: 8,
                damageDistribution: damageDistribution);
        }

        protected override void ServerOnSpecialEffect(ICharacter damagedCharacter, double damage)
        {
            if (RandomHelper.RollWithProbability(0.7))
            {
                damagedCharacter.ServerAddStatusEffect<StatusEffectPain>(intensity: 0.5);
            }

            if (RandomHelper.RollWithProbability(0.15))
            {
				damagedCharacter.ServerAddStatusEffect<StatusEffectPain>(intensity: 0.8);
            }
        }

        protected override ReadOnlySoundPreset<WeaponSound> PrepareSoundPresetWeapon()
        {
            return WeaponsSoundPresets.WeaponRangedShotgunDoublebarreled;
        }
    }
}