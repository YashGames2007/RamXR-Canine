using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "VR/Events/Organ Mode")]
public class OrganModeEventChannelSO : ScriptableObject
{
    public UnityAction<OrganMode> OnEventRaised;

    public void RaiseEvent(OrganMode mode)
    {
        OnEventRaised?.Invoke(mode);
    }
}