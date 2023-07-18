using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aldo.PubSub;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject gameoverPanel;

    List<EnemyController> enemyList = new List<EnemyController>();

    bool isEnemyDetects = false;
    bool winCondition;

    private enum StageGame
    {
        Start, Gameplay, Gameover
    }
    private void Awake()
    {
        Subcribe();
    }
    private void OnDestroy()
    {
        UnSubribe();
    }
    private void Start()
    {
        SetStage(StageGame.Start);
        EnemyController[] _enemy = FindObjectsOfType<EnemyController>();
        if (_enemy.Length <= 0) return;
        for (int i = 0; i < _enemy.Length; i++)
            enemyList.Add(_enemy[i]);
    }

    private void SetStage(StageGame stage)
    {
        switch (stage)
        {
            case StageGame.Start:
                startPanel.SetActive(true);
                gameoverPanel.SetActive(false);
                gameplayUI.SetActive(false);
                Time.timeScale = 0;
                break;
            case StageGame.Gameplay:
                PublishSubscribe.Instance.Publish<MessageGameplayStart>(new MessageGameplayStart());
                startPanel.SetActive(false);
                gameoverPanel.SetActive(false);
                gameplayUI.SetActive(true);
                Time.timeScale = 1;
                break;
            case StageGame.Gameover:
                startPanel.SetActive(false);
                gameoverPanel.SetActive(true);
                gameplayUI.SetActive(false);
                Time.timeScale = 0;
                break;
        }
    }

    private IEnumerator EnemyDetect()
    {
        yield return new WaitForSeconds(15f);
        GameplayScene.Instance._winText.text = "YOU LOST";
        SetStage(StageGame.Gameover);
    }

    private void ReceiveMessageEnemyDie(MessageEnemyDie message)
    {
        enemyList.Remove(message._enemy);
        if (enemyList.Count <= 0)
        {
            GameplayScene.Instance._winText.text = "YOU WIN";
            SetStage(StageGame.Gameover);
        }
    }
    private void ReceiveMessageStartButtonPressed(MessageStartButtonPressed message)
    {
        SetStage(StageGame.Gameplay);
        PublishSubscribe.Instance.Subscribe<MessageEnemyDie>(ReceiveMessageEnemyDie);
    }
    private void ReceiveMessageShootMiss(MessageShootMiss message)
    {
        GameplayScene.Instance._arlertNotifObj.SetActive(true);
        if (!isEnemyDetects)
        {
            StartCoroutine(EnemyDetect()); 
            isEnemyDetects = true;
            
        }       
    }

    #region PubSub
    private void Subcribe()
    {
        PublishSubscribe.Instance.Subscribe<MessageEnemyDie>(ReceiveMessageEnemyDie);
        PublishSubscribe.Instance.Subscribe<MessageStartButtonPressed>(ReceiveMessageStartButtonPressed);
        PublishSubscribe.Instance.Subscribe<MessageShootMiss>(ReceiveMessageShootMiss);
    }
    private void UnSubribe()
    {
        PublishSubscribe.Instance.Unsubscribe<MessageEnemyDie>(ReceiveMessageEnemyDie);
        PublishSubscribe.Instance.Unsubscribe<MessageStartButtonPressed>(ReceiveMessageStartButtonPressed);
        PublishSubscribe.Instance.Subscribe<MessageShootMiss>(ReceiveMessageShootMiss);
    }
    #endregion
}
