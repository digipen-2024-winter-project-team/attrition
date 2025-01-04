using Attrition.Camera;
using Attrition.Common;
using Attrition.Common.ScriptableVariables.ComponentTypes;
using UnityEngine;

namespace Attrition.PlayerCharacter.Targeting
{
    public class PlayerTargetingUI : MonoBehaviour
    {
        [SerializeField] private GameObjectVariable targetGameObject;
        [SerializeField] private CameraVariable mainCamera;
        [SerializeField] private RectTransform indicator;
        [SerializeField] private float indicatorSpeed;
        
        private Vector2 indicatorPosition;
        private Vector2 indicatorVelocity;
        
        private void OnEnable()
        {
            targetGameObject.ValueChanged += TargetGameObjectOnValueChanged;
        }

        private void OnDisable()
        {
            targetGameObject.ValueChanged -= TargetGameObjectOnValueChanged;
        }

        private void Start()
        {
            indicatorPosition = Vector2.zero;
        }
        
        private void TargetGameObjectOnValueChanged(ValueChangeArgs<GameObject> args)
        {
            if (args.From != null)
            {
                indicatorPosition += mainCamera.Value.GetNormalizedScreenPosition(args.From.transform.position);
            }
            else
            {
                indicatorPosition = Vector2.zero;
            }
            
            if (args.To != null)
            {
                indicatorPosition -= mainCamera.Value.GetNormalizedScreenPosition(args.To.transform.position);
            }
            
            indicator.gameObject.SetActive(args.To != null);
        }
        
        private void LateUpdate()
        {
            if (targetGameObject.Value != null)
            {
                indicatorPosition = Vector2.SmoothDamp(indicatorPosition, Vector2.zero, ref indicatorVelocity, indicatorSpeed);

                Vector2 targetUV = mainCamera.Value.GetNormalizedScreenPosition(targetGameObject.Value.transform.position) + indicatorPosition;
                var parentRect = ((RectTransform)indicator.parent).rect;
                indicator.anchoredPosition = parentRect.size * targetUV;
            }
        }
    }
}
