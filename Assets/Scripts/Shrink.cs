using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal; //2019 VERSIONS

public class Shrink : MonoBehaviour
{

    public float rate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Light2D>().pointLightInnerRadius -= (rate * Time.deltaTime);
        GetComponent<Light2D>().pointLightOuterRadius -= (rate * Time.deltaTime);

        
    }
}
