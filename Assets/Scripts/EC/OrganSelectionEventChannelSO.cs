using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "VR/Events/Organ Selection")]
public class OrganSelectionEventChannelSO : ScriptableObject
{
    public UnityAction<OrganID> OnEventRaised;

    public void RaiseEvent(OrganID organID)
    {
        OnEventRaised?.Invoke(organID);
    }
}