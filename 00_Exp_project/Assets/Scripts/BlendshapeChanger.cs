using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlendshapeChanger : MonoBehaviour
{
    [SerializeField]
    public SkinnedMeshRenderer faceskin;
    [SerializeField]
    public SkinnedMeshRenderer jawskin;
    [SerializeField]
    public SkinnedMeshRenderer tongueskin;

    public float m_time;
    public float m_facechangelength = 5.0f;
    public bool isAnimate;
    public bool isInit;
    public bool isStart;    //only first time
    public int shapeNum = 0;    //target expression index num
    public bool isChange;   //compare prev face number
    public int shapenum;    //index num of face blendshape
    public int curnum;      //change face blendshape index num
    float val1 = 0f;        //face value
    float val2 = 0f;        //jaw value
    float val3 = 0f;        //teeth value


    // Start is called before the first frame update
    void Start()
    {
        isStart = true;
        m_time = 0.0f;
        isAnimate = false;
        isInit = true;
        isChange = true;
        shapenum = faceskin.sharedMesh.blendShapeCount; //get face blendshape  index num
    }

    // Update is called once per frame
    void Update()
    {
        m_time += Time.deltaTime;


        //first_expression
        if (isStart)
        {
            Debug.Log("first_expression");
            SetBlendshape(7);
            if (m_time >= m_facechangelength)
            {
                Debug.Log("5seconds left");
                isStart = false;
                m_time = 0.0f;
                //setBlendShapeNum();
            }
        }

        //normal
        if (!isStart)
        {
            if (isInit && !isAnimate)
            {
                isInit = false;
                isAnimate = true;
                setBlendShapeNum();
            }
            if(isAnimate && m_time >= m_facechangelength)
            {
                isAnimate = false;
                StartCoroutine(InitBlendShape());
                isInit = true;

                m_time = 0.0f;
            }
            //m_time = 0.0f;
        }
    }

    public void InitBlendshape()
    {
        for (int i = 0; i < faceskin.sharedMesh.blendShapeCount; i++) {
            faceskin.SetBlendShapeWeight(i, 0);
        }
        for (int i = 0; i < jawskin.sharedMesh.blendShapeCount; i++) {
            jawskin.SetBlendShapeWeight(i, 0);
        }
        for (int i = 0; i < tongueskin.sharedMesh.blendShapeCount; i++) {
            tongueskin.SetBlendShapeWeight(i, 0);
        }

    }

    public void SetBlendshape(int num)
    {
        //isInit = false;
        //isAnimate = true;

        switch (num){
            case 0://eyes open mouth close
                curnum = shapenum - 6;
                val1 = 100;
                val2 = 0;
                val3 = 0;
                StartCoroutine(SetBlendShapeChange());/*setBlendShapeChange(shapenum - 6, 1, 0, 100, 0, 0);*/
                break;
            case 1://eyes open mouth open
                curnum = shapenum - 5;
                val1 = 100;
                val2 = 0;
                val3 = 0;
                StartCoroutine(SetBlendShapeChange());
                break;
            case 2://eyes close mouth close
                curnum = shapenum - 4;
                val1 = 100;
                val2 = 0;
                val3 = 0;
                StartCoroutine(SetBlendShapeChange());
                break;
            case 3://eyes close mouth open
                curnum = shapenum - 3;
                val1 = 100;
                val2 = 0;
                val3 = 0;
                StartCoroutine(SetBlendShapeChange());
                break;
            case 4://eyes close teeth show
                curnum = shapenum - 2;
                val1 = 100;
                val2 = 70;
                val3 = 70;
                StartCoroutine(SetBlendShapeChange());
                break;
            case 5://eyes open teeth show
                curnum = shapenum - 1;
                val1 = 100;
                val2 = 70;
                val3 = 70;
                StartCoroutine(SetBlendShapeChange());
                break;
            default:
                break;
        }
    }

    public void setBlendShapeNum()
    {
        int prev_num = 7;
        shapeNum = Random.Range(0, 6);

        if(prev_num == shapeNum)
        {
            isChange = false;
        }else if(prev_num != shapeNum)
        {
            isChange = true;
            prev_num = shapeNum;
        }

        Debug.Log("face case:" + shapeNum);
        SetBlendshape(shapeNum);
        Debug.Log("func setBlendShapeNum is called.");
    }

    IEnumerator SetBlendShapeChange()
    {
        int span = 10;
        float span_f = val1 / span;
        float span_j = val2 / span;
        float span_t = val3 / span;
        float w_f = 0;
        float w_j = 0;
        float w_t = 0;

        //init process
        /*if (!isInit)
        {
            for (int i = 0; i < span; i++)
            {
                w_f = Mathf.MoveTowards(val1, 0, val1 - i * span_f);
                faceskin.SetBlendShapeWeight(curnum, w_f);

                w_j = Mathf.MoveTowards(val2, 0, val2 - i * span_j);
                jawskin.SetBlendShapeWeight(1, w_j);

                w_t = Mathf.MoveTowards(val3, 0, val3 - i * span_t);
                tongueskin.SetBlendShapeWeight(0, w_t);
                yield return new WaitForSeconds(0.02f);
            }

            isInit = true;
        }*/

        
        //normal process
        for (int i = 0; i <= span; i++)
        {
            w_f = Mathf.MoveTowards(0, val1, i * span_f);
            faceskin.SetBlendShapeWeight(curnum, w_f);

            w_j = Mathf.MoveTowards(0, val2, i * span_j);
            jawskin.SetBlendShapeWeight(1, w_j);

            w_t = Mathf.MoveTowards(0, val3, i * span_t);
            tongueskin.SetBlendShapeWeight(0, w_t);
            yield return new WaitForSeconds(0.02f);
        }       
        
    }

    IEnumerator InitBlendShape()
    {
        int span = 10;
        float span_f = val1 / span;
        float span_j = val2 / span;
        float span_t = val3 / span;
        float w_f = 0;
        float w_j = 0;
        float w_t = 0;
        for (int i = 0; i <= span; i++)
        {
            w_f = Mathf.MoveTowards(val1, 0, val1 - i * span_f);
            faceskin.SetBlendShapeWeight(curnum, w_f);

            w_j = Mathf.MoveTowards(val2, 0, val2 - i * span_j);
            jawskin.SetBlendShapeWeight(1, w_j);

            w_t = Mathf.MoveTowards(val3, 0, val3 - i * span_t);
            tongueskin.SetBlendShapeWeight(0, w_t);
            yield return new WaitForSeconds(0.02f);
        }

        //isInit = true;
        Debug.Log("Init called");
        //m_time = 0.0f;
    }
}
