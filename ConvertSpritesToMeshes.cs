using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

//That scrip used for creating meshes with materials from sprite renderers on scene, with preservation of objects hierarchy

namespace Utils
{
    public static class ConvertSpritesToMeshes
    {
        private static string _shaderName = "Sprites/Default";

        public static Dictionary<Sprite, Material> _materials;
        public static Dictionary<Sprite, Mesh> _meshes;

        private static Shader shader;
        private static int index;

        [MenuItem("GameObject/ConvertSpritesToMeshes", false, 20)]

        public static void SpritesToMeshes(MenuCommand menuCommand)
        {
            var path = Path.Combine(Application.dataPath, "Plugins/MeshBakerUtil", "Temp");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            shader = Shader.Find(_shaderName);
            index = 0;

            var _gameObject = menuCommand.context as GameObject;

            _materials = new Dictionary<Sprite, Material>();
            _meshes = new Dictionary<Sprite, Mesh>();

            GetChildren(_gameObject, _gameObject.transform.root, true);

            AssetDatabase.SaveAssets();
        }

        private static void GetChildren(GameObject currentGameObject, Transform newCurrentGameObjectTransform, bool isRootGameObject)
        {
            GameObject newGameObject;
            var spriteRenderer = currentGameObject.GetComponent<SpriteRenderer>();
            var childGameObjectCount = currentGameObject.transform.childCount;

            if (spriteRenderer == null && currentGameObject.transform.childCount == 0)
                return;

            if (isRootGameObject)
            {
                newGameObject = new GameObject($"{currentGameObject.name.ToUpper()}_MESH_OBJECT");
            }
            else
            {
                newGameObject = spriteRenderer != null
                    ? CreateMeshObject(newCurrentGameObjectTransform, spriteRenderer, 1)
                    : CreateEmptyObject(currentGameObject.name, newCurrentGameObjectTransform);
            }

            if (childGameObjectCount == 0)
                return;

            for (var i = 0; i < childGameObjectCount; i++)
            {
                var childGameObject = currentGameObject.transform.GetChild(i).gameObject;

                GetChildren(childGameObject, newGameObject.transform, false);
            }
        }

        private static GameObject CreateEmptyObject(string name, Transform transform)
        {
            var newGameObject = new GameObject(name);

            newGameObject.transform.SetParent(transform);
            newGameObject.isStatic = true;
            newGameObject.transform.position = transform.position;
            newGameObject.transform.rotation = transform.rotation;
            newGameObject.transform.localScale = transform.localScale;

            return newGameObject;
        }

        private static GameObject CreateMeshObject(Transform parent, SpriteRenderer spriteRenderer, int index)
        {
            var meshObject = new GameObject(spriteRenderer.name);
            var meshFilter = meshObject.AddComponent<MeshFilter>();
            var meshRenderer = meshObject.AddComponent<MeshRenderer>();

            meshObject.isStatic = true;

            Material material;
            Mesh mesh;

            // Materials
            if (_materials.ContainsKey(spriteRenderer.sprite))
                material = _materials[spriteRenderer.sprite];
            else
            {
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, $"Assets/Plugins/MeshBakerUtil/Temp/{spriteRenderer.sprite.name}_material_{index}.asset");
                _materials[spriteRenderer.sprite] = material;
            }
            material.SetTexture("_MainTex", spriteRenderer.sprite.texture);

            // Meshes
            if (_meshes.ContainsKey(spriteRenderer.sprite))
                mesh = _meshes[spriteRenderer.sprite];
            else
            {
                mesh = SpriteToMesh(spriteRenderer.sprite);
                MeshUtility.Optimize(mesh);
                AssetDatabase.CreateAsset(mesh, $"Assets/Plugins/MeshBakerUtil/Temp/{spriteRenderer.sprite.name}_mesh_{index}.asset");
                _meshes[spriteRenderer.sprite] = mesh;
            }

            index++;

            meshFilter.mesh = mesh;
            meshRenderer.material = material;
            meshObject.transform.SetParent(parent);

            var transform = spriteRenderer.transform;

            meshObject.transform.position = transform.position;
            meshObject.transform.rotation = transform.rotation;
            meshObject.transform.localScale = transform.localScale;

            return meshObject;
        }

        private static Mesh SpriteToMesh(Sprite sprite)
        {
            var mesh = new Mesh();
            mesh.SetVertices(Array.ConvertAll(sprite.vertices, i => (Vector3)i).ToList());
            mesh.SetUVs(0,sprite.uv.ToList());
            mesh.SetTriangles(Array.ConvertAll(sprite.triangles, i => (int)i),0);
            return mesh;
        }
    }
}