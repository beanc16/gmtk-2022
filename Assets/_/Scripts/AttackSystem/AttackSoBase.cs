using System.Threading;
using _.Scripts.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace _.Scripts.AttackSystem
{
    public abstract class AttackSoBase : ScriptableObject
    {
        [SerializeField] protected GameObject attackPrefab;
        
        [SerializeField] protected string attackName;
        [SerializeField] protected bool destroyOnHit;
        [SerializeField] protected float attackCooldown;
        [SerializeField] protected float damage;
        public string GetAttackName() => attackName;
        public bool GetDestroyOnHit() => destroyOnHit;
        public float GetDamage() => damage; // expand upon this for variations.
        
        protected IObjectPool<GameObject> Pool;
        protected static GameController GameController;
        protected CancellationToken CancellationToken;
        
        public virtual void EnableAttack()
        {
            if(GameController == null) GameController = GameController.Instance;
            CancellationToken = GameController.gameObject.GetCancellationTokenOnDestroy();
            Pool = new ObjectPool<GameObject>(() 
                => Instantiate(attackPrefab),
                OnGet,
                OnRelease,
                Destroy,
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
        public abstract void AttackUpdate(GameObject attackObject, UnityAction onAttackFinished);


    }
}