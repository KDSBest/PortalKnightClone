using System;
using System.Collections.Generic;
using UnityEngine;

namespace PKClone
{
    [Serializable]
    public class SpawnDefinition
    {
        public BlockType Type;
        public int Chance;
        public Material Material;
    }
}