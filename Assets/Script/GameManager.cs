using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Animator sceneTransition;
    [SerializeField] GameObject[] levels;
    int currentLevel;
    GameObject currentLevelObj;
    float cd;


    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        currentLevelObj = Instantiate(levels[currentLevel], transform.position, Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetGame();
        }
        cd -= Time.deltaTime;
    }

    public void ResetGame()
    {
        if(cd <= 0)
        {
            StartCoroutine(ReadyToResetCO());
            cd = 0.5f;
        }
    }

    public void LoseGame()
    {
        if (cd <= 0)
        {
            text.gameObject.SetActive(true);
            text.color = Color.gray;
            text.text = "Failed";
            StartCoroutine(ReadyToResetCO());
            cd = 0.5f;
        }
    }

    IEnumerator ReadyToResetCO()
    {
        yield return new WaitForSeconds(0.25f);
        sceneTransition.SetTrigger("start");
        yield return new WaitForSeconds(0.75f);
        sceneTransition.SetTrigger("end");
        Destroy(currentLevelObj);
        currentLevelObj = Instantiate(levels[currentLevel], transform.position, Quaternion.identity);
        text.gameObject.SetActive(false);
    }

    public void NextLevel()
    {
        if (cd <= 0)
        {
            text.gameObject.SetActive(true);
            text.color = new Color(1, 0.8f, 0.3f);
            text.text = "Complete Level";
            StartCoroutine(ReadyToNextLevelCO());
            cd = 0.5f;
        }
    }
    IEnumerator ReadyToNextLevelCO()
    {
        yield return new WaitForSeconds(0.25f);
        sceneTransition.SetTrigger("start");
        currentLevel++;
        levelText.text = "LEVEL: " + (currentLevel + 1) + "/" + levels.Length;
        yield return new WaitForSeconds(0.75f);
        sceneTransition.SetTrigger("end");
        Destroy(currentLevelObj);
        if (currentLevel < levels.Length)
        {
            currentLevelObj = Instantiate(levels[currentLevel], transform.position, Quaternion.identity);
            text.gameObject.SetActive(false);
        }
        else
        {
            CompleteGame();
        }
    }

    void CompleteGame()
    {
        text.text = "You have clear all level!\n\nGame design, art, code, sound: Duc Anh\n\nThank you for playing";
    }
}
