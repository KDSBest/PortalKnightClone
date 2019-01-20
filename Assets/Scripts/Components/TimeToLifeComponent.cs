using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace PKClone.Components
{
    [Serializable]
    public struct TimeToLife : IComponentData
    {
        public float TimeToLifeInSeconds;
    }

    public class TimeToLifeComponent : ComponentDataWrapper<TimeToLife>
    {
    }
}