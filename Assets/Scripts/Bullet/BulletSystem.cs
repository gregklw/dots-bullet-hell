using Unity.Entities;
using UnityEngine;
using Unity.Collections;
using Unity.Transforms;

public partial struct BulletSystem : ISystem
{
    private void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> allEntities = entityManager.GetAllEntities();

        foreach (Entity entity in allEntities)
        {
            if (entityManager.HasComponent<BulletComponent>(entity))
            {
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);

                bulletTransform.Position += bulletComponent.Speed * SystemAPI.Time.DeltaTime * bulletTransform.Forward();
                bulletComponent.RemainingLifeTime -= SystemAPI.Time.DeltaTime;

                if (bulletComponent.RemainingLifeTime <= 0f)
                {
                    entityManager.DestroyEntity(entity);
                    continue;
                }

                entityManager.SetComponentData(entity, bulletTransform);
                entityManager.SetComponentData(entity, bulletComponent);
            }
        }
    }
}
