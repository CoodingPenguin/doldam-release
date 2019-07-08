using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 파티클이 오브젝트가 움직일 때 같이 움직이도록 하는 것 =
/// </summary>
public class ParticleScroll : MonoBehaviour
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (gameManager.gameState == GameManager.GameState.PLAY)
        {
            transform.Translate(new Vector2(0, -Time.deltaTime * gameManager.GetScrollSpeed()) / 2);
        }
    }
}
