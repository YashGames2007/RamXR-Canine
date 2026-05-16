using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class HoverTint : MonoBehaviour
{
    public Renderer targetRenderer;

    [SerializeField] private Color hoverColor = Color.cyan;

    [SerializeField] private bool useEmission = true;

    private string propertyName = "baseColorFactor";

    private XRSimpleInteractable interactable;

    private MaterialPropertyBlock propertyBlock;


    // private static readonly int BaseColorID = Shader.PropertyToID(propertyName);
    private static readonly int EmissionColorID = Shader.PropertyToID("_EmissionColor");

    private Color originalColor;
    private Color originalEmission;

    void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();

        propertyBlock = new MaterialPropertyBlock();

        Material mat = targetRenderer.material;
        int BaseColorID = Shader.PropertyToID(propertyName);
        if (mat.HasProperty(BaseColorID))
            originalColor = mat.GetColor(BaseColorID);

        if (mat.HasProperty(EmissionColorID))
            originalEmission = mat.GetColor(EmissionColorID);
    }

    void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        targetRenderer.GetPropertyBlock(propertyBlock);
        int BaseColorID = Shader.PropertyToID(propertyName);


        if (useEmission)
        {
            propertyBlock.SetColor(EmissionColorID, hoverColor * 2f);
        }
        else
        {
            propertyBlock.SetColor(BaseColorID, hoverColor);
        }

        targetRenderer.SetPropertyBlock(propertyBlock);
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        int BaseColorID = Shader.PropertyToID(propertyName);

        targetRenderer.GetPropertyBlock(propertyBlock);

        if (useEmission)
        {
            propertyBlock.SetColor(EmissionColorID, originalEmission);
        }
        else
        {
            propertyBlock.SetColor(BaseColorID, originalColor);
        }

        targetRenderer.SetPropertyBlock(propertyBlock);
    }
}