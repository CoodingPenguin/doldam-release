using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 뒤의 배경을 자연스럽게 움직이기 위한 스크립트
/// </summary>
public class BgScroll : MonoBehaviour
{

    private GameManager gameManager;
    private float pixToUnit;
    private float screenHeight;

    public GameObject bg1;
    public GameObject bg2;
    public GameObject wall1;
    public GameObject wall2;


    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.instance;
        pixToUnit = bg1.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        screenHeight = 19.2f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameManager.GameState.PLAY)
        {
            // 계속해서 위로 움직인다
            bg1.transform.Translate(new Vector2(0, -Time.deltaTime * gameManager.GetScrollSpeed()));
            bg2.transform.Translate(new Vector2(0, -Time.deltaTime * gameManager.GetScrollSpeed()));
            wall1.transform.Translate(new Vector2(0, -Time.deltaTime * gameManager.GetScrollSpeed()));
            wall2.transform.Translate(new Vector2(0, -Time.deltaTime * gameManager.GetScrollSpeed()));

            // 범위를 벗어나면 초기 위치로 이동한다
            if (bg1.transform.position.y < -screenHeight)
                bg1.transform.Translate(new Vector2(0, screenHeight * 2));
            if (bg2.transform.position.y < -screenHeight)
                bg2.transform.Translate(new Vector2(0, screenHeight * 2));
            if (wall1.transform.position.y < -screenHeight)
                wall1.transform.Translate(new Vector2(0, screenHeight * 2));
            if (wall2.transform.position.y < -screenHeight)
                wall2.transform.Translate(new Vector2(0, screenHeight * 2));
        }
    }
}
