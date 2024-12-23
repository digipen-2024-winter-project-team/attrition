using Unity.Cinemachine;

namespace Attrition.CharacterSelection.Characters
{
    public class CharacterSelectionCameraFocusController
    {
        private readonly CinemachineCamera focusCamera;

        public CharacterSelectionCameraFocusController(CinemachineCamera camera)
        {
            this.focusCamera = camera;
        }

        public void Focus()
        {
            this.focusCamera.gameObject.SetActive(true);
        }

        public void Unfocus()
        {
            this.focusCamera.gameObject.SetActive(false);
        }
    }
}
