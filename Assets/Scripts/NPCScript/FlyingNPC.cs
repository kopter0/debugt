using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingNPC : NPCScript
{

    private bool leftWingExists, rightWingExists;
    private Transform leftWing, rightWing;

    private void Start()
    {
        Init();
        leftWingExists = rightWingExists = true;
        leftWing = transform.Find("PA_ArchfireLeftRing");
        rightWing = transform.Find("PA_ArchfireRightRing");
    }


    public override void HitTaken(GameObject go)
    {
        DamagablePart dp = damagableParts[nameToIndex[go.name]];
        curHits[nameToIndex[go.name]]++;
        if (curHits[nameToIndex[go.name]] > dp.max_hit)
        {
            if (dp.go.name.Contains("Ring"))
            {
                if (dp.go.name.Contains("Right"))
                    rightWingExists = false;
                else
                    leftWingExists = false;
                Destroy(dp.go);
                bool a = rightWing && leftWing;
            }
            else
                StartCoroutine("Death");
        }
    }

    protected override IEnumerator Death()
    {
        Vector3 fallTo = transform.position; fallTo.y = 0;
        while (transform.position.y > 0.5f)
        {
            transform.position = Vector3.Lerp(transform.position, fallTo, Time.deltaTime);
            yield return null;
        }

        gameController.DestroyMe(this);
        yield return null;
    }

    protected override void UpdateState()
    {
        bool condition = Vector3.Distance(transform.position, player.position) < range;
        NPCState prev = state;
        state = condition ? NPCState.Engage : NPCState.Patrolling;

        if (state != prev)
        {
            switch (state)
            {
                case NPCState.Patrolling:
                    if (leftWingExists)
                        leftWing.GetComponent<ConstantForce>().relativeForce = 3 * 9.81f * Vector3.forward;
                    if (rightWingExists)
                        rightWing.GetComponent<ConstantForce>().relativeForce = 3 * 9.81f * Vector3.forward;
                    break;
                case NPCState.Engage:
                    if (leftWingExists)
                    {
                        //leftWing.GetComponent<ConstantForce>().relativeForce = 6 * 9.81f * Vector3.up;
                        leftWing.GetComponent<ConstantForce>().force = Vector3.zero;
                        leftWing.GetComponent<ConstantForce>().relativeForce = Vector3.zero;
                    }
                    if (rightWingExists)
                    {
                        //rightWing.GetComponent<ConstantForce>().relativeForce = 6 * 9.81f * Vector3.up;
                        rightWing.GetComponent<ConstantForce>().force = Vector3.zero;
                        rightWing.GetComponent<ConstantForce>().relativeForce = Vector3.zero;
                    }
                    break;
            }
        }
    }

    protected override void Patrol()
    {
        //transform.position = new Vector3(3 * Mathf.Cos(counter), transform.position.y, 3 * Mathf.Sin(counter));

        if (leftWingExists || rightWingExists)
        {
            Quaternion a = Quaternion.LookRotation(new Vector3(-Mathf.Sin(counter), 0, Mathf.Cos(counter)), Vector3.up);
            transform.rotation = a;
            counter += Time.deltaTime / 2;
            Mathf.Clamp(counter, 0, 2 * Mathf.PI);
        }
    }

    protected override void Engage()
    {
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toPlayer, Vector3.up), 4 * Time.deltaTime);
        //transform.position =  Vector3.Lerp(transform.position, player.position - 5 * toPlayer, Time.deltaTime);

        float d = Vector3.Distance(transform.position, player.position) - range / 2;

        if (leftWingExists)
            leftWing.GetComponent<ConstantForce>().relativeForce = -3 * d * 9.81f * Vector3.forward * Time.deltaTime + 6 * 9.81f * Vector3.up;
        if (rightWingExists)
            rightWing.GetComponent<ConstantForce>().relativeForce = -3 * d * 9.81f * Vector3.forward * Time.deltaTime + 6 * 9.81f * Vector3.up;
    }
}
