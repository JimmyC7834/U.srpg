using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Game
{
    public static class Extensions
    {
        public static Vector3 FlatY(this Vector3 v) => new Vector3(v.x, 0, v.z);
        public static Vector3 ExtractX(this Vector3 v) => Vector3.right * v.x;
        public static Vector3 ExtractY(this Vector3 v) => Vector3.up * v.y;
        public static Vector3 ExtractZ(this Vector3 v) => Vector3.forward * v.z;
        public static Vector2 GameV3ToV2(this Vector3 v) => new Vector2(v.x, v.z);
        public static Vector2Int GameV3ToV2Int(this Vector3 v) => Vector2Int.FloorToInt(new Vector2(v.x, v.z));
        public static Vector3 GameV2ToV3(this Vector2 v) => GameV2ToV3(v, 0f);
        public static Vector3 GameV2ToV3(this Vector2 v, float y) => new Vector3(v.x, y, v.y);
        public static Vector2 GameRotateV2(this Vector2 v, float rad) => new Vector2(
            v.x * Mathf.Cos(rad) - v.y * Mathf.Sin(rad), 
            v.x * Mathf.Sin(rad) + v.y * Mathf.Cos(rad));
    }

    public class GameObjectPool<T> where T : MonoBehaviour
    {
        private ObjectPool<T> _pool;
        private GameObject _prefab;
        private Transform _parent;
        public GameObjectPool(GameObject prefab, Transform parent = null)
        {
            _prefab = prefab;
            _parent = parent;
            _pool = new ObjectPool<T>(
                CreatT,
                PoolT,
                ReleaseT
                );
        }

        public T GetRaw() => _pool.Get();

        public T Get(Action<T> Initialize)
        {
            T newObject = _pool.Get();
            Initialize(newObject);
            return newObject;
        }
        
        public void Release(T item) => _pool.Release(item);

        private T CreatT() => 
            Object.Instantiate(_prefab, _parent).GetComponent<T>();
        
        private void PoolT(T item) =>
            item.gameObject.SetActive(true);

        private void ReleaseT(T item) => 
            item.gameObject.SetActive(false);
    }
}