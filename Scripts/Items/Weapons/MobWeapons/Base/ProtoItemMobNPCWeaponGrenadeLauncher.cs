namespace AtomicTorch.CBND.CoreMod.Items.Weapons.MobWeapons
{
    using System.Collections.Generic;
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.CharacterSkeletons;
    using AtomicTorch.CBND.CoreMod.Skills;
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.CBND.CoreMod.Systems.Physics;
    using AtomicTorch.CBND.CoreMod.Systems.Weapons;
    using AtomicTorch.CBND.CoreMod.Damage;
    using AtomicTorch.CBND.CoreMod.Helpers.Client;
    using AtomicTorch.CBND.CoreMod.Items.Ammo;
    using AtomicTorch.CBND.GameApi.Data.Characters;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Data.Physics;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Data.State;
    using AtomicTorch.CBND.GameApi.Data.Weapons;
    using AtomicTorch.CBND.GameApi.Scripting.Network;
    using AtomicTorch.GameEngine.Common.Primitives;
    using AtomicTorch.CBND.GameApi.Scripting.ClientComponents;
    using AtomicTorch.CBND.GameApi.ServicesClient.Components;

    public abstract class ProtoItemMobNPCWeaponGrenadeLauncher : ProtoItemWeaponRanged
    {
        public override DamageStatsComparisonPreset DamageStatsComparisonPreset
            => DamageStatsComparisonPresets.PresetRangedGrenades;
			
        public override ushort AmmoCapacity => 0;

        public override ushort VirtualAmmoCapacity => 10;

        public override double AmmoReloadDuration => 0;

        public override bool CanDamageStructures => true;

        public override CollisionGroup CollisionGroup => CollisionGroups.HitboxRanged;

        public override double DamageApplyDelay => 0;

        public override string Description => null;

        public override uint DurabilityMax => 0;

        public override bool IsLoopedAttackAnimation => false;

        public override string Name => this.ShortId;

        public override double ReadyDelayDuration => 0;

        public override (float min, float max) SoundPresetWeaponDistance
            => (SoundConstants.AudioListenerMinDistanceRangedShot,
                SoundConstants.AudioListenerMaxDistanceRangedShotFirearms);

        public override double SpecialEffectProbability =>
            1; // Must always be 1 for all animal weapons. Individual effects will be rolled in the effect function.

        protected override ProtoSkillWeapons WeaponSkill => null;

        protected override TextureResource WeaponTextureResource => null;

        public override void ClientSetupSkeleton(
            IItem item,
            ICharacter character,
            ProtoCharacterSkeleton protoCharacterSkeleton,
            IComponentSkeleton skeletonRenderer,
            List<IClientComponent> skeletonComponents)
        {
            // do nothing
        }

        public override bool SharedCanSelect(IItem item, ICharacter character, bool isAlreadySelected, bool isByPlayer)
        {
            return character.ProtoCharacter is IProtoCharacterMob;
        }

        public override void ClientOnFireModChanged(bool isFiring, uint shotsDone)
        {
            var character = ClientCurrentCharacterHelper.Character;
            var weaponState = ClientCurrentCharacterHelper.PrivateState.WeaponState;

            if (isFiring
                && !weaponState.ProtoWeapon.SharedCanFire(character, weaponState))
            {
                // cannot fire now
                weaponState.SharedSetInputIsFiring(false);
                return;
            }

            if (!isFiring)
            {
                weaponState.CustomTargetPosition = null;
                return;
            }

            var targetPosition = WeaponGrenadeLauncherHelper.ClientGetCustomTargetPosition();
            weaponState.CustomTargetPosition = targetPosition;
        }

        protected override void ClientInitialize(ClientInitializeData data)
        {
            base.ClientInitialize(data);

            // preload all the explosion spritesheets
            foreach (var ammoProto in this.CompatibleAmmoProtos)
            {
                if (!(ammoProto is IAmmoGrenade protoGrenade))
                {
                    continue;
                }

                foreach (var textureAtlasResource in protoGrenade.ExplosionPreset.SpriteAtlasResources)
                {
                    Client.Rendering.PreloadTextureAsync(textureAtlasResource);
                }
            }
        }

        protected abstract void PrepareProtoGrenadeLauncher(
            out IEnumerable<IProtoItemAmmo> compatibleAmmoProtos);

        protected sealed override void PrepareProtoWeaponRanged(
            out IEnumerable<IProtoItemAmmo> compatibleAmmoProtos,
            ref DamageDescription overrideDamageDescription)
        {
            this.PrepareProtoGrenadeLauncher(out compatibleAmmoProtos);
        }

        protected override void PrepareMuzzleFlashDescription(MuzzleFlashDescription description)
        {
            description.Set(MuzzleFlashPresets.GrenadeLauncher)
                       .Set(textureScreenOffset: (20, 11));
        }

        protected override ReadOnlySoundPreset<ObjectMaterial> PrepareSoundPresetHit()
        {
            return MaterialHitsSoundPresets.Ranged;
        }

        protected override ReadOnlySoundPreset<WeaponSound> PrepareSoundPresetWeapon()
        {
            return WeaponsSoundPresets.WeaponRanged;
        }

    }
}