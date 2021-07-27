namespace AtomicTorch.CBND.CoreMod.Items.Special
{
    using System.Linq;
    using System.Threading.Tasks;
    using AtomicTorch.CBND.CoreMod.Items.Generic;
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Misc;
    using AtomicTorch.CBND.CoreMod.Systems.Notifications;
    using AtomicTorch.CBND.CoreMod.Systems.TeleportsSystem;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Data.State;
    using AtomicTorch.CBND.GameApi.Scripting.Network;
    using AtomicTorch.GameEngine.Common.Extensions;

    public abstract class ProtoItemSecretLocationUnlock<TObjectTeleport>
        : ProtoItemGeneric,
          IProtoItemUsableFromContainer
        where TObjectTeleport : ProtoObjectSecretLocationTeleport, new()
    {
        public const string Notification_AlreadyDiscoveredAllTeleports
            = "You have already discovered all of the teleport locations.";

        private Task<bool> lastTask;

        public string ItemUseCaption => ItemUseCaptions.Use;

        public override ushort MaxItemsPerStack => ItemStackSize.Small;

        protected override bool ClientItemUseFinish(ClientItemData data)
        {
            Logger.Important("ClientItemUseFinish ");
            this.ClientUseAsync(data.Item);
            return true;
        }

        protected override string GenerateIconPath()
        {
            return "Items/Special/" + this.GetType().Name;
        }

        protected override ReadOnlySoundPreset<ItemSound> PrepareSoundPresetItem()
        {
            return base.PrepareSoundPresetItem()
                       .Clone()
                       .Replace(ItemSound.Use, "Items/Special/TeleportUnlock/Use");
        }

        private async void ClientUseAsync(IItem item)
        {
            
            if (this.lastTask != null
                && this.lastTask.Status == TaskStatus.Running)
            {
                Logger.Important("last task status " + this.lastTask.Status);
                return;
            }

            this.lastTask = this.CallServer(_ => _.ServerRemote_UseItem(item));
            var result = await this.lastTask;
            Logger.Important("Result " + result);
            if (!result)
            {
                NotificationSystem.ClientShowNotification(
                    title: null,
                    message: Notification_AlreadyDiscoveredAllTeleports,
                    icon: this.Icon);
            }
        }

        [RemoteCallSettings(timeInterval: 1)]
        private bool ServerRemote_UseItem(IItem item)
        {
            var character = ServerRemoteContext.Character;
            this.ServerValidateItemForRemoteCall(item, character);

            var teleportObjects = Server.World
                                        .GetStaticWorldObjectsOfProto<TObjectTeleport>()
                                        .ToList();
            Logger.Important("[SL] Count before " + teleportObjects.Count, character);
            var knownTeleportPositions = TeleportsSystem.ServerGetDiscoveredTeleports(character);
            Logger.Important("[SL] knownTeleportPositions " + knownTeleportPositions.Count);
            teleportObjects.RemoveAll(t => knownTeleportPositions.Contains(t.TilePosition));

            Logger.Important("[SL] Count left " + teleportObjects.Count);
            if (teleportObjects.Count == 0)
            {
                // no teleports to reveal
                return false;
            }

            // reveal a single random teleport
            Logger.Important("[SL] Count left " + teleportObjects.Count);
            TeleportsSystem.ServerAddTeleportToDiscoveredList(character,
                                                              teleportObjects.TakeByRandom());

            Server.Items.SetCount(item, item.Count - 1);
            NotificationSystem.ServerSendItemsNotification(character, protoItem: this, deltaCount: -1);
            Logger.Important("[SL] Discovered a teleport with " + this.ShortId, character);
            return true;
        }

    }
}