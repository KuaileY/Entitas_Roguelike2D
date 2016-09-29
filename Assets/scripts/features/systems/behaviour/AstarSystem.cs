using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

public sealed class AstarSystem : IReactiveSystem, ISetPools
{
    public TriggerOnEvent trigger { get { return CoreMatcher.Astar.OnEntityAdded(); } }
    Dictionary<Entity, List<Node>> paths = new Dictionary<Entity, List<Node>>();
    public List<Node> openList = new List<Node>();
    public List<Node> closeList = new List<Node>();

    Pools _pools;
    Group _occupyEntities;
    Node[,] _map;
    public void SetPools(Pools pools)
    {
        _pools = pools;
        _occupyEntities = pools.core.GetGroup(CoreMatcher.Occupy);
    }

    public void Execute(List<Entity> entities)
    {
        TestLoadConfig.log.Trace(string.Format("AstarSystem Execute:{0}", entities.Count));
        #region 检测玩家周边是否阻塞
        var player = _pools.core.controlableEntity;
        var boardCache = _pools.core.gameBoardCacheEntity.gameBoardCache;
        if (player.Around(boardCache))
        {
            TestLoadConfig.log.Trace("Player around!!");
            foreach (var e in entities)
            {
                e.IsAstar(false);
            }
            TestLoadConfig.log.Trace("AIEnd.");
            _pools.input.isActiveAI = false;
            _pools.input.isProcessing = false;
            return;
        }
        #endregion

        //构建Astar tiled map,
        Map();
        //选择最近可移动敌人，建立下一步坐标
        SetPostion(entities);
    }

    void SetPostion(ICollection<Entity> enemyList)
    {
        foreach (var enemy in enemyList)
        {
            PathFind(enemy);
        }
        Move();
    }

    //移动
    void Move()
    {
        if (paths.Count > 0)
        {
            //排序，选择路线最短的enemy
            var sortPaths = paths.OrderBy(r => r.Value.Count).ToDictionary(r => r.Key, r => r.Value);
            var first = sortPaths.First();

            //移动并移除Astar
            first.Key.ReplacePosition(first.Value[0].x, first.Value[0].y);
            first.Key.IsMove(true);
            first.Key.IsAstar(false);

            TestLoadConfig.log.DebugFormat("{0}:x: {1},y: {2}", sortPaths.First().Key.view.gameObject.name,
                first.Value[0].x, first.Value[0].y);
            //建立阻塞
            setNode(new Vector2(first.Value[0].x, first.Value[0].y), GridType.obstacle);
            //移除已经移动的enemy
            sortPaths.Remove(sortPaths.First().Key);
            //清除列表，剩下enemy继续执行操作
            var enemys = sortPaths.Keys;
            paths.Clear();
            SetPostion(enemys);
        }
        else
            foreach (var occupy in _occupyEntities.GetEntities())
                occupy.IsOccupy(false);

    }

    //寻找路径
    void PathFind(Entity enemy)
    {
        Setup(enemy);
        StartSearch(enemy);
    }

    //开始搜索
    void StartSearch(Entity enemy)
    {
        Node grid = (Node)openList[0];
        while (grid.gridType != GridType.end)
        {
            #region //没有路径,原地移动
            if (openList.Count == 0)
            {
                TestLoadConfig.log.Trace(string.Format("{0} is Stop!", enemy.view.gameObject.name));
                enemy.IsAstar(false);
                enemy.IsMove(true);
                return;
            }
            #endregion
            //排序，从最近端开始
            openList.Sort();
            //1.选择第一个格子进行搜索
            grid = (Node)openList[0];
            //2.判断是否为终点，追踪路径
            if (grid.gridType == GridType.end)
            {
                TestLoadConfig.log.Trace("Find way!");
                RetracePath(enemy, enemy.view.gameObject.transform.position,
                    _pools.core.controlableEntity.view.gameObject.transform.position);
                return;
            }
            #region //3.搜索当前周边格子
            Search(grid, (i, j) =>
            {
                int x = grid.x + i;
                int y = grid.y + j;
                //如果格子不越界，不是障碍，不在关闭列表
                if (x >= 0 && x < _pools.board.gameBoard.columns && y >= 0 && y < _pools.board.gameBoard.rows
                    && _map[x, y].gridType != GridType.obstacle
                    && !closeList.Contains(_map[x, y]))
                {
                    _map[x, y].x = x;
                    _map[x, y].y = y;
                    //重新计算G值并分情况比较
                    int g = grid.G + (int)(Mathf.Sqrt(Mathf.Abs(i)
                                                       + Mathf.Abs(j)) * 10);
                    if (!openList.Contains(_map[x, y]))
                    {
                        _map[x, y].G = g;
                        _map[x, y].H = Manhattan(x, y);
                        _map[x, y].F = _map[x, y].G + _map[x, y].H;
                        // 【重点】记录“我从哪里来”
                        _map[x, y].parent = grid;
                        // 【重点】将新搜索的格子放到开启列表中
                        openList.Add(_map[x, y]);
                    }
                    else if (_map[x, y].G > g)
                    {
                        _map[x, y].G = g;
                        _map[x, y].F = _map[x, y].G + _map[x, y].H;
                        _map[x, y].parent = grid;
                    }
                }

            });
            #endregion
        }
    }

    //追踪路径
    void RetracePath(Entity enemy, Vector2 startPos, Vector2 endPos)
    {
        var path = new List<Node>();
        Node currentNode = _map[(int)endPos.x, (int)endPos.y];
        Node startNode = _map[(int)startPos.x, (int)startPos.y];

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        //路径列表添加路径
        paths.Add(enemy, path);
        TestLoadConfig.log.Trace(string.Format("PathsCount:{0}", paths.Count));
    }

    //搜索上下左右
    void Search(Node grid, Action<int, int> action)
    {
        for (int i = -1; i <= 1; i++)
        {
            if (i != 0)
            {
                action.Invoke(i, 0);
                action.Invoke(0, i);
            }
        }
        closeList.Add(grid);
        openList.Remove(grid);
    }

    //建立起始点
    void Setup(Entity enemy)
    {
        openList.Clear();
        closeList.Clear();

        var startPos = enemy.view.gameObject.transform.position;
        var endPos = _pools.core.controlableEntity.view.gameObject.transform.position;
        setNode(startPos, GridType.start);
        setNode(endPos, GridType.end);
        //已经运动的节点为阻塞节点
        foreach (var occupy in _occupyEntities.GetEntities())
        {
            setNode(occupy.view.gameObject.transform.position, GridType.obstacle);
        }
        //计算起始点H值
        _map[(int)startPos.x, (int)startPos.y].H = Manhattan((int)startPos.x, (int)startPos.y);
        openList.Add(_map[(int)startPos.x, (int)startPos.y]);
    }

    //建立map某一Node类型
    void setNode(Vector2 pos, GridType type)
    {
        int nodex = (int)pos.x;
        int nodey = (int)pos.y;
        _map[nodex, nodey].x = nodex;
        _map[nodex, nodey].y = nodey;
        _map[nodex, nodey].gridType = type;
    }

    //曼哈顿启发函数
    int Manhattan(int x, int y)
    {
        return (int)(Mathf.Abs(_pools.board.gameBoard.columns - x)
                      + Mathf.Abs(_pools.board.gameBoard.rows - y)) * 10;
    }

    //建立tile map
    void Map()
    {
        int columns = _pools.board.gameBoard.columns;
        int rows = _pools.board.gameBoard.rows;
        _map = new Node[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                _map[i, j] = new Node();
                if (_pools.core.gameBoardCache.grid[i, j] == null)
                    _map[i, j].gridType = GridType.normal;
                else
                {
                    //Astar元素区域为可通过区域
                    if (_pools.core.gameBoardCache.grid[i, j].isAstar)
                        _map[i, j].gridType = GridType.normal;
                    else
                        _map[i, j].gridType = GridType.obstacle;
                }
            }
        }
    }


}

public enum GridType
{
    normal,
    start,
    end,
    obstacle
}

public class Node : IComparable
{
    public int x;
    public int y;
    public int F;//路线经由当前格子的总评估值
    public int G;//从起始点到当前格子的最小消耗值
    public int H;//从当前格子到终点的消耗评估值

    public GridType gridType;//格子类型
    public Node parent;  // "我从哪里来"
    public int CompareTo(object obj)
    {
        Node g = (Node)obj;
        if (F < g.F)
            return -1;
        else if (F > g.F)
            return 1;
        else
            return 0;
    }
}