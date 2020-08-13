using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("CoDestroy");
    }

    private IEnumerator CoDestroy()
    {
        yield return new WaitForSeconds(1.0f);

        Destroy(gameObject);
    }
}
