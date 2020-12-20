using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Robot2Script : NPCScript
{

    public Animator anim;
    public float reloadTime;
    public float damage = 1.0f;

    private ParticleSystem leftBlaster, rightBlaster;
    private float leftReload = 0.0f;
    private float rightReload = 1.0f;
    void Start()
    {
        Init();
        ParticleSystem[] temp = GetComponentsInChildren<ParticleSystem>();
        if (temp[0].name.Contains("Right"))
        {
            rightBlaster = temp[0];
            leftBlaster = temp[1];
        }
        else
        {
            rightBlaster = temp[1];
            leftBlaster = temp[0];
        }

        rightBlaster.startSpeed = leftBlaster.startSpeed = 15.0f;
    }

    #region StateControl

    protected override void UpdateState()
    {
        if (IsPlayerInSight())
        {
            state = NPCState.Engage;
            return;
        }
        state = NPCState.Patrolling;
    }

    protected override void Patrol()
    {
        
    }


    protected override void Engage()
    {
        //Vector3 v1 = -transform.right; v1.y = 0;
        //Vector3 v2 = player.position - transform.position; v2.y = 0;
        //float a = Vector3.SignedAngle(v1, v2, -transform.forward);
        //float cosa = Mathf.Cos(Mathf.Deg2Rad * a), sina = Mathf.Sin(Mathf.Deg2Rad * a);
        //anim.SetFloat("Right", 4 * -sina * (1 - cosa));

        //if (cosa > 0.707f)
        //    anim.SetFloat("Aiming", Mathf.Min(anim.GetFloat("Aiming") + Time.deltaTime, 1));
        //else
        //    anim.SetFloat("Aiming", Mathf.Max(anim.GetFloat("Aiming") - Time.deltaTime, 0));


        Vector3 v1 = -transform.right; v1.y = 0;
        Vector3 v2 = player.position - transform.position; v2.y = 0;
        float a = Vector3.SignedAngle(v1.normalized, v2.normalized, Vector3.up);
        //float cosa = Mathf.Cos(Mathf.Deg2Rad * a), sina = Mathf.Sin(Mathf.Deg2Rad * a);
        anim.SetFloat("Right", a / 45);
        anim.SetFloat("AimRight", a / 90);


        //if (cosa > 0.707f)
        if (Mathf.Abs(a) < 20)
        {
            //anim.SetFloat("Right", -a / 45.0f);

            if (leftReload <= 0.0f)
            {
                leftBlaster.Stop();
                leftBlaster.Play();
                leftReload = reloadTime;
            }
            if (rightReload <= 0.0f)
            {
                rightBlaster.Stop();
                rightBlaster.Play();
                rightReload = reloadTime;
            }


            leftReload -= Time.deltaTime;
            rightReload -= Time.deltaTime;
        }
        else
        {

        }
    }
    #endregion

    #region parts

    public override void HitTaken(GameObject go)
    {
        DamagablePart dp = damagableParts[nameToIndex[go.name]];
        dp.max_hit--;
        if (dp.max_hit <= 0)
        {
            go.transform.localScale = Vector3.zero;
            if (go.name.Contains("Forearm"))
            {
                if (go.name.Contains(" R "))
                    rightBlaster.gameObject.SetActive(false);
                else 
                    leftBlaster.gameObject.SetActive(false);
            }

            else if (go.name.Contains("Spine") || go.name.Contains("Neck"))
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
        nameToIndex = new Dictionary<string, int>();
        BoxCollider[] bcs = transform.GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < bcs.Length; i++)
        {
            nameToIndex[bcs[i].name] = i;
            damagableParts.Add(new DamagablePart(0, bcs[i].gameObject));
        }
    }
    #endregion

}
