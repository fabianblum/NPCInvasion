using AtomicTorch.CBND.CoreMod.Systems.Weapons;
using AtomicTorch.CBND.GameApi.Data.Characters;
using AtomicTorch.CBND.GameApi.Data.Items;
using System;
using System.Threading.Tasks;

namespace AtomicTorch.CBND.CoreMod.Characters
{
    public abstract class ProtoCharacterNPCBA : ProtoCharacterNPC
    {
        private Task reloadTimer;

        private bool isReloading = false;

        public virtual double RetreatDistance => 7;

        public virtual double EnemyToCloseDistance => 7;

        public virtual double EnemyToFarDistance => 14;

        public virtual bool RetreatWhenReloading => true;

        public virtual double RetreatingHealthPercentage => 50.0;

        protected override void ServerUpdateMob(ServerUpdateData data)
        {
            var character = data.GameObject;


            var currentStats = data.PublicState.CurrentStats;


            var closestTarget = ServerNpcAiHelper.GetClosestTargetPlayer(character);

            var privateState = character.GetPrivateState<CharacterMobPrivateState>();
            var weaponState = privateState.WeaponState;
            var weapon = weaponState.ProtoWeapon;

            int reloadTime = Convert.ToInt32(Math.Floor(weapon.AmmoReloadDuration)) * 1000; 
            int fireTime = Convert.ToInt32(Math.Floor(weapon.VirtualAmmoCapacity * weapon.FireInterval)) * 1000;

            if (closestTarget is not null && !closestTarget.IsNpc)
            {
                if (reloadTimer is null && weaponState.SharedGetInputIsFiring())
                {
                    reloadTimer = Task.Delay(fireTime).ContinueWith(t => setIsReloading(true));
                    Task.Delay(fireTime).ContinueWith(t => playSound(weaponState, false, character));
                }

                if (reloadTimer is not null && isReloading)
                {
                    Task.Delay(reloadTime).ContinueWith(t => setIsReloading(false));
                    Task.Delay(reloadTime).ContinueWith(t => playSound(weaponState, true, character));
                    reloadTimer = null;
                }
            }

            ServerNpcAiHelper.ProcessAggressiveAi(
                character,
                targetCharacter: closestTarget,
                isRetreating: currentStats.HealthCurrent < currentStats.HealthMax * (RetreatingHealthPercentage / 100) || RetreatWhenReloading,
                isRetreatingForHeavyVehicles: false,
                distanceRetreat: RetreatDistance,
                distanceEnemyTooClose: EnemyToCloseDistance,
                distanceEnemyTooFar: EnemyToFarDistance,
                movementDirection: out var movementDirection,
                rotationAngleRad: out var rotationAngleRad,
                attackFarOnlyIfAggro: false,
                isReloading: isReloading);

            this.ServerSetMobInput(character, movementDirection, rotationAngleRad);
        }

        protected void setIsReloading(bool IsReloading)
        {
            isReloading = IsReloading;
        }

        protected void playSound(WeaponState weaponState, bool reloadFinish, ICharacter character)
        {

            if (reloadFinish)
            {
                WeaponAmmoSystem.playReloadSoundFinished(weaponState, character);
            }
            else
            {
                WeaponAmmoSystem.playReloadSound(weaponState, character);
            }
        }
    }
}
        