using Unity.Entities;
using UnityEngine;

public struct BulletComponent : IComponentData
{
    public float Speed;
    public float RemainingLifeTime;
}
