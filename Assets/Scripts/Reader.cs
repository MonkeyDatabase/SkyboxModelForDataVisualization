using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reader : MonoBehaviour
{
    void Start()
    {
        TextAsset nodeData = Resources.Load<TextAsset>(Config.Instance.nodeFile);        
        string[] nodedata = nodeData.text.Split(new char[] { '\n' });
        for (int i = 0; i < nodedata.Length-1; i++) {
            if (i == 0) {
                string[] titledata = nodedata[i].Split(new char[] { ',' });
                for (int j = 0; j < titledata.Length; j++) {
                    Config.Instance.titleInfo.Add(titledata[j]);
                }
            }
            else
            {
                List<string> tempCol = new List<string>();
                string[] nodecoldata = nodedata[i].Split(new char[] { ',' });
                for (int j = 0; j < nodecoldata.Length; j++) {
                    tempCol.Add(nodecoldata[j]);
                }
                Config.Instance.nodeFileInfo.Add(tempCol);
            }
        }
        TextAsset edgeData = Resources.Load<TextAsset>(Config.Instance.edgeFile);
        string[] edgedata = edgeData.text.Split(new char[] { '\n' });
        for (int i = 1; i < edgedata.Length - 1; i++) {
            string[] edgecoldata = edgedata[i].Split(new char[] { ',' });
            List<string> tempCol = new List<string>();
            for (int j = 0; j < edgecoldata.Length; j++)
            {
                tempCol.Add(edgecoldata[j]);
            }
            Config.Instance.edgeFileInfo.Add(tempCol);
        }
        Transform prefab;
        for (int i = 0; i < Config.Instance.nodeFileInfo.Count; i++) {
            prefab= Instantiate(Config.Instance.nodePrefab, randomPosition(), Quaternion.identity);
            prefab.GetComponent<Node>().myInfo = new List<string>(Config.Instance.nodeFileInfo[i]);
            prefab.parent = Config.Instance.panel1;
            prefab.name = Config.Instance.nodeFileInfo[i][Config.Instance.getColumn("name")];
            Config.Instance.nodeList.Add(prefab.GetComponent<Node>());
        }

        for (int i = 0; i < Config.Instance.edgeFileInfo.Count; i++) {
            prefab=Instantiate(Config.Instance.edgePrefab,Vector3.zero, Quaternion.identity);
            prefab.name = Config.Instance.edgeFileInfo[i][0] + "->" + Config.Instance.edgeFileInfo[i][1];
            prefab.GetComponent<Edge>().from = Config.Instance.nodeList[Config.Instance.getNodeRow(Config.Instance.edgeFileInfo[i][0])].transform;
            prefab.GetComponent<Edge>().to = Config.Instance.nodeList[Config.Instance.getNodeRow(Config.Instance.edgeFileInfo[i][1])].transform;
            prefab.GetComponent<Edge>()._info = Config.Instance.edgeFileInfo[i][2];
            Config.Instance.nodeList[Config.Instance.getNodeRow(Config.Instance.edgeFileInfo[i][0])].sonList.Add(Config.Instance.nodeList[Config.Instance.getNodeRow(Config.Instance.edgeFileInfo[i][1])]);
            Config.Instance.nodeList[Config.Instance.getNodeRow(Config.Instance.edgeFileInfo[i][0])].edgeList.Add(prefab.GetComponent<Edge>());
            Config.Instance.nodeList[Config.Instance.getNodeRow(Config.Instance.edgeFileInfo[i][1])].parentList.Add(Config.Instance.nodeList[Config.Instance.getNodeRow(Config.Instance.edgeFileInfo[i][0])]);
        }
    }

    private Vector3 randomPosition()
    {
        Vector3 pos = Vector3.zero;
        while(pos.x==0||pos.y==0||pos.z<=0||pos.normalized.y>0.8||pos.normalized.y<-0.6||pos.x<-0.7||pos.x>0.7)
            pos = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        return pos.normalized*Config.Instance.skyboxRadius;
    }
}
