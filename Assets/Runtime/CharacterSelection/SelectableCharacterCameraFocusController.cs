using Unity.Cinemachine;

namespace Attrition.CharacterSelection
{
    public class SelectableCharacterCameraFocusController
    {
        private readonly CinemachineCamera focusCamera;

        public SelectableCharacterCameraFocusController(CinemachineCamera camera)
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
