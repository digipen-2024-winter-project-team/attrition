using UnityEngine;

namespace Attrition.Common.Containers
{
    /// <summary>
    /// A specialized container for managing a list of <see cref="GameObject"/> instances.
    /// Inherits the functionality of the generic <see cref="Container{T}"/> class.
    /// </summary>
    /// <remarks>
    /// This class provides the same functionality as the base container but is specifically 
    /// designed for handling Unity's <see cref="GameObject"/> type. Additional game-object-specific 
    /// behavior can be added by overriding or extending the base class methods.
    /// </remarks>
    public class GameObjectContainer : Container<GameObject>
    {
    }
}
