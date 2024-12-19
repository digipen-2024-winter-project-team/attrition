using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Attrition.MainMenu.Settings
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
            slider.minValue = minValue / increment;
            slider.maxValue = maxValue / increment;
            slider.wholeNumbers = true;
            
            slider.onValueChanged.AddListener(OnSliderValueChanged);
            setting.ValueChanged += OnSettingOnValueChanged;
            
            slider.value = setting.Value / increment;
        }

        private void OnSliderValueChanged(float value)
        {
            setting.Value = value * increment;
        }

        private void OnSettingOnValueChanged(float value)
        {
            valueText.text = $"{value * displayMultiple}{valuePostfix}";
            slider.SetValueWithoutNotify(value / increment);
        }
    }
}
