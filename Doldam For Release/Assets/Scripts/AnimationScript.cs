using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 공 애니메이션 관련 스크립트
/// </summary>
public class AnimationScript : MonoBehaviour
{
    private GameManager gameManager;

    public bool loop;
    public float frameSeconds;  // 공의 스프라이트 프레임

    // Sprites를 불러올 path값
    public string locationExposed;
    public string locationCovered;

    private SpriteRenderer spriteRenderer;
    private Sprite[] sprites;

    private int frame = 0;
    private float deltaTime = 0;

    public float coveredScale;

    // 눈 사이즈에 따른 Sprite 변경 유무
    private bool isSetCovered;
    private bool isSetExposed;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;

        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = Resources.LoadAll<Sprite>(locationExposed);

        isSetExposed = false;
        isSetCovered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.gameState == GameManager.GameState.PLAY) 
        {
            deltaTime += Time.deltaTime;
            frameSeconds = Mathf.PI * sprites[0].rect.width / gameManager.GetScrollSpeed() / sprites.Length / 9.6f / 2.5f;

            if(PlayerMove.instance.ballScale > coveredScale && !isSetCovered)
            {
                Debug.Log("스프라이트 변경 : 공 크기 초과");
                sprites = null;
                sprites = Resources.LoadAll<Sprite>(locationCovered);
                isSetCovered = true;
                isSetExposed = false;
            }
            else if(PlayerMove.instance.ballScale < coveredScale && !isSetExposed)
            {
                Debug.Log("스프라이트 변경 :  공 크기 미만");
                sprites = null;
                sprites = Resources.LoadAll<Sprite>(locationExposed);
                isSetCovered = false;
                isSetExposed = true;
            }

            while (deltaTime>= frameSeconds)
            {
                deltaTime -= frameSeconds;
                frame++;

                frame %= sprites.Length;
            }

            spriteRenderer.sprite = sprites[frame];
        }
    }
}
