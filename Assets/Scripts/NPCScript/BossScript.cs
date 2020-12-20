using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : NPCScript
{
    public Animator anim;
    public Material mat;


    // Start is called before the first frame update
    void Start()
    {
        Init();
        state = NPCState.Engage;
        mat.SetColor("_EmissionColor", Color.clear);
    }

    protected override void Engage()
    {
        //Vector3 v1 = -transform.right; v1.y = 0;
        //Vector3 v2 = player.position - transform.position; v2.y = 0;
        //float a = Vector3.SignedAngle(v1.normalized, v2.normalized, Vector3.up);
        //float cosa = Mathf.Cos(Mathf.Deg2Rad * a), sina = Mathf.Sin(Mathf.Deg2Rad * a);
        //anim.SetFloat("Right", a / 90);
        //anim.SetFloat("AimRight", a / 90);
        //if (Mathf.Abs(a) < 20)
        //{
        //    anim.SetFloat("Right", 0);
        //    anim.SetFloat("AimRight", a / 90);
        //}
    }

    public void Revive()
    {
        anim.SetBool("Revive", true);
        mat.SetColor("_EmissionColor", new Color(0, 1, 0, 5));

    }

    public void OnDisable()
    {
        mat.SetColor("_EmissionColor", Color.clear);

    }


}
