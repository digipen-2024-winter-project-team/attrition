using System;
using System.Collections.Generic;
using System.Linq;
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
        private bool randomizeOnEnable = true;
        [SerializeField]
        private bool overwriteExisting = false;
        [SerializeField]
        private List<CharacterClass> availableClasses;
        private CharacterClassBehaviour classBehaviour;
        private Picker<CharacterClass> picker;
        
        private static readonly List<CharacterClass> AlreadyPicked = new();
        private static bool wasCleared;

        private void Awake()
        {
            this.classBehaviour ??= this.GetComponent<CharacterClassBehaviour>();
        }

        private void OnEnable()
        {
            wasCleared = false;
            
            var builder = new PickerBuilder<CharacterClass>();
            this.picker = builder
                .UseRandom()
                .ExcludeAnyIn(AlreadyPicked)
                .Build();
            
            var shouldRandomize = this.overwriteExisting || this.classBehaviour.CharacterClass == null;

            if (this.randomizeOnEnable && this.classBehaviour != null && shouldRandomize)
            {
                this.Randomize();
            }
        }

        private void OnDisable()
        {
            // So this is a bit gross. We want to share state between all randomizers so that each one can pick
            // a unique class. Ideally this should be broken into a separate (Scriptable?) object that can be shared
            // between all randomizers. For now, I'm using statics.
            // 
            // The problem is that static data is shared between instances of the same type - across the entire
            // application lifecycle. This means that we need to carefully manage this static state to ensure that
            // when the selection menu is entered again, we don't have carry-over from the previous selection.
            
            if (!wasCleared)
            {
                AlreadyPicked.Clear();
                wasCleared = true;
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
