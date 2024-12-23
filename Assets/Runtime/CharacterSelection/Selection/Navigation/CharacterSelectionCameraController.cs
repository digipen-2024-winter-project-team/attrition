using System;
using Attrition.CharacterSelection.Characters;
using Attrition.Common.Cinemachine;
using Unity.Cinemachine;

namespace Attrition.CharacterSelection.Selection.Navigation
{
    public class CharacterSelectionCameraController
    {
        private readonly CinemachineDollyTweener dollyTweener;

        public CharacterSelectionCameraController(CinemachineCamera dollyCamera)
        {
            if (dollyCamera == null)
            {
                throw new ArgumentNullException(nameof(dollyCamera));
            }

            this.dollyTweener = dollyCamera.GetComponent<CinemachineDollyTweener>()
                                ?? throw new InvalidOperationException("Missing CinemachineDollyTweener component.");
        }

        public void MoveTo(CharacterSelectionCharacterBehaviour targetCharacter, bool isNavigatingRight)
        {
            if (targetCharacter == null)
            {
                throw new ArgumentNullException(nameof(targetCharacter));
            }

            this.dollyTweener.MoveToPosition(targetCharacter.BrowseFollowTarget.position, isNavigatingRight);
        }
    }
}
