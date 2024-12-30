using System;
using UnityEngine;
using UnityEngine.Events;

namespace Attrition.Main_Menu.Settings 
{
    public abstract class Setting<TValue> : ScriptableObject
    {
        [SerializeField] private TValue defaultValue;
        [SerializeField] private UnityEvent<TValue> valueChanged;

        public event Action<TValue> ValueChanged;

        public TValue DefaultValue => this.defaultValue;

        public TValue Value
        {
            get => this.ToValue(PlayerPrefs.GetFloat(this.name, this.ToFloat(this.defaultValue)));

            set
            {
                this.SetValueWithoutNotify(value);
                this.InvokeValueChanged();
            }
        }

        public void SetValueWithoutNotify(TValue value)
        {
            PlayerPrefs.SetFloat(this.name, this.ToFloat(value));
        }

        public void InvokeValueChanged()
        {
            var value = this.Value;

            this.ValueChanged?.Invoke(value);
            this.valueChanged.Invoke(value);
        }

        public void ResetToDefault()
        {
            PlayerPrefs.DeleteKey(this.name);

            this.InvokeValueChanged();
        }

        protected abstract TValue ToValue(float value);
        protected abstract float ToFloat(TValue value);

        private void OnValidate()
        {
            this.InvokeValueChanged();
        }
    }
}
