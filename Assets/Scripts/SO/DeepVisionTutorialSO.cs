using UnityEngine;

[CreateAssetMenu(menuName = "DeepVision/Tutorial")]
public class DeepVisionTutorialSO : ScriptableObject
{
    [TextArea(3, 10)]
    public string[] steps;
}