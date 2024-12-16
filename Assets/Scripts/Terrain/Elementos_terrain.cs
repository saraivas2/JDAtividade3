using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elementos_terrain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("fire"))
        {
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("AttackFire"))
        {
            Destroy(collision.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
