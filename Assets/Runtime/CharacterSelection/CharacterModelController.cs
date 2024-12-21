using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Attrition.CharacterSelection
{
    [RequireComponent(typeof(CharacterIdentity))]
    public class CharacterModelController : MonoBehaviour
    {
        private struct ModelKeyValuePair
        {
            public CharacterClass Class;
            public Transform Transform;
        }
        
        [SerializeField]
        private Transform modelContainer;
        [SerializeField]
        private List<ModelKeyValuePair> models;
        private CharacterIdentity identity;
        
        private void Awake()
        {
            this.ResolveDependencies();
        }

        private void Reset()
        {
            this.ResolveDependencies();
        }

        private void OnEnable()
        {
            this.UpdateCharacterModel();
        }

        public void UpdateCharacterModel()
        {
            foreach (Transform child in this.modelContainer)
            {
                child.gameObject.SetActive(false);
            }
            
            var characterClass = this.identity.CharacterClass;
            
            if (characterClass == null)
            {
                return;
            }

            var model = this.models
                .Where(kvp => kvp.Class == this.identity.CharacterClass)
                .Select(kvp => kvp.Transform)
                .FirstOrDefault();
            
            if (model != null)
            {
                model.gameObject.SetActive(true);
            }
        }
        
        private void ResolveDependencies()
        {
            this.identity ??= this.GetComponent<CharacterIdentity>();
        }
    }
}
