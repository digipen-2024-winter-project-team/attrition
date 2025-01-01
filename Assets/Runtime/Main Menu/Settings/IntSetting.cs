using UnityEngine;

namespace Attrition.Main_Menu.Settings 
{
    [CreateAssetMenu(menuName = "Attrition/Settings/Int")]
    public class IntSetting : Setting<int> 
    {
        protected override float ToFloat(int value) => value;

        protected override int ToValue(float value) => (int)value;
    }
}
