
using UnityEngine;

public static class Res
{
    #region Property---------

    public const int initPoints = 100;
    public const int foodPoints = 10;
    public const int sodaPoints = 2;
    public const int enemy1Dmg = -1;
    public const int enemy2Dmg = -2;

    public const int WallsDestructible = 4;

    #endregion

    #region GameBoard------

    public const int columns = 8;
    public const int rows = 8;
    public const int foodCountMin = 1;
    public const int foodCountMax = 5;
    public const int wallCountMin = 5;
    public const int wallCountMax = 9;

    #endregion

    #region Path---------

    public const string prefabsPath = "Prefabs/GameElements/";
    public const string animationsPath = "Animation/Animations/";
    public const string tileNumsPath = "Sprites/FontTileNum";
    public const string ten = "Ten";
    public const string single = "Single";

    #endregion

    #region  render----------

    public const string exit = "Exit";
    public const string player = "Player";
    public const string enemy1 = "Enemy1";
    public const string enemy2 = "Enemy2";
    public const string soda = "Soda";
    public const string num = "Num";

    public static readonly string[] enemies =
    {
        "Enemy1",
        "Enemy2"
    };

    public static readonly string[] floors =
    {
        "Floor1",
        "Floor2",
        "Floor3",
        "Floor4",
        "Floor5",
        "Floor6",
        "Floor7",
        "Floor8"
    };

    public static readonly string[] foods =
    {
        "Food",
        "Soda"
    };

    public static readonly string[] outerWalls =
    {
        "OuterWall1",
        "OuterWall2",
        "OuterWall3",
    };

    public static readonly string[] walls =
    {
        "Wall1",
        "Wall2",
        "Wall3",
        "Wall4",
        "Wall5",
        "Wall6",
        "Wall7",
        "Wall8"
    };

    public static readonly string[] damageWalls =
    {
        "Scavengers_SpriteSheet_48",
        "Scavengers_SpriteSheet_49",
        "Scavengers_SpriteSheet_50",
        "Scavengers_SpriteSheet_51",
        "Scavengers_SpriteSheet_52",
        "Scavengers_SpriteSheet_52", //used twice
        "Scavengers_SpriteSheet_53",
        "Scavengers_SpriteSheet_54"
    };

    public static readonly string[] TileNums =
    {
        "0",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9"
    };

    #endregion

    #region animation-------

    public enum animations
    {
        Attack,
        Hit
    }

    #endregion

    #region View--------

    public static readonly UIType GameStart = new UIType("Prefabs/Views/GameStartView");
    public static readonly UIType GameOver = new UIType("Prefabs/Views/GameOverView");
    public static readonly UIType Options = new UIType("Prefabs/Views/OptionsView");
    public static readonly UIType Level = new UIType("Prefabs/Views/LevelView");
    public static readonly UIType Food = new UIType("Prefabs/Views/FoodView");

    #endregion

    #region audio------

    public const string audioPath = "Audio/";
    public enum audios
    {
        //efx
        scavengers_chop1,
        scavengers_chop2,
        scavengers_die,
        scavengers_enemy1,
        scavengers_enemy2,
        scavengers_footstep1,
        scavengers_footstep2,
        scavengers_fruit1,
        scavengers_fruit2,
        scavengers_soda1,
        scavengers_soda2,
        //music
        scavengers_music
    }
    #endregion

}
