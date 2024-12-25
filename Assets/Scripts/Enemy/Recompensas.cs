using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recompensas : MonoBehaviour
{
    public GameObject[] lista;
    public GameObject life;
    float altura = 0.5f;
    
    public void InstantiateFoods()
    {
        //0 = maçã, 1 = laranja, 2 = uvas
        int num = Random.Range(0, lista.Length);
        Instantiate(lista[num], new Vector3(transform.position.x + 0.2f, transform.position.y + altura, transform.position.z), Quaternion.identity);
        num = Random.Range(0, lista.Length);
        Instantiate(lista[num], new Vector3(transform.position.x - 0.2f, transform.position.y + altura, transform.position.z), Quaternion.identity);


    }

    public void InstantiateLife()
    {
        Instantiate(life, new Vector3(transform.position.x, transform.position.y + altura, transform.position.z), Quaternion.identity);
    }
}
