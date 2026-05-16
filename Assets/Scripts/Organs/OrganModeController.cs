using UnityEngine;

public class OrganModeController : MonoBehaviour
{
    [SerializeField]
    private OrganModeEventChannelSO modeEvent;

    private OrganRuntime currentRuntime;

    void OnEnable()
    {
        modeEvent.OnEventRaised += OnModeChanged;
    }

    void OnDisable()
    {
        modeEvent.OnEventRaised -= OnModeChanged;
    }

    public void SetCurrentRuntime(
        OrganRuntime runtime
    )
    {
        currentRuntime = runtime;
    }

    private void OnModeChanged(
        OrganMode mode
    )
    {
        if (currentRuntime == null)
            return;

        currentRuntime.ApplyMode(mode);
    }
}