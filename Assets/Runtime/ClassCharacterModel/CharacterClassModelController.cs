using System.Collections.Generic;
using System.Linq;
using Attrition.CharacterClasses;
using Attrition.Common;
using Attrition.Common.SerializedEvents;
using UnityEngine;

namespace Attrition.ClassCharacterModel
{
    /// <summary>
    /// Manages character models associated with different character classes.
    /// This class provides functionality to enable, disable, and retrieve models
    /// corresponding to a specified character class.
    /// </summary>
    public class CharacterClassModelController
    {
        private readonly Dictionary<CharacterClass, Animator> dictionary;
        private readonly Transform modelContainer;
        private readonly SerializedEvent<Animator> modelUpdated;

        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterClassModelController"/> class.
        /// </summary>
        /// <param name="models">A list of class model data containing character classes and their associated animators.</param>
        /// <param name="modelContainer">The container transform holding all the model objects.</param>
        public CharacterClassModelController(
            Transform modelContainer,
            List<CharacterClassModelBehaviour.ClassModelData> models,
            SerializedEvent<Animator> modelUpdated)
        {
            this.modelContainer = modelContainer;
            this.modelUpdated = modelUpdated;

            // Create a dictionary for fast lookup by character class
            this.dictionary = models
                .Where(model => model.Class != null && model.Animator != null)
                .ToDictionary(model => model.Class, model => model.Animator);
        }

        /// <summary>
        /// Activates the character model corresponding to the given character class
        /// and disables all other models.
        /// </summary>
        /// <param name="characterClass">The character class whose model should be activated.</param>
        /// <remarks>
        /// If the given character class does not have an associated model,
        /// all models will remain disabled.
        /// </remarks>
        public void UpdateActiveModel(CharacterClass characterClass)
        {
            this.DisableAllModels();

            var modelData = this.FindModelData(characterClass);
            if (modelData == null || modelData.Animator == null)
            {
                return;
            }

            this.EnableModel(modelData.Animator);
            this.modelUpdated?.Invoke(modelData.Animator);
        }

        /// <summary>
        /// Retrieves the model data associated with the specified character class.
        /// </summary>
        /// <param name="characterClass">The character class to search for.</param>
        /// <returns>
        /// A <see cref="CharacterClassModelBehaviour.ClassModelData"/> object containing the
        /// class and animator, or <c>null</c> if no matching model data is found.
        /// </returns>
        public CharacterClassModelBehaviour.ClassModelData FindModelData(CharacterClass characterClass)
        {
            if (characterClass == null || !this.dictionary.TryGetValue(characterClass, out var animator))
            {
                return null;
            }

            return new()
            {
                Class = characterClass,
                Animator = animator
            };
        }

        /// <summary>
        /// Disables all models contained within the model container.
        /// </summary>
        /// <remarks>
        /// This method iterates over all child objects of the model container
        /// and deactivates their <see cref="GameObject"/>.
        /// </remarks>
        private void DisableAllModels()
        {
            foreach (Transform child in this.modelContainer)
            {
                child.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Enables the model associated with the given animator.
        /// </summary>
        /// <param name="animator">The animator of the model to activate.</param>
        /// <remarks>
        /// This method activates the <see cref="GameObject"/> associated with the given animator.
        /// If the animator is <c>null</c>, the method does nothing.
        /// </remarks>
        private void EnableModel(Animator animator)
        {
            if (animator != null)
            {
                animator.gameObject.SetActive(true);
            }
        }
    }
}
