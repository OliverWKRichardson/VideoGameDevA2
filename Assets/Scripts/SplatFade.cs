using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatFade : MonoBehaviour
{
    private Color color;
    private float destroySpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, color.a -= destroySpeed * Time.deltaTime);
        if (color.a <= 0)
        {
            //Destroy
            Destroy(gameObject);
        }
    }
}
