using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

namespace Game
{
    /**
     * Simple Object Pool for Generic MonoBehaviour
     */
    public class GameObjectPool<T> where T : MonoBehaviour
    {
        private ObjectPool<T> _pool;
        private T _prefab;
        
        /**
         * the parent of pool object
         */
        private Transform _parent;

        public GameObjectPool(T prefab, Transform parent = null)
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

        public T Get(Action<T> initialize)
        {
            T newObject = _pool.Get();
            initialize.Invoke(newObject);
            newObject.transform.SetParent(_parent, false);
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