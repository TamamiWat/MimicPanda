using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCTRL : MonoBehaviour
{
    [SerializeField]
    GameObject mainCamera;      //メインカメラ格納用
    [SerializeField]
    GameObject subCamera;       //サブカメラ格納用 

    public float m_time;
    public float m_facechangelength = 5.0f;

    bool isMain;


    //呼び出し時に実行される関数
    void Start()
    {
        isMain = true;
        m_time = 0;
        //メインカメラとサブカメラをそれぞれ取得
        //mainCamera = GameObject.Find("MainCamera");
        //subCamera = GameObject.Find("SubCamera");
        mainCamera.SetActive(true);
        //サブカメラを非アクティブにする
        subCamera.SetActive(false);
    }


    //単位時間ごとに実行される関数
    void Update()
    {
        m_time += Time.deltaTime;

        if(m_time >= Time.deltaTime && mainCamera.activeSelf && isMain)
        {
            //サブカメラをアクティブに設定
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
            m_time = 0;            
        }else if (m_time >= Time.deltaTime && subCamera.activeSelf && !isMain)
        {
            //メインカメラをアクティブに設定
            mainCamera.SetActive(true);
            subCamera.SetActive(false);
            m_time = 0;
        }
        //スペースキーが押されている間、サブカメラをアクティブにする
        /*if (Input.GetKey("space"))
        {
            //サブカメラをアクティブに設定
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
        }
        else
        {
            //メインカメラをアクティブに設定
            subCamera.SetActive(false);
            mainCamera.SetActive(true);
        }*/
    }
}