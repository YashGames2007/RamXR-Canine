using UnityEngine;
using UnityEngine.UI;

public class OrgansHierarchyUI : MonoBehaviour
{
    [Header("Event Channel")]
    [SerializeField] private OrganSelectionEventChannelSO organSelectedEvent;

    [Header("Buttons")]
    [SerializeField] private Button brainButton;
    [SerializeField] private Button lungsButton;
    [SerializeField] private Button liverButton;
    [SerializeField] private Button heartButton;
    [SerializeField] private Button gutsButton;
    [SerializeField] private Button testiclesButton;

    void Awake()
    {
        brainButton.onClick.AddListener(() =>
        {
            organSelectedEvent.RaiseEvent(OrganID.Brain);
        });

        lungsButton.onClick.AddListener(() =>
        {
            organSelectedEvent.RaiseEvent(OrganID.Lungs);
        });

        liverButton.onClick.AddListener(() =>
        {
            organSelectedEvent.RaiseEvent(OrganID.Liver);
        });

        heartButton.onClick.AddListener(() =>
        {
            organSelectedEvent.RaiseEvent(OrganID.Heart);
        });

        gutsButton.onClick.AddListener(() =>
        {
            organSelectedEvent.RaiseEvent(OrganID.Guts);
        });

        testiclesButton.onClick.AddListener(() =>
        {
            organSelectedEvent.RaiseEvent(OrganID.Testicles);
        });
    }
}