using UnityEngine;

[System.Serializable]
public class OrganBinding
{
    public OrganID organID;

    public GameObject organObject;

    [Header("Initial State")]
    public Vector3 inactivePosition;
    public Vector3 inactiveRotation;
    public Vector3 inactiveScale;

    [Header("Focused State")]
    public Vector3 activePosition;
    public Vector3 activeRotation;
    public Vector3 activeScale;
}