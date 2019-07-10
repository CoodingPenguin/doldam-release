using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장애물 이동에 관한 스크립트
/// </summary>
public class ObstacleScroll : MonoBehaviour
{

    private GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private float pixToUnit;
    private float width;
    private float height;

    private bool isHit;
    private bool isFlying;

    public Vector2 goToVec;
    public float flySpeed;
  
    void Start()
    {
        gameManager = GameManager.instance;
        spriteRenderer = GetComponent<SpriteRenderer>();

        pixToUnit = spriteRenderer.sprite.pixelsPerUnit;
        width = spriteRenderer.sprite.rect.width / pixToUnit;
        height = spriteRenderer.sprite.rect.height / pixToUnit;

        isHit = false;
        isFlying = false;

        flySpeed = 50f;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.gameState == GameManager.GameState.PLAY)
        {
            // 피버모드의 장애물 이동
            if (isFlying)
            {
                transform.Translate(goToVec * flySpeed * Time.deltaTime);
                if (transform.position.x + width / 2 < -10.8f / 2 ||
                    transform.position.x - width / 2 > 10.8f / 2 ||
                    transform.position.y - height / 2 > 19.2f / 2)
                    Destroy(gameObject);
            }
            // 평상시 모드의 장애물 이동
            else
            {
                transform.Translate(new Vector2(0, -Time.deltaTime * gameManager.GetScrollSpeed()));
                if (transform.position.y + height / 2 < -19.2f / 2)
                    Destroy(gameObject);
            }
        }
    }

    public void HitByPlayer()
    {
        isHit = true;
    }

    public bool GetIsHit()
    {
        return isHit;
    }

    public void HitAtFever()
    {
        float c = Random.Range(0, Mathf.PI * 2);
        goToVec = new Vector2(Mathf.Abs(Mathf.Cos(c)) * Mathf.Sign(transform.position.x), Mathf.Abs(Mathf.Sin(c)));
        isHit = true;
        isFlying = true;
    }
}
