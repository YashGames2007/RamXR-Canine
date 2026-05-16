// using UnityEngine;
// using System.Collections;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;

// public class OrganMovementController : MonoBehaviour
// {
//     [Header("References")]
//     public OrganMovementBinder binder;
//     public OrganSelectionEventChannelSO organSelectedEvent;

//     [Header("Scene References")]
//     public GameObject skeletonObject;

//     [Header("Settings")]
//     public float moveDuration = 1f;

//     private OrganBinding currentBinding;

//     private Coroutine currentRoutine;

//     void OnEnable()
//     {
//         organSelectedEvent.OnEventRaised += OnOrganSelected;
//     }

//     void OnDisable()
//     {
//         organSelectedEvent.OnEventRaised -= OnOrganSelected;
//     }

//     private void OnOrganSelected(OrganID organID)
//     {
//         if (currentRoutine != null)
//         {
//             StopCoroutine(currentRoutine);
//         }

//         currentRoutine =
//             StartCoroutine(FocusOrganRoutine(organID));
//     }

//     private IEnumerator FocusOrganRoutine(OrganID organID)
//     {
//         // STEP 1
//         // Return previous organ back

//         if (currentBinding != null)
//         {
//             yield return ReturnOrganRoutine(currentBinding);
//         }

//         // STEP 2
//         // Focus new organ

//         OrganBinding binding =
//             binder.GetBinding(organID);

//         currentBinding = binding;

//         HideAllExcept(binding);

//         yield return MoveOrgan(
//             binding.organObject.transform,
//             binding.activePosition,
//             binding.activeRotation,
//             binding.activeScale,
//             moveDuration
//         );
//     }

//     private IEnumerator ReturnOrganRoutine(
//         OrganBinding binding
//     )
//     {
//         // Enable all objects before moving back

//         skeletonObject.SetActive(true);

//         foreach (var item in binder.GetAllBindings())
//         {
//             item.organObject.SetActive(true);
//         }

//         // Animate back

//         yield return MoveOrgan(
//             binding.organObject.transform,
//             binding.inactivePosition,
//             binding.inactiveRotation,
//             binding.inactiveScale,
//             moveDuration
//         );
//     }

//     private void HideAllExcept(OrganBinding selected)
//     {
//         skeletonObject.SetActive(false);

//         foreach (var binding in binder.GetAllBindings())
//         {
//             bool isSelected =
//                 binding == selected;

//             binding.organObject.SetActive(isSelected);
//         }
//     }

//     private IEnumerator MoveOrgan(
//         Transform target,
//         Vector3 targetPos,
//         Vector3 targetRot,
//         Vector3 targetScale,
//         float duration
//     )
//     {
//         Vector3 startPos = target.position;

//         Quaternion startRot =
//             target.rotation;

//         Vector3 startScale =
//             target.localScale;

//         Quaternion endRot =
//             Quaternion.Euler(targetRot);

//         float time = 0f;

//         while (time < duration)
//         {
//             time += Time.deltaTime;

//             float t =
//                 Mathf.Clamp01(time / duration);

//             // Smooth easing

//             t = t * t * (3f - 2f * t);

//             target.position =
//                 Vector3.Lerp(startPos, targetPos, t);

//             target.rotation =
//                 Quaternion.Slerp(startRot, endRot, t);

//             target.localScale =
//                 Vector3.Lerp(startScale, targetScale, t);

//             yield return null;
//         }

//         target.position = targetPos;

//         target.rotation = endRot;

//         target.localScale = targetScale;
//     }
// }