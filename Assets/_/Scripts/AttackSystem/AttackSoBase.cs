using UnityEngine;
using UnityEngine.Pool;

namespace _.Scripts.AttackSystem
{
    public abstract class AttackSoBase : ScriptableObject
    {
        [SerializeField] protected string attackName;
        [SerializeField] protected GameObject attackPrefab;
        //protected ObjectPool<GameObject> Pool;
        protected IObjectPool<GameObject> Pool;
        public abstract void Shoot(Transform fromTransform);

        public virtual void InitAttack()
        {
            Pool = new ObjectPool<GameObject>(() 
                => Instantiate(attackPrefab),
                obj => obj.SetActive(true),
                obj => obj.SetActive(false),
                Destroy,
                false,
                10,
                20);
        }
    }
}