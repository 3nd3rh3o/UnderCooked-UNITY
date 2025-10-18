using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Environment", menuName = "Scriptable Objects/Environment")]
public class Environment : ScriptableObject
{
    public List<Goal> goals;
    public List<Tuple<Transform, ItemInstance>> itemInWorld;
    public List<Tuple<Transform, ItemInstance, StandInstance>> itemsOnStands;
    public List<StandInstance> stands;
}