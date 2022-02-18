using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] Canvas Skills;
    bool mySkillBoard = false;

    //input fields
    private ThirdPersonActionAsset playerActionAsset;
    private InputAction move;
    public bool DebugTrail;

    //power
    [SerializeField]
    private int maxPower = 60;
    private int powerCost = 10;
    private int currentPower;
    public PlayerPowerBar playerPowerBar;

    //mine
    public GameObject mine;
    public Text mineAmount;


    //power charging
    [SerializeField] int powerHealin = 5;
    [SerializeField] float healinTime = 2f;
    WaitForSeconds regenTick = new WaitForSeconds(1f);
    Coroutine regen;


    //weapon attack
    public WeaponHandlerRef weaponHandlerRef;
    private BoxCollider weaponCollider;
    public LayerMask hitLayer;

    public struct BufferObj
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 size;
    }
    
    private int maxFrameBuffer = 2;
    private LinkedList<BufferObj> trailList = new LinkedList<BufferObj>();
    private LinkedList<BufferObj> trailFillerList = new LinkedList<BufferObj>();

    //movment fields
    private Rigidbody rb;
    [SerializeField]
    private float movementForce = 1f;
    [SerializeField]
    private float jumpForce = 5f;
    [SerializeField]
    private float maxSpeed = 5f;
    private Vector3 forceDirection = Vector3.zero;
    [SerializeField]
    private Camera playerCamera;
    private Animator animator;

    //attack
    private int attackId = 0;
    [SerializeField]
    private int attackDamage;
    public int AttackDamage 
    { 
        set 
        { 
            attackDamage = value; 
        } 
    }

    private void Awake()
    {

        Skills.enabled = false;
       // gameOver.SetActive(false);


        rb = GetComponent<Rigidbody>();
        playerActionAsset = new ThirdPersonActionAsset();
        animator = GetComponent<Animator>();
        weaponCollider = (BoxCollider)weaponHandlerRef.weapon.GetComponent<Collider>();
        currentPower = maxPower;
        playerPowerBar.SetBarPower(maxPower);
    }

    private void OnEnable()
    {
        playerActionAsset.Player.Skill.started += PlayerSkillBoard;
        playerActionAsset.Player.Mine.started += ThrowMine;
        playerActionAsset.Player.Jump.started += Dojump;
        playerActionAsset.Player.Attack.started += DoAttack;
        playerActionAsset.Player.Quit.started += DoQuit;
        move = playerActionAsset.Player.Move;
        playerActionAsset.Player.Enable();
    }


    private void OnDisable()
    {
        playerActionAsset.Player.Skill.started -= PlayerSkillBoard;
        playerActionAsset.Player.Mine.started -= ThrowMine;
        playerActionAsset.Player.Jump.started -= Dojump;
        playerActionAsset.Player.Attack.started -= DoAttack;
        playerActionAsset.Player.Quit.started -= DoQuit;
        playerActionAsset.Player.Disable();
    }

    private void DoQuit(InputAction.CallbackContext obj)
    {
        Application.Quit();
    }



    private void PlayerSkillBoard(InputAction.CallbackContext obj)
    {
        if (!mySkillBoard)
        {
            Skills.enabled = true;
        }
        else
        {
            Skills.enabled = false;
        }
        mySkillBoard = !mySkillBoard;
    }

    private void FixedUpdate()
    {
        forceDirection += move.ReadValue<Vector2>().x * GetCameraRight(playerCamera) * movementForce;
        forceDirection += move.ReadValue<Vector2>().y * GetCameraForward(playerCamera) * movementForce;
        rb.AddForce(forceDirection, ForceMode.Impulse);
        forceDirection = Vector3.zero;

        if (rb.velocity.y < 0f)
            rb.velocity -= Vector3.down * Physics.gravity.y * Time.fixedDeltaTime;
        Vector3 horizontalVelocity = rb.velocity;
        horizontalVelocity.y = 0;
        if (horizontalVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            rb.velocity = horizontalVelocity.normalized * maxSpeed + Vector3.up * rb.velocity.y;
        if (animator.GetBool("isDamageOn"))
        {
            AttackOn();
        }
        LookAt();
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
        foreach(Collider collider in colliderList.Values)
        {
            HitData hd = new HitData();
            hd.id = attackId;
            Hittable hittable = collider.GetComponent<Hittable>();
            if (hittable)
            {
                hittable.Hit(hd, attackDamage);
            }
        }
    }

    private LinkedList<BufferObj> FillTrail(BufferObj from, BufferObj to)
    {
        LinkedList<BufferObj> fillerList = new LinkedList<BufferObj>();
        float distance = Mathf.Abs((from.position - to.position).magnitude);
        if (distance > weaponCollider.size.z)
        {
            float steps = Mathf.CeilToInt(distance / weaponCollider.size.z);
            float stepsAmount = 1 / (steps + 1);
            float stepValue = 0;
            for(int i=0; i < (int)steps; i++)
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
    }

    private void OnDrawGizmos()
    {
        if (DebugTrail)
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
        }
    }

    private void CollectColliders(Collider[] hits,Dictionary<long,Collider> colliderList)
    {
        for(int i = 0; i < hits.Length; i++)
        {
            
            if (!(colliderList.ContainsKey(hits[i].GetInstanceID())))
            {
                colliderList.Add(hits[i].GetInstanceID(), hits[i]);
            }
        }
    }

    private void LookAt()
    {
        Vector3 direction = rb.velocity;
        direction.y = 0f;
        if (move.ReadValue<Vector2>().sqrMagnitude > 0.1f && direction.sqrMagnitude > 0.1f)
            rb.rotation = Quaternion.LookRotation(direction, Vector3.up);
        else
            rb.angularVelocity = Vector3.zero;
           
    }
    private Vector3 GetCameraRight(Camera playerCamera)
    {
        Vector3 right = playerCamera.transform.right;
        right.y = 0;
        return right.normalized;
    }

    private Vector3 GetCameraForward(Camera playerCamera)
    {
        Vector3 forward = playerCamera.transform.forward;
        forward.y = 0;
        return forward.normalized;
    }

    private void ThrowMine(InputAction.CallbackContext obj)
    {

        CapacityMine amountMine = mineAmount.GetComponent<CapacityMine>();
        if (amountMine.currentAmountMine > 0)
        {
            amountMine.ThrowMine();
            Instantiate(mine, transform.position, transform.rotation);
        }
    }
    private void Dojump(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            forceDirection += Vector3.up * jumpForce;
        }
    }

    private bool IsGrounded()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.25f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 0.3f))
            return true;
        else
            return false;
    }

    private void DoAttack(InputAction.CallbackContext obj)
    {
        if (currentPower >= powerCost) { 
            attackId++;
            PowerDamage(powerCost);
            animator.SetTrigger("attack");
        }
        
    }
    public void StartAttack()
    {
        animator.SetBool("isDamageOn", true);
    }

    public void EndtAttack()
    {
        animator.SetBool("isDamageOn", false);
    }

    public void PowerDamage(int powerDamage)
    {
        playerPowerBar.SetPowerDown(powerDamage);
        if (regen != null)
        {
            StopCoroutine(regen);
        }
        regen = StartCoroutine(PowerCharging());
    }

    private IEnumerator PowerCharging()
    {
        yield return new WaitForSeconds(healinTime);
        while (currentPower < maxPower)
        {
            currentPower += powerHealin;
            playerPowerBar.SetPowerUp(powerHealin);
            yield return regenTick;
        }
        regen = null;
    }

    public void ManaCost()
    {
        currentPower -= powerCost;
    }

}
