using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Collections;

public partial struct ShootingMinionSystem : ISystem
{
    private EntityManager _entityManager;
    private Entity _shootingMinionEntity;
    private ShootingMinionComponent _shootingMinionComponent;

    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;
        _shootingMinionEntity = SystemAPI.GetSingletonEntity<ShootingMinionComponent>();
        _shootingMinionComponent = _entityManager.GetComponentData<ShootingMinionComponent>(_shootingMinionEntity);

        Move(ref state);
        Shoot(ref state);
    }

    private void Move(ref SystemState state)
    {
        //move forward
        LocalTransform shootingMinionTransform = _entityManager.GetComponentData<LocalTransform>(_shootingMinionEntity);
        shootingMinionTransform.Position += shootingMinionTransform.Forward() * _shootingMinionComponent.MoveSpeed;

        //apply the positional changes
        _entityManager.SetComponentData(_shootingMinionEntity, shootingMinionTransform);
    }

    private void Shoot(ref SystemState state)
    {
        if (_shootingMinionComponent.BulletSpawnCooldownTimer >= _shootingMinionComponent.BulletSpawnCooldown)
        {
            _shootingMinionComponent.BulletSpawnCooldownTimer = 0;
            for (int i = 0; i < _shootingMinionComponent.BulletSpawnCount; i++)
            {

                //buffer to execute certain code all at once
                EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

                Entity bulletEntity = _entityManager.Instantiate(_shootingMinionComponent.BulletPrefab);

                ecb.AddComponent(bulletEntity, new BulletComponent
                {
                    Speed = 25f,
                    RemainingLifeTime = 5f
                });

                LocalTransform bulletTransform = _entityManager.GetComponentData<LocalTransform>(bulletEntity);
                LocalTransform shootingMinionTransform = _entityManager.GetComponentData<LocalTransform>(_shootingMinionEntity);

                bulletTransform.Rotation = shootingMinionTransform.Rotation;
                float offset = UnityEngine.Random.Range(-_shootingMinionComponent.BulletSpread, _shootingMinionComponent.BulletSpread);
                
                bulletTransform.Position = shootingMinionTransform.Position + shootingMinionTransform.Forward() + shootingMinionTransform.Right() * offset;

                ecb.SetComponent(bulletEntity, bulletTransform);
                ecb.Playback(_entityManager);

                ecb.Dispose();
            }
        }
        _shootingMinionComponent.BulletSpawnCooldownTimer += SystemAPI.Time.DeltaTime;
        _entityManager.SetComponentData(_shootingMinionEntity, _shootingMinionComponent);
        Debug.Log(_shootingMinionComponent.BulletSpawnCooldownTimer);
    }
}
