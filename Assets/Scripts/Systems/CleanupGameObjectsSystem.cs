using Unity.Entities;
using UnityEngine;

namespace PKClone.Systems
{
    public class CleanupGameObjectsSystem : ComponentSystem
    {
            protected override void OnUpdate()
            {
                var goes = GameObject.FindObjectsOfType<GameObjectEntity>();

                foreach (var goe in goes)
                {
                    if (!EntityManager.Exists(goe.Entity))
                    {
                        GameObject.Destroy(goe.gameObject);
                    }
                }
            }
    }
}
