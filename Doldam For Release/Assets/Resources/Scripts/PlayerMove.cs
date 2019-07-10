using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove instance;

    public bool isDead; // 게임오버 여부

    private int level;  // fever 여부


    private float width;
    private float ScreenWidth;

    public float moveSpeed;     // 얼마나 빨리 움직일지
    public float rotSpeed;      // 얼마나 빨리 회전할지
    public float rollSpeed;     // 얼마나 빨리 공을 굴릴지
    public float scaleSpeed;    // 얼마나 빨리 커질지

    public float ballScale; // 공의 크기
    public float minScale;  // 최소 크기

    private int dir;    
    private int beforeDir;
    private float distance;



    public float feverDuration;
    public float scaleAtFever;
    public float goToFeverScale;
    private float feverTime = 0f;

    public float snowManScale;
    public float obstacleScale;


    public GameObject SnowParticle;
    public GameObject feverParticle;
    public GameObject rockParticle;
    public GameObject treeParticle;
    public GameObject snowmanParticle;



    public bool feverIsEnd = false;


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        level = 0;
        isDead = false;
        distance = 0f;
        width = GetComponent<SpriteRenderer>().sprite.rect.width / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        ScreenWidth = 10.8f - GameObject.FindGameObjectWithTag("SideWall").GetComponent<SpriteRenderer>().sprite.rect.width / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        Debug.Log("width:" + width);
        Debug.Log("ScreenWidth:" + ScreenWidth);

    }

    void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.PLAY)
        {
            distance += ballScale * rollSpeed * Time.deltaTime;
            //Debug.Log("Ball Scale : " + ballScale);

            if (distance >= 19.2 / 10)
            {
                GameManager.instance.AddScore(50);
                distance -= 19.2f / 10;
            }

            if (isDead)
            {
                Debug.Log("게임오버");
                if (GameManager.instance.gameState == GameManager.GameState.PLAY)
                {
                    GameManager.instance.gameState = GameManager.GameState.GAMEOVER;
                    // 게임 오버시
                }
            }

            GameManager.instance.SetScrollSpeed(ballScale * (rollSpeed + level / 10));
            
            // Level 1. 평상시
            if (GameManager.instance.feverState == 0)
            {
                SetScale(ballScale + scaleSpeed * Time.deltaTime);
                MoveAndRotate();

                if (ballScale >= goToFeverScale)
                { 
                    GameManager.instance.feverState = 1;
                    SetScale(goToFeverScale);
                    GameObject particle = Instantiate(feverParticle, transform.position, Quaternion.identity);
                    particle.transform.localScale = particle.transform.localScale * ballScale / 2;
                    Destroy(particle, 2f);
                    level += 1;
                }

            }
            // Level 2. 평상시에서 피버모드로 전환하는 중
            else if (GameManager.instance.feverState == 1) 
            {
                Debug.Log("평상시 -> 피버모드");
                transform.position = Vector2.Lerp(transform.position, new Vector2(0, transform.position.y), 7 * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, 7 * Time.deltaTime);
                transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(scaleAtFever, scaleAtFever), 7 * Time.deltaTime);

                if (transform.localScale.x >= scaleAtFever - 1)
                {
                    GameManager.instance.feverState = 2;
                    transform.position = new Vector2(0, transform.position.y);
                    transform.rotation = Quaternion.identity;
                }
                GameManager.instance.SetScrollSpeed(5);
            }
            // Level 3. 피버모드
            else if (GameManager.instance.feverState == 2)
            {
                Debug.Log("피버모드");
                if (GameManager.instance.score > 100000) feverDuration = 2.0f;
                else if (GameManager.instance.score > 200000) feverDuration = 1.5f;

                MoveAndRotate();
                SetScale(scaleAtFever);
                feverTime += Time.deltaTime;

                // 피버모드 종료
                if (feverTime > feverDuration) 
                {
                    GameObject particle = Instantiate(SnowParticle, transform.position, Quaternion.identity);
                    particle.transform.localScale = particle.transform.localScale * ballScale / 2.5f;
                    Destroy(particle, 2f);
                    SetScale(5);
                    GameManager.instance.feverState = 0;
                    feverTime = 0;
                    feverIsEnd = true;
                }
            }
            KeepInScreen();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (GameManager.instance.gameState == GameManager.GameState.PLAY)
        {
            ObstacleScroll obstacleScroll = col.gameObject.GetComponent<ObstacleScroll>();

            Debug.Log("트리거 작동?");

            if (obstacleScroll != null)
            {
                if (!obstacleScroll.GetIsHit())
                {
                    if (GameManager.instance.feverState == 0)
                    {
                        if (col.tag == "Snowman")
                        {              
                            GameObject particle = Instantiate(snowmanParticle, col.transform.position, Quaternion.identity);
                            Destroy(particle, 2f);
                            obstacleScroll.HitByPlayer();
                            SetScale(ballScale + snowManScale);
                            Destroy(col.gameObject);
                            GameManager.instance.AddScore(1000);
                        }
                        else if (col.tag == "Rock" || col.tag == "Tree")
                        {
                            
                            GameObject particle = Instantiate(SnowParticle, col.transform.position, Quaternion.identity);
                            particle.transform.localScale = particle.transform.localScale * ballScale / 4;
                            Destroy(particle, 2f);
                            obstacleScroll.HitByPlayer();
                            SetScale(ballScale - obstacleScale);
                        }
                    }
                    if (GameManager.instance.feverState == 2)
                    {
                        
                        obstacleScroll.HitAtFever();
                        GameObject particle;
                        if (col.tag == "Rock")
                            particle = Instantiate(rockParticle, col.transform.position, Quaternion.identity);
                        else if (col.tag == "Tree")
                            particle = Instantiate(treeParticle, col.transform.position, Quaternion.identity);
                        else// if (col.tag == "Snowman")
                            particle = Instantiate(snowmanParticle, col.transform.position, Quaternion.identity);
                        particle.transform.localScale = particle.transform.localScale * 1.5f;
                        Destroy(particle, 2f);
                        GameManager.instance.AddScore(2000);    // 2000점
                    }


                }
            }
        }
    }

    /// <summary>
    /// 공의 크기를 설정하는 함수
    /// </summary>
    /// <param name="scale"></param>
    void SetScale(float scale)
    {
        if (scale <= minScale)
        {
            scale = minScale;
            isDead = true;
            return;
        }
        ballScale = scale;
        transform.localScale = new Vector2(ballScale, ballScale);
    }

    /// <summary>
    /// 공을 움직이고 회전시키기 위한 함수
    /// </summary>
    void MoveAndRotate()
    {
        dir = 0;

        // 왼쪽으로 기울일 경우
        if (InputLeft())
        {
            transform.position += new Vector3(moveSpeed + ballScale / 5, 0) * Time.deltaTime * Input.acceleration.x * 4;
            dir = -1;
            beforeDir = -1;
            if (transform.rotation.z <= 0.2 && transform.rotation.z >= -0.2)
                transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
        }

        // 오른쪽으로 기울일 경우
        if (InputRight())
        {
            transform.position += new Vector3(moveSpeed + ballScale / 5, 0) * Time.deltaTime * Input.acceleration.x * 4;
            dir = 1;
            beforeDir = 1;
            if (transform.rotation.z <= 0.2 && transform.rotation.z >= -0.2)
                transform.Rotate(Vector3.forward * -rotSpeed * Time.deltaTime);
        }

        if (dir == 0)
            transform.Rotate(new Vector3(0, 0, rotSpeed / 1.5f * beforeDir) * Time.deltaTime);

        if (beforeDir * transform.rotation.z > 0)
        {
            transform.rotation = Quaternion.identity;
            beforeDir = 0;
        }
    }

    /// <summary>
    /// 공이 스크린 밖을 벗어나는 것을 방지하기 위한 함수
    /// </summary>
    void KeepInScreen()
    {
        if (transform.position.x - width * ballScale / 2 < -ScreenWidth / 2)
            transform.position = new Vector2(width * ballScale / 2 - ScreenWidth / 2, transform.position.y);
        else if (transform.position.x + width * ballScale / 2 > ScreenWidth / 2)
            transform.position = new Vector2(ScreenWidth / 2 - width * ballScale / 2, transform.position.y);
    }

    /// <summary>
    /// 왼쪽으로 기울였는지를 판단하는 함수
    /// </summary>
    /// <returns></returns>
    bool InputLeft()
    {
        // 게임 환경이 모바일인 경우
        if (GameManager.instance.testEnvironment == GameManager.TestEnvironment.MOBILE)
        {
            if (Input.acceleration.x < -0.05)
                return true;
            return false;
        }
        // 게임 환경이 PC인 경우
        else if (GameManager.instance.testEnvironment == GameManager.TestEnvironment.PC)
        {
            if (Input.GetKey(KeyCode.A))
                return true;
            return false;
        }

        return false;
    }

    /// <summary>
    /// 오른쪽으로 기울였는지를 판단하는 함수
    /// </summary>
    /// <returns></returns>
    bool InputRight()
    {
        // 게임 환경이 모바일인 경우
        if (GameManager.instance.testEnvironment == GameManager.TestEnvironment.MOBILE)
        {
            if (Input.acceleration.x > 0.05)
                return true;
            return false;
        }
        // 게임 환경이 PC인 경우
        else if (GameManager.instance.testEnvironment == GameManager.TestEnvironment.PC)
        {
            if (Input.GetKey(KeyCode.D))
                return true;
            return false;
        }

        return false;
    }
}
