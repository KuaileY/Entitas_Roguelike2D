using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class CreateGameBoardCacheSystem : ISystem, ISetPools
{
    Pool _pool;
    public void SetPools(Pools pools)
    {
        _pool = pools.core;
        //初始化GameBoardCache
        var gameBoard = pools.board.GetGroup(BoardMatcher.GameBoard);
        gameBoard.OnEntityAdded += (group, entity, index, component) =>
        {
            CreateGameBoardCache((GameBoardComponent)component);
        };
        gameBoard.OnEntityUpdated += (group, entity, index, previousComponent, newComponent) =>
        {
            CreateGameBoardCache((GameBoardComponent)newComponent);
        };


        //添加Items
        var gameItem = _pool.GetGroup(Matcher.AllOf(CoreMatcher.Interactive, CoreMatcher.Position));
        gameItem.OnEntityAdded += (group, entity, index, component) =>
        {
            TestLoadConfig.log.Trace("gameItem.OnEntityAdded");
            var grid = _pool.gameBoardCache.grid;
            grid[entity.position.x, entity.position.y] = entity;
            _pool.ReplaceGameBoardCache(grid);
        };

        gameItem.OnEntityRemoved += (group, entity, index, component) =>
        {
            TestLoadConfig.log.Trace("gameItem.OnEntityRemoved");
            var pos = component as PositionComponent;
            if (pos == null)
                pos = entity.position;
            var grid = _pool.gameBoardCache.grid;
            grid[pos.x, pos.y] = null;
            pools.core.ReplaceGameBoardCache(grid);
        };
    }

    void CreateGameBoardCache(GameBoardComponent gameBoard)
    {
        var grid = new Entity[gameBoard.columns, gameBoard.rows];
        foreach (var entity in _pool.GetEntities(CoreMatcher.Interactive))
        {
            grid[entity.position.x, entity.position.y] = entity;
        }
        _pool.ReplaceGameBoardCache(grid);
    }

}
