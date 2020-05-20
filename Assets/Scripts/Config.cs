using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static Config Instance;

    //Associated with file
    [Header("File")]
    public string nodeFile;
    public string edgeFile;
    public string keywordName;
    public List<List<string> > edgeFileInfo=new List<List<string>>();
    public List<List<string> > nodeFileInfo=new List<List<string>>();
    public List<string> titleInfo = new List<string>();

    //Associated with skybox
    [Header("Skybox")]
    public float skyboxRadius;
    public Transform nodePrefab;
    public Transform edgePrefab;
    public int lineSegment;

    //Associated with panel
    [Header("Panel")]
    public Transform panel1;

    //Associated with modules of node
    [Header("NodeList")]
    public List<Node> nodeList=new List<Node>();

    //Associated with layouts
    [Header("Layout")]
    public int mode;
    public float fdaSeparateRadius;
    public float fdaSpringLength;
    public float fdaCenter;
    public float fdaParentSpring;
    public float fdaMaxSpeed;
    public float fdaMaxAccelerate;
    public float layerRadius;
    public int layerMaxDepth;
    public float domeRadius;
    public float domeMaxDepth;

    //Associated with interactive
    [Header("Interactive")]
    public bool focused;
    public float focusedCameraLength;
    public float focusedElectricity;
    public bool highlighted;
    public Node rootNode;

    void Awake()
    {
        Instance = this;
    }

    public int getColumn(string m_title)
    {
        int i = 0;
        for (i = 0; i < titleInfo.Count; i++)
        {
            if (titleInfo[i] == m_title)
                break;
        }
        return i;
    }

    public int getNodeRow(string id) {
        int i = 0;
        for (i = 0; i < nodeFileInfo.Count; i++) {
            if (nodeFileInfo[i][getColumn("id")] == id)
                break;
        }
        return i;
    }

    public List<Node> getNeighbours(string id)
    {
        List<Node> nodes = new List<Node>();
        int tempRow = getNodeRow(id);
        for(int i = 0; i < nodeList.Count; i++)
        {
            if (i == tempRow)
                continue;
            if ((nodeList[i].transform.position - nodeList[tempRow].transform.position).magnitude <= fdaSeparateRadius)
                nodes.Add(nodeList[i]);
        }
        return nodes;
    }

    public void selectInit(string id)
    {
        int tempRow = getNodeRow(id);
        rootNode = nodeList[tempRow];
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (i == tempRow)
                nodeList[i].selected = true;
            else
                nodeList[i].selected = false;
            nodeList[i].grandSelected = false;
        }
    }

    public Vector3 getTreePos(float angle,float depth)
    {
        Vector3 pos=Vector3.zero;
        float r = depth * layerRadius;
        pos = new Vector3(r * Mathf.Cos(Mathf.PI * angle / 180f), r * Mathf.Sin(Mathf.PI * angle / 180f), skyboxRadius);
        return pos;
    }

    public Vector3 getDomePos(float angle,float depth)
    {
        Vector3 pos = Vector3.zero;
        float r = (1000f / depth);
        pos = new Vector3(r * Mathf.Cos(Mathf.PI * angle / 180f), skyboxRadius, r * Mathf.Sin(Mathf.PI * angle / 180f));
        return pos;
    }
    
}
