using System;
using System.Collections.Generic;
using Attrition.Common;
using Attrition.Common.Picking;
using Attrition.Common.Picking.Builder;
using UnityEngine;

namespace Attrition.CharacterClasses
{
    [DefaultExecutionOrder(10)]
    [RequireComponent(typeof(CharacterClassBehaviour))]
    public class CharacterClassRandomizer2 : MonoBehaviour
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
                .UniqueFrom(this.availableClasses)
                .UniqueFrom(alreadyPicked)
                .Build();
        }

        private void OnEnable()
        {
            this.Randomize();
        }

        public void Randomize()
        {
            var picked = this.picker.Pick(this.availableClasses);
            alreadyPicked.Add(picked);

            this.classBehaviour.CharacterClass = picked;
        }
    }
}
