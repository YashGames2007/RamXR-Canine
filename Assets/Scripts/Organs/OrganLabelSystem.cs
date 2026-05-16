using UnityEngine;
using System.Collections.Generic;

public class OrganLabelSystem : MonoBehaviour
{
    [Header("All Organ Runtimes")]
    [SerializeField]
    private List<OrganRuntime> organs;

    public static OrganLabelSystem Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void EnableLabels(
        OrganRuntime runtime
    )
    {
        DisableAllLabels();

        if (runtime == null)
            return;

        ModelPart[] parts =
            runtime.GetComponentsInChildren<ModelPart>(true);

        foreach (var part in parts)
        {
            part.TurnOnLabel();
        }
    }

    public void DisableLabels(
        OrganRuntime runtime
    )
    {
        if (runtime == null)
            return;

        ModelPart[] parts =
            runtime.GetComponentsInChildren<ModelPart>(true);

        foreach (var part in parts)
        {
            part.TurnOffLabel();
        }
    }

    public void DisableAllLabels()
    {
        foreach (var organ in organs)
        {
            if (organ == null)
                continue;

            ModelPart[] parts =
                organ.GetComponentsInChildren<ModelPart>(true);

            foreach (var part in parts)
            {
                part.TurnOffLabel();
            }
        }
    }
}