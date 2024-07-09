using Unity.Entities;

public struct ShootingMinionComponent : IComponentData
{
    public float MoveSpeed;
    public Entity BulletPrefab;
    public int BulletSpawnCount;
    public float BulletSpread;
    public float BulletSpawnCooldown;
    public float BulletSpawnCooldownTimer;
}
