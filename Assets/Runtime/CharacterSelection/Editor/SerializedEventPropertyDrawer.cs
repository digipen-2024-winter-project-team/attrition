using Attrition.CharacterSelection.Characters;
using Attrition.CharacterSelection.Selection;
using Attrition.Common.Events.SerializedEvents;
using Attrition.Runtime.Common.SerializedEvents.Editor;
using UnityEditor;

namespace Attrition.Runtime.CharacterSelection.Editor
{
    [CustomPropertyDrawer(typeof(SerializedEvent<CharacterSelectionCharacterBehaviour>))]
    public class SerializedEventCharacterSelectionCharacterBehaviourPropertyDrawer 
        : SerializedEventPropertyDrawer<CharacterSelectionCharacterBehaviour>
    {
    }
    
    [CustomPropertyDrawer(typeof(SerializedEvent<CharacterSelectionStateHandler.SelectionState>))]
    public class SerializedEventSelectionStatePropertyDrawer 
        : SerializedEventPropertyDrawer<CharacterSelectionStateHandler.SelectionState>
    {
    }
}
