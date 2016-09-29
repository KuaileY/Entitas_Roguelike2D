using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class GameBoardSystem:IInitializeSystem,ISetPools
{
    Pool _inputPool;
    Pools _pools;
    public void SetPools(Pools pools)
    {
        _pools = pools;
        _inputPool = pools.input;
    }
    public void Initialize()
    {
        var gameBoard = _pools.board.SetGameBoard(Res.columns, Res.rows).gameBoard;
        //创建地图
        BoardSetup(gameBoard);
        //暂停键盘输入
        _inputPool.isProcessing = true;
    }

    //创建背景地图
    void BoardSetup(GameBoardComponent gameBoard)
    {
        for (int x = -1; x <= gameBoard.columns; x++)
        {
            for (int y = -1; y <= gameBoard.rows; y++)
            {
                bool edge = x == -1 || x == Res.columns || y == -1 || y == Res.rows;
                string prefab = edge ? Res.outerWalls.Random() : Res.floors.Random();
                _pools.board.CreateEntity()
                    .AddPosition(x, y)
                    .AddAsset(prefab);
            }
        }
    }

    

    
}

