using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Attrition.Main_Menu.Settings
{
    public class FloatSettingUI : MonoBehaviour
    {
        [SerializeField] private FloatSetting setting;
        [Space]
        [SerializeField] private Slider slider;

        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private string valuePostfix;
        [SerializeField] private float increment;
        [SerializeField] private float displayMultiple;
        [SerializeField] private float minValue, maxValue;
        
        private void Awake()
        {
            this.slider.minValue = this.minValue / this.increment;
            this.slider.maxValue = this.maxValue / this.increment;
            this.slider.wholeNumbers = true;
            
            this.slider.onValueChanged.AddListener(this.OnSliderValueChanged);
            this.setting.ValueChanged += this.OnSettingOnValueChanged;
            
            this.slider.value = this.setting.Value / this.increment;
        }

        private void OnSliderValueChanged(float value)
        {
            this.setting.Value = value * this.increment;
        }

        private void OnSettingOnValueChanged(float value)
        {
            this.valueText.text = $"{value * this.displayMultiple}{this.valuePostfix}";
            this.slider.SetValueWithoutNotify(value / this.increment);
        }
    }
}
