namespace AtomicTorch.CBND.CoreMod.CharacterSkeletons
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Media;
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.SoundPresets;
    using AtomicTorch.CBND.GameApi.Resources;
    using AtomicTorch.CBND.GameApi.Scripting;
    using AtomicTorch.CBND.GameApi.ServicesClient.Components;
    using AtomicTorch.GameEngine.Common.Extensions;
    using AtomicTorch.GameEngine.Common.Primitives;

    public abstract class ProtoCharacterSkeletonNPC : ProtoCharacterSkeletonAnimal
    {
		
        public override void ClientResetItemInHand(IComponentSkeleton skeletonRenderer)
        {
            skeletonRenderer.SetAttachmentSprite(this.SlotNameItemInHand,
                                                 attachmentName: "WeaponMelee",
                                                 textureResource: null);
            skeletonRenderer.SetAttachmentSprite(this.SlotNameItemInHand,
                                                 attachmentName: "WeaponRanged",
                                                 textureResource: null);
            skeletonRenderer.SetAttachment(this.SlotNameItemInHand, attachmentName: "WeaponRanged");
        }
    }
}