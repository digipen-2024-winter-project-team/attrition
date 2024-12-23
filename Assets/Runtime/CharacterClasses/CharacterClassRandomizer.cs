using System;
using System.Collections.Generic;
using Attrition.Common;
using Attrition.Common.Picking;
using Attrition.Common.Picking.Builder;
using UnityEngine;

namespace Attrition.CharacterClasses
{
    [DefaultExecutionOrder(ExecutionOrder.EntitySetup)]
    [RequireComponent(typeof(CharacterClassBehaviour))]
    public class CharacterClassRandomizer : MonoBehaviour
    {
        [SerializeField]
        private List<CharacterClass> availableClasses;
        private CharacterClassBehaviour classBehaviour;
        private Picker<CharacterClass> picker;

        private static List<CharacterClass> alreadyPicked;
        
        private void Awake()
        {
            this.classBehaviour ??= this.GetComponent<CharacterClassBehaviour>();
            
            var builder = new PickerBuilder<CharacterClass>();
            this.picker = builder
                .UseRandom()
                .ExcludeAnyIn(alreadyPicked)
                .Build();
        }

        private void OnEnable()
        {
            this.Randomize();
        }

        public void Randomize()
        {
            var picked = this.picker.PickFrom(this.availableClasses);
            alreadyPicked.Add(picked);

            this.classBehaviour.CharacterClass = picked;
        }
    }
}
