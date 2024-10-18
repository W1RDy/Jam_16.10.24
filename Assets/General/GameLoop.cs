using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] _gameLoopObjs;

    private void Start()
    {
        foreach (var gameLoopObj in _gameLoopObjs)
        {
            if (gameLoopObj is IGameLoopComponent gameLoopComp)
            {
                gameLoopComp.StarLoop();
            }
            else throw new System.ArgumentException(gameLoopObj.name + " doesn't realise IInitializable interface!");
        }
    }
}
