using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections.Generic;

public class OrganRuntime : MonoBehaviour
{
    private List<XRSimpleInteractable>
        interactables = new();

    [Header("DeepVision")]
    public DeepVisionTutorialSO tutorialData;

    public DeepVisionMovementSequence movementSequence;

    [SerializeField] private OrgansDeepVisionUIController uIController;

    void Awake()
    {
        CacheInteractables();
    }

    void CacheInteractables()
    {
        interactables.Clear();

        XRSimpleInteractable[] found =
            GetComponentsInChildren<
                XRSimpleInteractable>(true);

        interactables.AddRange(found);
    }

    public void ApplyMode(
        OrganMode mode
    )
    {
        switch (mode)
        {
            case OrganMode.Explore:
                ApplyExploreMode();
                break;

            case OrganMode.Labels:
                ApplyLabelMode();
                break;

            case OrganMode.DeepVision:
                ApplyDeepVisionMode();
                break;
        }
    }

    void ApplyExploreMode()
    {
        SetInteractables(true);

        OrganLabelSystem.Instance.DisableAllLabels();

        uIController.gameObject.SetActive(false);

        OrgansDeepVisionMovementController.Instance
            .ResetAllTransforms();
    }

    void ApplyLabelMode()
    {
        SetInteractables(false);

        OrganLabelSystem.Instance.EnableLabels(this);

        uIController.gameObject.SetActive(false);

        OrgansDeepVisionMovementController.Instance
            .ResetAllTransforms();
    }

    void ApplyDeepVisionMode()
    {
        SetInteractables(false);

        OrganLabelSystem.Instance.DisableAllLabels();

        OrgansDeepVisionMovementController.Instance.SetSequence(movementSequence);

        uIController.gameObject.SetActive(true);

        uIController.InitializeTutorial(tutorialData);
    }

    public void ResetRuntime()
    {
        SetInteractables(false);

        OrganLabelSystem.Instance.DisableLabels(this);

        uIController.gameObject.SetActive(false);

        OrgansDeepVisionMovementController.Instance
            .ResetAllTransforms();
    }

    void SetInteractables(bool value)
    {
        foreach (var interactable
            in interactables)
        {
            if (interactable != null)
            {
                interactable.enabled = value;
            }
        }
    }
}