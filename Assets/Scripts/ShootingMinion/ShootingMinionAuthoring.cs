using UnityEngine;
using Unity.Entities;

//used to bake data from Component into ECS
public class ShootingMinionAuthoring : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public GameObject BulletPrefab;
    public int BulletSpawnCount = 50;
    [Range(0f, 10f)] public float BulletSpread = 5f;
    public float BulletSpawnCooldown = 0.02f;
    public float BulletSpawnCooldownTimer = 0;


    public class ShootingMinionBaker : Baker<ShootingMinionAuthoring>
    {
        public override void Bake(ShootingMinionAuthoring authoring)
        {
            Entity shootingMinionEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(shootingMinionEntity, new ShootingMinionComponent
            {
                MoveSpeed = authoring.MoveSpeed,
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.None),
                BulletSpawnCount = authoring.BulletSpawnCount,
                BulletSpread = authoring.BulletSpread,
                BulletSpawnCooldown = authoring.BulletSpawnCooldown,
                BulletSpawnCooldownTimer = authoring.BulletSpawnCooldownTimer
            });
        }
    }
}
