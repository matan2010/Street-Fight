using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public GameObject m_RightFist;

    PlayerHealth target;
    [SerializeField] 
    private int attackDamage = 10;
    private Animator animator;
    public struct BufferObj
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 size;
    }
    public WeaponHandlerRef weaponHandlerRef;
    private BoxCollider weaponCollider;
    public LayerMask hitLayer;

    private int maxFrameBuffer = 2;
    private LinkedList<BufferObj> trailList = new LinkedList<BufferObj>();
    private LinkedList<BufferObj> trailFillerList = new LinkedList<BufferObj>();

    private int attackId = 0;


    private void Awake()
    {
        target = FindObjectOfType<PlayerHealth>();
        animator = GetComponent<Animator>();
        weaponCollider = (BoxCollider)weaponHandlerRef.weapon.GetComponent<Collider>();
    }
   /* public void AttackHitEvent()
    {
        if (target == null) return;
        target.TakeDamage(attackDamage);
       // target.GetComponent<DisplayDamage>().ShowDamageImpact();
    }*/

    /*private void FixedUpdate()
    {
      //  if (animator.GetBool("isDamageOn"))
       // {
       //     AttackOn();
       // }
    }

    private void AttackOn()
    {
        BufferObj bo = new BufferObj();
        bo.size = weaponCollider.size;
        bo.rotation = weaponCollider.transform.rotation;
        bo.position = weaponCollider.transform.position + weaponCollider.transform.TransformDirection(weaponCollider.center);
        trailList.AddFirst(bo);
        if (trailList.Count > maxFrameBuffer)
        {
            trailList.RemoveLast();
        }
        if (trailList.Count > 1)
        {
            trailFillerList = FillTrail(trailList.First.Value, trailList.Last.Value);
        }
        Collider[] hits = Physics.OverlapBox(bo.position, bo.size / 2, bo.rotation, hitLayer, QueryTriggerInteraction.Ignore);
        Dictionary<long, Collider> colliderList = new Dictionary<long, Collider>();
        CollectColliders(hits, colliderList);
        foreach (BufferObj cbo in trailFillerList)
        {
            hits = Physics.OverlapBox(cbo.position, cbo.size / 2, cbo.rotation, hitLayer, QueryTriggerInteraction.Ignore);
            CollectColliders(hits, colliderList);
        }
        
        foreach (Collider collider in colliderList.Values)
        {
            Debug.Log(collider.tag);
            HitData hd = new HitData();
            hd.id = attackId;
            needtochnge hittable = collider.GetComponent<needtochnge>();
            if (hittable)
            {
                
                hittable.Hit(hd, attackDamage);
            }
        }
    }*/

   /* private LinkedList<BufferObj> FillTrail(BufferObj from, BufferObj to)
    {
        LinkedList<BufferObj> fillerList = new LinkedList<BufferObj>();
        float distance = Mathf.Abs((from.position - to.position).magnitude);
        if (distance > weaponCollider.size.z)
        {
            float steps = Mathf.CeilToInt(distance / weaponCollider.size.z);
            float stepsAmount = 1 / (steps + 1);
            float stepValue = 0;
            for (int i = 0; i < (int)steps; i++)
            {
                stepValue += stepsAmount;
                BufferObj tmpBo = new BufferObj();
                tmpBo.size = weaponCollider.size;
                tmpBo.position = Vector3.Lerp(from.position, to.position, stepValue);
                tmpBo.rotation = Quaternion.Lerp(from.rotation, to.rotation, stepValue);
                fillerList.AddFirst(tmpBo);
            }
        }
        return fillerList;
    }*/

    /*private void CollectColliders(Collider[] hits, Dictionary<long, Collider> colliderList)
    {
        for (int i = 0; i < hits.Length; i++)
        {

            if (!(colliderList.ContainsKey(hits[i].GetInstanceID())))
            {
                colliderList.Add(hits[i].GetInstanceID(), hits[i]);
            }
        }
    }*/


    public void StartAttack()
    {
        m_RightFist.GetComponent<Collider>().enabled = true;
       // Debug.Log("a1");
       // animator.SetBool("isDamageOn", true);
    }

    public void EndAttack()
    {
        m_RightFist.GetComponent<Collider>().enabled = false;
        //animator.SetBool("isDamageOn", false);
    }
    /* private void OnDrawGizmos()
     {
         foreach (BufferObj bo in trailList)
         {
             Gizmos.color = Color.blue;
             Gizmos.matrix = Matrix4x4.TRS(bo.position, bo.rotation, Vector3.one);
             Gizmos.DrawWireCube(Vector3.zero, bo.size);
         }
         foreach (BufferObj bo in trailFillerList)
         {
             Gizmos.color = Color.yellow;
             Gizmos.matrix = Matrix4x4.TRS(bo.position, bo.rotation, Vector3.one);
             Gizmos.DrawWireCube(Vector3.zero, bo.size);
         }
     }*/


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(10);
            //Debug.Log("123");
            //TakeDamage(zombieDamage);
        }
    }
}
