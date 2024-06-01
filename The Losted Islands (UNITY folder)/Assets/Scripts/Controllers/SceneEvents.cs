using System.Collections;
using System.Collections.Generic;
using DarkHorizon;
using UnityEngine;

public class SceneEvents : MonoBehaviour
{
    public GameObject generatingApple;

    public Vector3 appleGeneratingPosition;
    void Start()
    {
        StartCoroutine(Generate_Apple(Random.Range(10, 30)));
    }

    // Update is called once per frame
    IEnumerator Generate_Apple(float time)
    {
        yield return new WaitForSeconds(time);

        GameObject itemObject = Instantiate(generatingApple, appleGeneratingPosition, Quaternion.identity);
        itemObject.GetComponent<Item>().amount = 1;
        StartCoroutine(Generate_Apple(Random.Range(10,30)));
    }
}
