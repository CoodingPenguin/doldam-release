using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneStartBtnAnimation : MonoBehaviour
{
    public Color32 colors;
    private float speed = 1f;
    private float transparentRatio = 0f;
    private bool transparentRatioIsIncreasing = true;

    // value를 입력해줄 때 증가상태이면 계속 증가, 감소상태이면 계속 감소하도록 하는 역할을 Get Set으로 구현함.
    // ratio * 255 값으로 투명도를 적용한다.
    public float TransparentRatio
    {
        get { return transparentRatio; }

        // transparentRatio는 0~1의 값을 가진다
        set
        {
            if (transparentRatioIsIncreasing)
            {
                transparentRatio += value;
                if(transparentRatio >= 1)
                {
                    transparentRatio = 1f;
                    transparentRatioIsIncreasing = false;
                }
            }

            else
            {
                transparentRatio -= value;
                if (transparentRatio <= 0)
                {
                    transparentRatio = 0f;
                    transparentRatioIsIncreasing = true;
                }
            }
        }
    }

    private void Start()
    {
        colors = this.gameObject.GetComponent<Text>().color;
    }

    private void Update()
    {
        TransparentRatio = speed * Time.deltaTime;
        this.gameObject.GetComponent<Text>().color = new Color32(colors.r, colors.g, colors.b, (byte)(TransparentRatio * 255));
    }
}
