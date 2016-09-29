using Entitas;
using Entitas.CodeGenerator;

[Board,SingleEntity]
public sealed class GameBoardComponent : IComponent
{
    public int columns;
    public int rows;
}

