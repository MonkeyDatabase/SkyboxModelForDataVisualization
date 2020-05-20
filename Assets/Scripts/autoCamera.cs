using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class autoCamera : MonoBehaviour
{
    public float speed;
    public float y;
    public float R;
    private Vector3 pos;
    private float temp;
    // Start is called before the first frame update
    void Start()
    {
        temp = 0;
        pos = Vector3.zero;   
    }

    // Update is called once per frame
    void Update()
    {
        temp += speed;
        pos.y = y;
        pos.x = R * Mathf.Cos(temp / 50000 * Mathf.PI);
        pos.z = R * Mathf.Sin(temp / 50000 * Mathf.PI);

        this.transform.position = pos;
        this.transform.LookAt(new Vector3(0, 0, 0));                
    }
}
