using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage_Goal : MonoBehaviour
{
    private BoxCollider Goal;
    [SerializeField] private Define.Scene NextScene;

    private void Awake()
    {
        Goal = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("클리어");
            StageClear();
        }
    }
    public void StageClear()
    {
        Main.Game.ClearGame();
        Main.Scenes.LoadScene(NextScene);
    }
}
