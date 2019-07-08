using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{
    private GameManager gameManager;

    // 얼마나 떨어진 후에 오브젝트를 Spawn할 것인가
    public float objectSpawn; 
    private float objectEdis;

    // 게임 오브젝트 종류
    public GameObject rock;
    public GameObject tree;
    public GameObject treeWithSnow;
    public GameObject snowMan;
    public GameObject sillySnowMan;

    private bool isSnowTurn;
    private bool isSnowTurnTemp1;
    private bool isSnowTurnTemp2;

    private float screenWidth;
    private float screenHeight;
    private float pixToUnit;

    void Start()
    {
        gameManager = GameManager.instance;

        pixToUnit = rock.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        screenHeight = 19.2f;
        screenWidth = 10.8f - GameObject.FindGameObjectWithTag("SideWall").GetComponent<SpriteRenderer>().sprite.rect.width / 100;  // 벽을 제외한 범위

        objectEdis = 0f;
        objectSpawn = 10f;
        isSnowTurn = true;
    }

    void Update()
    {
        if (GameManager.instance.gameState == GameManager.GameState.PLAY)
        {
            objectEdis += Time.deltaTime * gameManager.GetScrollSpeed();

            if (objectSpawn <= objectEdis)
            {
                // 어떤 종류의 오브젝트를 Spawn할 것인가
                if (isSnowTurn)
                {
                    float b = Random.Range(0f, 1f);
                    if (b < 0.5)
                        SpawnObstacle(snowMan);
                    else
                        SpawnObstacle(sillySnowMan);
                }
                else
                {
                    float r = Random.Range(0f, 1f);
                    if (r < 0.5)
                        SpawnObstacle(rock);
                    else
                    {
                        float k = Random.Range(0f, 1f);
                        if (k < 0.5)
                            SpawnObstacle(tree);
                        else
                            SpawnObstacle(treeWithSnow);
                    }

                }

                // 피버모드가 끝날 때 바로 오브젝트가 나오지 않게 한다.
                float g;
                if (GameObject.Find("Player").GetComponent<PlayerMove>().feverIsEnd == true)
                {
                    g = 2.0f;
                    GameObject.Find("Player").GetComponent<PlayerMove>().feverIsEnd = false;
                }
                // 피버모드가 아닌 경우 점수대에 따라서 오브젝트 간의 거리를 다르게 한다.
                else
                {
                    g = Random.Range(0.8f, 1.2f);

                    if (gameManager.score > 50000)
                        g = Random.Range(0.5f, 1.2f);
                    else if (gameManager.score > 100000)
                        g = Random.Range(0.5f, 0.8f);
                    else if (gameManager.score > 200000)
                        g = Random.Range(0.6f, 1.0f);
                }

                // 더 많이 뺄수록 오브젝트 간의 거리가 늘어난다.
                objectEdis -= objectSpawn * g;

                // 패턴 변화
                isSnowTurnTemp1 = isSnowTurnTemp2;
                isSnowTurnTemp2 = isSnowTurn;

                if (gameManager.score > 150000)
                {
                    if (isSnowTurnTemp1 == true && isSnowTurnTemp2 == false)
                        isSnowTurn = false;
                    else if (isSnowTurnTemp1 == false && isSnowTurnTemp2 == true)
                        isSnowTurn = false;
                    else if (isSnowTurnTemp1 == false && isSnowTurnTemp2 == false)
                        isSnowTurn = true;
                }
                else
                    isSnowTurn = !isSnowTurn;

            }
        }
    }

    /// <summary>
    /// GameObject를 Spawn하는 함수
    /// </summary>
    /// <param name="target"></param>
    void SpawnObstacle(GameObject target)
    {
        SpriteRenderer spriteRenderer = target.GetComponent<SpriteRenderer>();

        float pixToUnit = spriteRenderer.sprite.pixelsPerUnit;
        float width = spriteRenderer.sprite.rect.width / pixToUnit;
        float height = spriteRenderer.sprite.rect.height / pixToUnit;

        GameObject spawnTarget = Instantiate(
            target,
            new Vector2(Random.Range(-screenWidth / 2 + width / 2, screenWidth / 2 - width / 2), screenHeight / 2 + height / 2),
            Quaternion.identity);
    }
}
