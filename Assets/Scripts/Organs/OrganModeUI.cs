using UnityEngine;
using UnityEngine.UI;

public class OrganModeUI : MonoBehaviour
{
    [SerializeField]
    private OrganModeEventChannelSO modeEvent;

    [SerializeField]
    private Button exploreButton;

    [SerializeField]
    private Button labelsButton;

    [SerializeField]
    private Button deepVisionButton;

    void Awake()
    {
        exploreButton.onClick.AddListener(() =>
        {
            modeEvent.RaiseEvent(
                OrganMode.Explore
            );
        });

        labelsButton.onClick.AddListener(() =>
        {
            modeEvent.RaiseEvent(
                OrganMode.Labels
            );
        });

        deepVisionButton.onClick.AddListener(() =>
        {
            modeEvent.RaiseEvent(
                OrganMode.DeepVision
            );
        });
    }
}