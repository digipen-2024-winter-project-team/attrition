// using System;
// using System.Collections;
// using UnityEngine;
//
// namespace Attrition.CharacterSelection.Selection.Navigation
// {
//     public class Cooldown
//     {
//         private readonly MonoBehaviour coroutineHandler;
//
//         public float Duration { get; set; }
//         public bool IsOnCooldown { get; private set; }
//
//         public Cooldown(MonoBehaviour coroutineHandler, float duration = 0f)
//         {
//             this.coroutineHandler = coroutineHandler ?? throw new ArgumentNullException(nameof(coroutineHandler));
//             this.Duration = duration;
//         }
//
//         public void StartCooldown()
//         {
//             if (this.IsOnCooldown)
//             {
//                 return;
//             }
//
//             this.coroutineHandler.StartCoroutine(this.Tick());
//         }
//
//         private IEnumerator Tick()
//         {
//             this.IsOnCooldown = true;
//             yield return new WaitForSeconds(this.Duration);
//             this.IsOnCooldown = false;
//         }
//     }
// }
