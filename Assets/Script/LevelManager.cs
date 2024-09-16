using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemys;
    [SerializeField] float cameraSize;

    GameManager GM;

    private void Start()
    {
        GM = GameManager.instance;
        Camera.main.orthographicSize = cameraSize;
    }

    public void DeathSignal()
    {
        StartCoroutine(CheckEnemyCO());
    }

    IEnumerator CheckEnemyCO()
    {
        yield return new WaitForSeconds(0.05f);
        if (GameObject.FindGameObjectWithTag("Player") == null && GameObject.FindGameObjectWithTag("HumanPlayer") == null)
        {
            GM.LoseGame();
            Destroy(this);
        }
        else
        {
            bool completeGame = true;
            foreach (GameObject enemy in enemys)
            {
                if (enemy != null)
                {
                    completeGame = false;
                }
            }

            if (completeGame)
            {
                GM.NextLevel();
            }
        }
    }
}
