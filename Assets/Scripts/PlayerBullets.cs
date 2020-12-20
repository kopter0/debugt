using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullets : MonoBehaviour
{

    private Vector3 direction;
    private float speed;

    private float lifetime = 3.0f;


    private void Update()
    {
        //transform.position = transform.position + direction * Time.deltaTime * speed;

        lifetime -= Time.deltaTime;
        if (lifetime < 0.0f)
            Destroy(gameObject);
    }

    public void SetDirectionAndSpeed(Vector3 v, float s) 
    {
        transform.GetComponent<Rigidbody>().AddForce(v * s, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
            return;
        if (other.tag.Equals("Dummie"))
        {
            other.GetComponentInParent<NPCScript>().HitTaken(other.gameObject);
            Debug.Log(other.name);
        }
        Destroy(gameObject);
    }

}
