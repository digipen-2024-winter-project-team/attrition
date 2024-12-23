using System;
using System.Collections.Generic;
using Attrition.Common;
using Attrition.Common.Picking;
using Attrition.Common.Picking.Builder;
using UnityEngine;
using UnityEngine.Serialization;

namespace Attrition.CharacterClasses
{
    [DefaultExecutionOrder(ExecutionOrder.EntitySetup)]
    [RequireComponent(typeof(CharacterClassBehaviour))]
    public class CharacterClassRandomizer : MonoBehaviour
    {
        [SerializeField]
        private bool randomizeOnEnable = true;
        [SerializeField]
        private List<CharacterClass> availableClasses;
        [FormerlySerializedAs("shouldRandomizeOnEnable")]
        private CharacterClassBehaviour classBehaviour;
        private Picker<CharacterClass> picker;

        private static readonly List<CharacterClass> AlreadyPicked = new();
        
        private void Awake()
        {
            this.classBehaviour ??= this.GetComponent<CharacterClassBehaviour>();
            
            var builder = new PickerBuilder<CharacterClass>();
            this.picker = builder
                .UseRandom()
                .ExcludeAnyIn(AlreadyPicked)
                .Build();
        }

        private void OnEnable()
        {
            if (this.randomizeOnEnable && this.classBehaviour != null && this.classBehaviour.CharacterClass != null)
            {
                this.Randomize();
            }
        }

        public void Randomize()
        {
            var picked = this.picker.PickFrom(this.availableClasses);
            AlreadyPicked.Add(picked);
            
            this.classBehaviour.SetClass(picked);
        }
    }
}
