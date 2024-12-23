using System;
using System.Collections.Generic;
using Attrition.CharacterSelection.Characters;

namespace Attrition.CharacterSelection.Selection.Navigation
{
    public class CharacterSelector
    {
        private readonly IList<CharacterSelectionCharacterBehaviour> characters;
        private int currentCharacterIndex;
        private bool isNavigatingRight;

        public CharacterSelectionCharacterBehaviour CurrentSelection =>
            this.characters[this.currentCharacterIndex];

        public CharacterSelector(IList<CharacterSelectionCharacterBehaviour> characters)
        {
            this.characters = characters ?? throw new ArgumentNullException(nameof(characters));
        }

        public void Navigate(bool isNavigatingRight)
        {
            this.isNavigatingRight = isNavigatingRight;
            this.UpdateIndex();
        }

        private void UpdateIndex()
        {
            var charactersCount = this.characters.Count;

            this.currentCharacterIndex = this.isNavigatingRight
                ? (this.currentCharacterIndex + 1) % charactersCount
                : (this.currentCharacterIndex - 1 + charactersCount) % charactersCount;
        }
    }
}
