using PKClone.Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace PKClone.Systems
{
    public class TimeToLifeDestroySystem : ComponentSystem
    {
        public struct TimeToLifeDestroySystemFilter
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray entity;
            public ComponentDataArray<TimeToLife> TTL;
        }

        [Inject] private TimeToLifeDestroySystemFilter Data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < Data.Length; i++)
            {
                var ttl = Data.TTL[i];
                ttl.TimeToLifeInSeconds -= Time.deltaTime;
                Data.TTL[i] = ttl;

                if (Data.TTL[i].TimeToLifeInSeconds < 0)
                {
                    PostUpdateCommands.DestroyEntity(Data.entity[i]);
                    
                }
            }
        }
    }
}