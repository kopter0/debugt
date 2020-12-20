using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMechanicsScript : MonoBehaviour
{

    public float maxStress = 10f;
    public float stressLevel { get; private set; } = 0f;
    [HideInInspector]
    public bool isInititated { get; private set; } = true;

    private Vector3 initialPos;
    private Quaternion initialRot;
    // Start is called before the first frame update
    void Awake()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
        isInititated = true;
    }

    public void UpdateStressLevel(float val)
    {
        stressLevel += val;
        if (stressLevel > maxStress)
        {
            GameObject gc = GameObject.FindGameObjectWithTag("GameController");
            gc.GetComponent<GameControllerScript>().PlayerDead();
        }
        stressLevel = Mathf.Clamp(stressLevel, 0, maxStress);
    }

    public void Restart()
    {
        transform.position = initialPos;
        transform.rotation = initialRot;
        stressLevel = 0;
    }


}
