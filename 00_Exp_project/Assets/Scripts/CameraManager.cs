using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float m_time;
    public float m_facechangelength = 20.0f;
    public float anim_time;
    public float mimic_time;
    [SerializeField]
    Transform cameraPos;
    Vector3 cameraTrans;

    // Start is called before the first frame update
    void Start()
    {
        //anim first
        cameraTrans.x = 0.2f;
        cameraTrans.y = 0.15f;
        cameraTrans.z = 0;
        cameraPos.position = cameraTrans;

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;

        if(m_time >= anim_time)
        {
            cameraTrans.x = -0.2f;
            cameraPos.position = cameraTrans;
        }

        if(m_time >= m_facechangelength)
        {
            cameraTrans.x = 0.2f;
            cameraPos.position = cameraTrans;
            Init();

        }

        

        /*if(m_time >= m_facechangelength)
        {
            if (cameraTrans.x == 0.2f)
            {
                cameraTrans.x = -0.2f;
                cameraPos.position = cameraTrans;
            }
            else if (cameraTrans.x == -0.2f)
            {
                cameraTrans.x = 0.2f;
                cameraPos.position = cameraTrans;
            }
            m_time = 0f;
        }*/
    }

    public void Init()
    {
        m_time = 0f;
        anim_time = Random.Range(10f, 15f);
        mimic_time = m_facechangelength - anim_time;
    }
}
