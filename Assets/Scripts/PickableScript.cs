using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableScript : MonoBehaviour
{
    float counter;
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 1.0f, 0);
        Vector3 next = transform.position;
        next.y = 0.5f + Mathf.Sin(counter) / 4.0f;
        transform.position = next;
        counter += Time.deltaTime;
        if (counter > 360)
        {
            counter = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Player"))
        {
            other.GetComponent<PlayerMechanicsScript>().UpdateStressLevel(-10.0f);
            Destroy(gameObject  );
        }
    }
}
