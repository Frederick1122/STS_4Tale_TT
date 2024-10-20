using System.Collections.Generic;

namespace Core
{
    public class Pool<T> where T : IPoolObject
    {
        protected readonly List<T> _PoolObjects = new();
        protected int _quantity = 0;

        public void Add(IEnumerable<T> poolObjects)
        {
            foreach (var poolObject in poolObjects)
            {
                Add(poolObject);
            }
        }

        public void Add(T poolObject)
        {
            if (_PoolObjects.Count == _quantity)
            {
                _PoolObjects.Add(poolObject);
            }
            else
            {
                _PoolObjects[_quantity] = poolObject;
            }

            _quantity++;
        }

        public void Clear()
        {
            _quantity = 0;
        }
        
        public void GetAll()
        {
            Get(_quantity);
        }

        public List<T> Get(int quantity = 1)
        {
            List<T> returnedPoolObjects = new();

            if (_quantity > 0)
            {
                quantity = quantity > _quantity 
                    ? _quantity : quantity;

                for (int i = 0; i < quantity; i++)
                {
                    returnedPoolObjects.Add(GetOne());
                }    
            }
            
            return returnedPoolObjects;
        }

        public int GetQuantity()
        {
            return _quantity;
        }
        
        private T GetOne()
        {
            _quantity--;
            return _PoolObjects[_quantity];
        }
    }
}