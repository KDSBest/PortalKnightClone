using System;
using Unity.Entities;

namespace PKClone.Components
{
    [Serializable]
    public struct Block : IComponentData
    {
        public BlockType Type;
    }

    public class BlockComponent : ComponentDataWrapper<Block>
    {

    }
}