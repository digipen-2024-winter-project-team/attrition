using System;
using Attrition.Common.ScriptableVariables.ComponentTypes;
using Attrition.Common.ScriptableVariables.DataTypes;
using TMPro;
using UnityEngine;

namespace Attrition.PlayerCharacter.Health
{
    public class PlayerDebugHealthUI : MonoBehaviour
    {
        [SerializeField] private FloatVariable hitpoints;
        [SerializeField] private FloatVariable deathTimer;
        [SerializeField] private BoolVariable invincible;
        [SerializeField] private TextMeshProUGUI textMesh;
        [SerializeField] private RectTransform uiParent;
        [SerializeField] private GameObjectVariable uiAnchor;
        [SerializeField] private CameraVariable mainCamera;
        
        private void LateUpdate()
        {
            string health = hitpoints.Value > 0
                ? $"{hitpoints.Value:0} HP"
                : $"{deathTimer.Value:0.00}s";
            
            textMesh.text = $"<mspace=0em>{health}\n{(invincible.Value ? "Invincible" : "Vulnerable")}";
            textMesh.color = invincible.Value
                ? Color.red
                : Color.white;
            
            uiParent.localPosition = GetUVPosition(uiAnchor.Value.gameObject.transform.position) * ((RectTransform)uiParent.parent).rect.size;
            
            Vector2 GetUVPosition(Vector3 position) =>
                (Vector2)mainCamera.Value.WorldToViewportPoint(position) - Vector2.one / 2f;
        }
    }
}
