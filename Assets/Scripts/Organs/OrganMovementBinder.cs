using UnityEngine;
using System.Collections.Generic;

public class OrganMovementBinder : MonoBehaviour
{
    public List<OrganBinding> bindings;

    private Dictionary<OrganID, OrganBinding> bindingMap;

    void Awake()
    {
        bindingMap = new();

        foreach (var binding in bindings)
        {
            bindingMap[binding.organID] = binding;
        }
    }

    public OrganBinding GetBinding(OrganID id)
    {
        return bindingMap[id];
    }

    public List<OrganBinding> GetAllBindings()
    {
        return bindings;
    }
}