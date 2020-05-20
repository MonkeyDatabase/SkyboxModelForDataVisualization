using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public float speed = 5;
    public Camera camera;
    public GameObject innerCamera;
    public GameObject outerCamera;
    public GameObject canzhao;
    public GameObject canzhao2;
    bool isOut = false;
    bool isFar = true;
    bool controlCam = true;
    // Start is called before the first frame update
    void Start()
    {
        innerCamera.SetActive(true);
        outerCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //mode
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            if (Config.Instance.mode == 1)
                Config.Instance.mode = 0;
            else
            {
                Config.Instance.mode = 1;
            }
            canzhao.SetActive(true);
            canzhao2.SetActive(false);            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && Config.Instance.rootNode != null)
        {
            Config.Instance.mode = 1;
            canzhao.SetActive(true);
            canzhao2.SetActive(false);
            Config.Instance.mode = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && Config.Instance.rootNode != null)
        {
            Config.Instance.mode = 1;
            canzhao.SetActive(false);
            canzhao2.SetActive(true);
            Config.Instance.mode = 3;
        }
        //infect
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Config.Instance.rootNode.infected = true;
        }
        

        //聚焦
        Zoom3();
        
        //摄像机
        if (Input.GetKeyDown(KeyCode.F)){
            controlCam = !controlCam;
        }
        if (controlCam)
        {
            float x = Input.GetAxis("Mouse X");
            camera.transform.Rotate(Vector3.up * x * speed, Space.World);

            float y = Input.GetAxis("Mouse Y");
            camera.transform.Rotate(Vector3.right * -y * speed);
        }

        //外视角
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isOut = !isOut;
            if (isOut)
            {
                innerCamera.SetActive(false);
                outerCamera.SetActive(true);
            }
            else
            {
                innerCamera.SetActive(true);
                outerCamera.SetActive(false);
            }
        }

    }


    private void Zoom3()
    {
        
        if (Input.GetMouseButtonDown(1))
        {
            //修改缩放等级
            isFar = !isFar;
        }
        if (isFar)
        {
            //拉远 20  --》 60        Lerp(起点、终点、比例)
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 80, 0.1f);
        }
        else
        {
            //拉近 60 --》 20
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 25, 0.1f);
        }
    }

}
