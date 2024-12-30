using System;
using UnityEngine;

namespace Attrition.Names.Generation
{
    [DefaultExecutionOrder(10)]
    [RequireComponent(typeof(NameBehaviour))]
    public class GenerateRandomNameOnEnableBehaviour : MonoBehaviour
    {
        [SerializeField]
        private NameData nameData;
        private NameBehaviour nameBehaviour;

        private void Awake()
        {
            this.GetDependencies();
        }

        private void Reset()
        {
            this.GetDependencies();
        }

        private void OnEnable()
        {
            var nameGenerator = new NameGenerator(this.nameData, DateTime.Now.Millisecond + this.GetInstanceID());
            var generatedName = nameGenerator.GenerateName();

            this.nameBehaviour.DisplayName = generatedName;
        }

        private void GetDependencies()
        {
            this.nameBehaviour ??= this.GetComponent<NameBehaviour>();
        }
    }
}
