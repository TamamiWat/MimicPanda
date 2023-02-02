using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndProgram : MonoBehaviour
{
    float m_time;

    [SerializeField]
    float m_endtime;
    [SerializeField]
    GameObject plane;
    // Start is called before the first frame update
    void Start()
    {
        m_time = 0f;
        plane.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;

        if (m_time >= m_endtime)
        {
            plane.SetActive(true);
            //Application.Quit();
        }
    }
}
