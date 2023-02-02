using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{
    public float m_time;
    public float m_facechangelength = 5.0f;

    [SerializeField]
    GameObject mimic_model;
    [SerializeField]
    GameObject anim_model;

    bool isMmodel;

    // Start is called before the first frame update
    void Start()
    {
        anim_model.SetActive(true);
        mimic_model.SetActive(false);
        isMmodel = false;
        m_time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;

        if(m_time >= m_facechangelength && !isMmodel)
        {
            mimic_model.SetActive(true);
            anim_model.SetActive(false);
            
            m_time = 0f;
        }
        else if(m_time >= m_facechangelength && isMmodel)
        {
            anim_model.SetActive(true);
            mimic_model.SetActive(false);
            m_time = 0f;
        }
    }
}
