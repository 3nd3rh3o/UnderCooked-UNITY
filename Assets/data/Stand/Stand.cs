using UnityEngine;

[CreateAssetMenu(fileName = "Stand", menuName = "Scriptable Objects/Stand")]
public class Stand : ScriptableObject
{
    public bool requireContainer;
    public Stand container;
    public bool isContainer;
    public float processing_time;
    public bool isGenerator;
}
