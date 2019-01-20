using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKClone.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEditorInternal;
using UnityEngine;
using Random = System.Random;

namespace PKClone
{
    public class BlockArea : MonoBehaviour
    {
        private static EntityManager manager;
        public GameObject BlockPrefab;
        public bool DrawGizmo;
        public Color GizmoColor = Color.white;
        public Mesh BlockMesh;

        public Vector3 Size = Vector3.one;
        public List<SpawnDefinition> Definitions;

        public void OnDrawGizmos()
        {
            if (DrawGizmo)
            {
                Gizmos.color = GizmoColor;
                Gizmos.DrawWireCube(this.transform.position, Size);
            }
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawWireCube(this.transform.position, Size);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public void Start()
        {
            manager = World.Active.GetOrCreateManager<EntityManager>();

            Generate();
        }

        private void Generate()
        {
            int3 offset = new int3(-Size / 2 + transform.position);
            int allChances = Definitions.Sum(x => x.Chance);
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    for (int z = 0; z < Size.z; z++)
                    {
                        var spawnDef = RandomPick(allChances, Definitions);

                        GenerateEntity(new int3(x, y, z) + offset, spawnDef.Material, spawnDef.Type);
                    }
                }
            }
        }

        private SpawnDefinition RandomPick(int allChances, List<SpawnDefinition> definitions)
        {
            int rnd = UnityEngine.Random.Range(0, allChances);

            for (int i = 0; i < definitions.Count; i++)
            {
                rnd -= definitions[i].Chance;
                if (rnd < 0)
                {
                    return definitions[i];
                }
            }

            return definitions[definitions.Count - 1];
        }

        private void GenerateEntity(int3 position, Material material, BlockType type)
        {
            var go = GameObject.Instantiate(BlockPrefab);

            go.transform.position = new Vector3(position.x, position.y, position.z);

            Entity entity = go.GetComponent<GameObjectEntity>().Entity;

            manager.SetComponentData(entity, new Position { Value = position });
            manager.SetComponentData(entity, new Block()
            {
                Type = type
            });

            manager.SetSharedComponentData(entity, new MeshInstanceRenderer
            {
                mesh = this.BlockMesh,
                material = material
            });
        }
    }

}
