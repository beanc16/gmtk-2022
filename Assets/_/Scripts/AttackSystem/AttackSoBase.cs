using System.Threading;
using _.Scripts.Core;
using _.Scripts.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace _.Scripts.AttackSystem
{
    public enum Attacker
    {
        PlayerAttack,
        EnemyAttack
    }
    public abstract class AttackSoBase : ScriptableObject
    {
        [SerializeField] protected GameObject attackPrefab;
        
        [SerializeField] protected string attackName;
        [SerializeField] protected bool destroyOnHit;
        [SerializeField] protected bool damageOnHit;
        [SerializeField] protected float attackCooldown;
        [SerializeField] protected float damage;
        [SerializeField] protected float aoeRange;
        [SerializeField] protected bool damagePlayer;
        [SerializeField] protected float lifeTime;
        [SerializeField] protected float speed;
        [SerializeField] protected Attacker attacker;
        [SerializeField] private LayerMask layerMask;
        public string GetAttackName() => attackName;
        public bool GetDamagePlayer() => damagePlayer;
        public bool GetDestroyOnHit() => destroyOnHit;
        public bool GetDamageOnHit() => damageOnHit;
        public float GetAoeRange() => aoeRange;
        public float GetDamage() => damage; // expand upon this for variations.
        public float GetCooldown() => attackCooldown;
        
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
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        public abstract void Shoot(Transform fromTransform);
        public abstract void AttackUpdate(GameObject attackObject, UnityAction onAttackFinished);

        protected Vector3 GetMoveDirection(Vector3 attackObject)
        {
            var targetPosition = attacker switch
            {
                Attacker.PlayerAttack => Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Attacker.EnemyAttack => PlayerController.Instance.transform.position,
                _ => new Vector3()
            };
            var moveDirection = targetPosition - attackObject;
            moveDirection.z = 0;
            return moveDirection.normalized;
        }

        public bool IsInLineOfSight(Vector3 fromPos)
        {
            if (attacker == Attacker.PlayerAttack) return true;
            var direction = GetMoveDirection(fromPos);
            var layerMaskInvert = layerMask;
            layerMaskInvert = ~layerMaskInvert;
            var hit2d = Physics2D.Raycast(fromPos, direction, Mathf.Infinity,layerMaskInvert);
            Debug.DrawRay(fromPos,direction * hit2d.distance, hit2d.transform.CompareTag("Player") ? Color.blue : Color.red, 2f);
            return hit2d.transform.CompareTag("Player");
        }
    }
}