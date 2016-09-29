using Entitas;
using Entitas.CodeGenerator;

[Core,SingleEntity]
public sealed class GameBoardCacheComponent:IComponent
{
    public Entity[,] grid;
}

