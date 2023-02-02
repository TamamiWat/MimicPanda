using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCTRL : MonoBehaviour
{
    [SerializeField]
    GameObject mainCamera;      //���C���J�����i�[�p
    [SerializeField]
    GameObject subCamera;       //�T�u�J�����i�[�p 

    public float m_time;
    public float m_facechangelength = 5.0f;

    bool isMain;


    //�Ăяo�����Ɏ��s�����֐�
    void Start()
    {
        isMain = true;
        m_time = 0;
        //���C���J�����ƃT�u�J���������ꂼ��擾
        //mainCamera = GameObject.Find("MainCamera");
        //subCamera = GameObject.Find("SubCamera");
        mainCamera.SetActive(true);
        //�T�u�J�������A�N�e�B�u�ɂ���
        subCamera.SetActive(false);
    }


    //�P�ʎ��Ԃ��ƂɎ��s�����֐�
    void Update()
    {
        m_time += Time.deltaTime;

        if(m_time >= Time.deltaTime && mainCamera.activeSelf && isMain)
        {
            //�T�u�J�������A�N�e�B�u�ɐݒ�
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
            m_time = 0;            
        }else if (m_time >= Time.deltaTime && subCamera.activeSelf && !isMain)
        {
            //���C���J�������A�N�e�B�u�ɐݒ�
            mainCamera.SetActive(true);
            subCamera.SetActive(false);
            m_time = 0;
        }
        //�X�y�[�X�L�[��������Ă���ԁA�T�u�J�������A�N�e�B�u�ɂ���
        /*if (Input.GetKey("space"))
        {
            //�T�u�J�������A�N�e�B�u�ɐݒ�
            mainCamera.SetActive(false);
            subCamera.SetActive(true);
        }
        else
        {
            //���C���J�������A�N�e�B�u�ɐݒ�
            subCamera.SetActive(false);
            mainCamera.SetActive(true);
        }*/
    }
}