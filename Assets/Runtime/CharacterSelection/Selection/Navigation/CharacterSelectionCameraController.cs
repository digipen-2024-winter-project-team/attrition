using System;
using Attrition.CharacterSelection.Characters;
using Attrition.Common.Cinemachine;
using Unity.Cinemachine;

namespace Attrition.CharacterSelection.Selection.Navigation
{
    public class CharacterSelectionCameraController
    {
        private readonly CharacterSelectionController controller;
        private readonly CinemachineDollyTweener dollyTweener;

        public CharacterSelectionCameraController(CharacterSelectionController controller, CinemachineCamera dollyCamera)
        {
            if (dollyCamera == null)
            {
                throw new ArgumentNullException(nameof(dollyCamera));
            }

            this.controller = controller;

            this.dollyTweener = dollyCamera.GetComponent<CinemachineDollyTweener>()
                                ?? throw new InvalidOperationException("Missing CinemachineDollyTweener component.");

            
            this.UpdateAnimationParameters();
        }

        public void MoveTo(CharacterSelectionCharacterBehaviour targetCharacter, bool isNavigatingRight)
        {
            if (targetCharacter == null)
            {
                throw new ArgumentNullException(nameof(targetCharacter));
            }

            this.dollyTweener.MoveToPosition(targetCharacter.CycleFollowTarget.position, isNavigatingRight);
        }

        private void UpdateAnimationParameters()
        {
            this.dollyTweener.DollyAnimation.Duration = this.controller.CycleDuration;
        }
    }
}
