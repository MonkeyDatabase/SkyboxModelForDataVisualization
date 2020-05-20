using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    [Header("Custom")]
    public List<Node> parentList;
    public List<Node> sonList;
    public List<Edge> edgeList;
    public List<string> myInfo;
    public Vector3 myPos;

    [Header("Tranfer")]
    public float infectivity = 0.1f;
    public float immunity = 0.2f;
    public bool infected = false;

    [Header("Interactive")]
    public bool drag = false;
    public bool showTip = false;

    [Header("Fda Attributes")]
    public Vector3 speed;
    public float pushAdded = 1;
    public Vector3 accelerate;

    [Header("Tree Attributes")]
    public bool selected;
    public bool grandSelected;
    public float depth;
    public float fromAngle;
    public float possessAngle;

    // Update is called once per frame
    void Update()
    {
        //Init data
        if(Config.Instance.mode!=2&&Config.Instance.mode!=3)
            myPos = this.transform.position;

        //transfer the information
        if (infected) {
            infectivity += 0.001f;
            transform.Find("Canvas").Find("Text").GetComponent<Text>().text = name+" illed";
            transform.Find("Canvas").Find("Text").GetComponent<Text>().color = Color.red;
        }
        else
        {
            transform.Find("Canvas").Find("Text").GetComponent<Text>().text = name;
        }
        if (!showTip)
        {
            //Associate with layout
            switch (Config.Instance.mode)
            {
                case 1:
                    Fda();
                    break;
                case 2:
                    treeSon();
                    break;
                case 3:
                    domeSon();
                    break;
            }

            this.transform.position = myPos.normalized * Config.Instance.skyboxRadius;
        }
        

        if(drag&&Config.Instance.mode==0)
            this.transform.position = Camera.main.ScreenPointToRay(Input.mousePosition).direction.normalized * Config.Instance.skyboxRadius;

        //final processing
        this.transform.LookAt(Vector3.zero); 
    }

    void domeSon()
    {
        if ((selected == false && grandSelected == false) || depth > Config.Instance.layerMaxDepth)
        {

            myPos = new Vector3(Config.Instance.skyboxRadius,-Config.Instance.skyboxRadius, -Config.Instance.skyboxRadius);
            grandSelected = false;
            return;
        }
        if (selected)
        {
            this.possessAngle = 360;
            this.fromAngle = 0;
            this.depth = 0;
            this.myPos = new Vector3(0,Config.Instance.skyboxRadius,0);
        }
        float singleangle = possessAngle / (float)sonList.Count;
        float thisangle = fromAngle;
        for (int i = 0; i < sonList.Count; i++)
        {
            if (sonList[i].selected || sonList[i].grandSelected)
                continue;
            sonList[i].grandSelected = true;
            sonList[i].fromAngle = thisangle;
            sonList[i].possessAngle = singleangle;
            sonList[i].depth = depth + 1f;
            sonList[i].myPos = Config.Instance.getDomePos(sonList[i].fromAngle + sonList[i].possessAngle / 2f, sonList[i].depth);

            //houchuli
            thisangle += singleangle;
        }
    }

    void OnMouseEnter()
    {
        showTip = true;
        pushAdded = 13;
    }
    void OnMouseExit()
    {
        showTip = false;
        pushAdded = 1;
    }

    void OnGUI()
    {
        if (showTip)
        {
            GUI.Label(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, 400, 400), "    " + name + "\n" + "           " + myInfo[Config.Instance.getColumn("des")]);
        }
    }

    void OnMouseDown()
    {
        if (Config.Instance.mode == 0)
            drag = !drag;
        if (Config.Instance.mode != 2)
            Config.Instance.selectInit(myInfo[0]);
    }

    void treeSon()
    {
        if ((selected==false&&grandSelected==false)|| depth > Config.Instance.layerMaxDepth)
        {
            
            myPos = new Vector3(Config.Instance.skyboxRadius, 0, -Config.Instance.skyboxRadius);
            grandSelected = false;
            return;
        }
        if (selected)
        {
            this.possessAngle = 360;
            this.fromAngle = 0;
            this.depth = 0;
            this.myPos = Config.Instance.getTreePos(fromAngle + possessAngle / 2f, depth);
        }
        float singleangle = possessAngle / (float)sonList.Count;
        float thisangle = fromAngle;
        for(int i = 0; i < sonList.Count; i++)
        {
            if (sonList[i].selected || sonList[i].grandSelected)
                continue;
            sonList[i].grandSelected = true;
            sonList[i].fromAngle = thisangle;
            sonList[i].possessAngle = singleangle;
            sonList[i].depth = depth + 1f;
            sonList[i].myPos = Config.Instance.getTreePos(sonList[i].fromAngle + sonList[i].possessAngle / 2f, sonList[i].depth);
                      
            //houchuli
            thisangle += singleangle;
        }
    }

    void Fda()
    {
        accelerate = geneFda();
        if (accelerate != Vector3.zero)
        {
            myPos += (speed * Time.deltaTime + accelerate * Time.deltaTime * Time.deltaTime / 2f);
            speed += accelerate * Time.deltaTime;
        }    
    }

    Vector3 geneFda()
    {
        Vector3 dir = Vector3.zero;
        dir +=  springFda() + Config.Instance.fdaParentSpring*parentspringFda() + centerFda() + coulumbFda();
        return dir;
    }

    Vector3 springFda()
    {
        Vector3 dir=Vector3.zero;
        List<Node> my_neithbours = Config.Instance.getNeighbours(myInfo[0]);
        for (int i = 0; i < my_neithbours.Count; i++)
        {
            Vector3 thisLength = my_neithbours[i].transform.position-myPos;
            float deltaLenth = thisLength.magnitude - Config.Instance.fdaSpringLength;
            dir += deltaLenth * thisLength.normalized * my_neithbours[i].pushAdded;
        }
        return dir;
    }

    Vector3 parentspringFda()
    {
        Vector3 dir = Vector3.zero;
        for(int i = 0; i < parentList.Count; i++)
        {
            dir += (parentList[i].transform.position - myPos);
        }
        return dir;
    }

    Vector3 coulumbFda()
    {
        Vector3 dir=Vector3.zero;
        List<Node> my_neithbours = Config.Instance.getNeighbours(myInfo[0]);
        for(int i = 0; i < my_neithbours.Count; i++)
        {
            Vector3 thisLength = my_neithbours[i].transform.position - myPos;
            if (thisLength.magnitude != 0)
                dir += thisLength.normalized*(pushAdded * my_neithbours[i].pushAdded) / thisLength.magnitude / thisLength.magnitude;
        }
        return dir;
    }

    Vector3 centerFda()
    {
        Vector3 dir = Vector3.zero;
        dir = new Vector3(0,0,Config.Instance.skyboxRadius) - myPos;
        return dir*Config.Instance.fdaCenter;
    }
}
