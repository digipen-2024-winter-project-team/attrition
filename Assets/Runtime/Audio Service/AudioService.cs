using Attrition.Game_Services;
using Attrition.Main_Menu.Settings;
using UnityEngine;
using UnityEngine.Audio;

namespace Attrition.Audio_Service
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
            Setup(this.masterVolume, this.masterVolumeParameter);
            Setup(this.musicVolume, this.musicVolumeParameter);
            Setup(this.soundsVolume, this.soundsVolumeParameter);

            void Setup(FloatSetting setting, string parameter) => setting.ValueChanged +=
                value => this.mixer.SetFloat(parameter, value == 0 ? -80f : Mathf.Log10(value) * 20f);
        }

        protected override void Start()
        {
            this.masterVolume.InvokeValueChanged();
            this.musicVolume.InvokeValueChanged();
            this.soundsVolume.InvokeValueChanged();
        }
    }
}
