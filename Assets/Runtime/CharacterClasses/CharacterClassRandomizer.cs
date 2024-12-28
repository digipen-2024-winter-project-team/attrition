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
        private bool randomizeOnEnable = true;
        [SerializeField]
        private bool overwriteExisting = false;
        [SerializeField]
        private List<CharacterClass> availableClasses;
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
            var shouldRandomize = this.overwriteExisting || this.classBehaviour.CharacterClass == null;

            if (this.randomizeOnEnable && this.classBehaviour != null && shouldRandomize)
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
