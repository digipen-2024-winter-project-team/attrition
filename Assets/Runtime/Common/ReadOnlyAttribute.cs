using UnityEngine;

namespace Attrition.Common
{
    /// <summary>
    /// An attribute used to mark fields in Unity as read-only in the Inspector.
    /// </summary>
    public class ReadOnlyAttribute : PropertyAttribute
    {
        public ReadOnlyAttribute()
        {
        }
    }
}
