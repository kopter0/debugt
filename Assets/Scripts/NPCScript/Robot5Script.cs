using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot5Script : NPCScript
{
    public Animator anim;
    public Transform destination;
    public float damage = 4.0f;
    //private int legNum = 4;
    private bool ball = false;
    private float ballTime = 1.0f;
    Vector3 targ;

    private void Start()
    {
        Init();
    }

    public override void HitTaken(GameObject go)
    {
        DamagablePart dp = damagableParts[nameToIndex[go.name]];
        dp.max_hit--;
        if (dp.max_hit <= 0)
        {
            go.transform.localScale = Vector3.zero;
            if (go.name.Contains("Body"))
            {
                Instantiate(shortRest, transform.position, Quaternion.identity);
                Destroy(transform.parent.gameObject);
            }
        }
        damagableParts[nameToIndex[go.name]] = dp;
    }

    public override void PopulateParts()
    {
        damagableParts = new List<DamagablePart>();
        damagableParts.Add(new DamagablePart(0, gameObject));
        foreach (Transform child in transform)
        {
            damagableParts.Add(new DamagablePart(0, child.gameObject));

            if (!child.GetComponent<CapsuleCollider>())
            {
                child.gameObject.AddComponent<CapsuleCollider>();
            }
        }
    }

    #region Actions

    protected override void UpdateState()
    {
        if (IsPlayerInSight())
        {
                anim.SetBool("Engage", true);
                state = NPCState.Engage;
                return;
        }
        anim.SetBool("Engage", false);
        state = NPCState.Patrolling;

    }

    protected override void Patrol()
    {
        
    }

    protected override void Engage()
    {
        if (!ball)
        {
            Vector3 v1 = transform.parent.forward; v1.y = 0;
            Vector3 v2 = player.position - transform.position; v2.y = 0;
            float a = Vector3.SignedAngle(v1, v2, -transform.parent.up);
            float cosa = Mathf.Cos(Mathf.Deg2Rad * a), sina = Mathf.Sin(Mathf.Deg2Rad * a);
            anim.SetFloat("Right", Mathf.Clamp(a, -1, 1));
            if (Mathf.Abs(a) < 3.0f)
            {
                anim.SetFloat("Right", 0);
                ball = true;
                targ = transform.position - Vector3.Distance(player.position, transform.position) * transform.up;
                anim.SetBool("isBall", true);
            }
        }
        else
        {
            ballTime -= Time.deltaTime;
            if (Vector3.Distance(targ, transform.position) < 2 || ballTime < 0){
                anim.SetBool("isBall", false);
                ball = false;
                ballTime = 1.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            player.GetComponent<PlayerMechanicsScript>().UpdateStressLevel(damage);
        }
        if (other.name.Contains("wall"))
        {
            state = NPCState.Dead;
            anim.SetBool("Death", true);
        }
    }

    #endregion

}
