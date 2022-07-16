using _.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace _.Scripts.AttackSystem
{
    public abstract class AttackSoBase : ScriptableObject
    {
        [SerializeField] protected GameObject attackPrefab;
        [SerializeField] protected string attackName;
        [SerializeField] protected float attackCooldown;
        public string GetAttackName() => attackName;
        public float GetAttackCooldown() => attackCooldown;
        
        protected IObjectPool<GameObject> Pool;
        protected static GameController GameController;
        
        public virtual void EnableAttack()
        {
            if(GameController == null) GameController = GameController.Instance;
            Pool = new ObjectPool<GameObject>(() 
                => Instantiate(attackPrefab),
                OnGet,
                OnRelease,
                DestroyImmediate,
                false,
                10,
                20);
        }

        public virtual void DisableAttack()
        {
            Pool.Clear();
        }

        public virtual void OnGet(GameObject obj)
        {
            obj.SetActive(true);
        }

        public virtual void OnRelease(GameObject obj)
        {
            obj.SetActive(false);
        }

        public abstract void Shoot(Transform fromTransform);
    }
}