using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recompensas : MonoBehaviour
{
    private GameObject maca;
    private GameObject uva;
    private GameObject laranja;
    private GameObject life;
    float altura = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        maca = Resources.Load("Maca") as GameObject;
        uva = Resources.Load("Uva") as GameObject;
        laranja = Resources.Load("Laranja") as GameObject;
        life = Resources.Load("PotionLife") as GameObject;
    }

    public void InstantiateFoods()
    {
        
        Instantiate(maca,new Vector3(transform.position.x+0.2f, transform.position.y+ altura, transform.position.z),Quaternion.identity);
        Instantiate(uva, new Vector3(transform.position.x + 0.4f, transform.position.y + altura, transform.position.z), Quaternion.identity);
        Instantiate(laranja, new Vector3(transform.position.x - 0.2f, transform.position.y + altura, transform.position.z), Quaternion.identity);
    }

    public void InstantiateLife()
    {
        Instantiate(life, new Vector3(transform.position.x, transform.position.y + altura, transform.position.z), Quaternion.identity);
    }
}
