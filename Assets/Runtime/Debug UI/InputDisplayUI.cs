using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Attrition
{
    public class InputDisplayUI : MonoBehaviour
    {
        [FormerlySerializedAs("inputs")] [SerializeField] private List<Input> actions;
        [SerializeField] private Color defaultColor;
        [SerializeField] private Color pressedColor;

        [SerializeField] private RectTransform movePosition;
        [SerializeField] private RectTransform moveArea;
        [SerializeField] private RectTransform moveLine;
        [SerializeField] private InputActionReference moveInput;
        [SerializeField] private float movePositionSpeed;
        [SerializeField] private float maxMovePositionSize;
        [SerializeField] private float minMovePositionSize;
        [SerializeField] private AnimationCurve movePositionSizeCurve;

        private Vector2 movePositionVelocity;
        
        [Serializable]
        private class Input
        {
            public InputActionReference inputAction;
            public Image image;
        }

        private void Update()
        {
            foreach (var action in actions)
            {
                action.image.color = action.inputAction.action.IsPressed()
                    ? pressedColor
                    : defaultColor;
            }

            Vector2 input = moveInput.action.ReadValue<Vector2>();
            Vector2 targetPosition = moveArea.rect.size / 2f * input;
            movePosition.localPosition = Vector2.SmoothDamp(movePosition.localPosition, targetPosition,
                ref movePositionVelocity, movePositionSpeed);
            movePosition.sizeDelta = Vector2.one * Mathf.Lerp(minMovePositionSize, maxMovePositionSize,
                movePositionSizeCurve.Evaluate(input.magnitude));

            Vector2 moveDelta = movePosition.localPosition;
            moveLine.SetLocalPositionAndRotation(moveDelta / 2f,
                Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, moveDelta)));
            moveLine.sizeDelta = new Vector2(moveLine.sizeDelta.x, moveDelta.magnitude);
        }
    }
}
