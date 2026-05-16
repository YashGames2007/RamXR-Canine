using UnityEngine;

[CreateAssetMenu(menuName = "VR/Organs/Organ Definition")]
public class OrganMovementDefinitionSO : ScriptableObject
{
    public OrganID organID;

    public float moveDuration = 1f;
}