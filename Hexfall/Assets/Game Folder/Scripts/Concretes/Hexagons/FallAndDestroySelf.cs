using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallAndDestroySelf : MonoBehaviour
{
    float destroyTime = 3;
    void Start()
    {
        StartCoroutine(DestroyGameObject());
    }

    IEnumerator DestroyGameObject()
    {
        gameObject.AddComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(1, 1.5f) + Vector3.right * Random.Range(-1, 2), ForceMode.Impulse);
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}
