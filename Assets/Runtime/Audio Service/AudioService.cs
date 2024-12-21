using Attrition.GameServices;
using Attrition.MainMenu.Settings;
using UnityEngine;
using UnityEngine.Audio;

namespace Attrition.AudioService
{
    [CreateAssetMenu(menuName = "Attrition/Services/Audio Service")]
    public class AudioService : GameService
    {
        [SerializeField] private FloatSetting masterVolume;
        [SerializeField] private FloatSetting musicVolume;
        [SerializeField] private FloatSetting soundsVolume;
        [SerializeField] private string masterVolumeParameter;
        [SerializeField] private string musicVolumeParameter;
        [SerializeField] private string soundsVolumeParameter;
        
        [SerializeField] private AudioMixer mixer;
        
        protected override void Initialize()
        {
            Setup(masterVolume, masterVolumeParameter);
            Setup(musicVolume, musicVolumeParameter);
            Setup(soundsVolume, soundsVolumeParameter);

            void Setup(FloatSetting setting, string parameter) => setting.ValueChanged +=
                value => mixer.SetFloat(parameter, value == 0 ? -80f : Mathf.Log10(value) * 20f);
        }

        protected override void Start()
        {
            masterVolume.InvokeValueChanged();
            musicVolume.InvokeValueChanged();
            soundsVolume.InvokeValueChanged();
        }
    }
}
