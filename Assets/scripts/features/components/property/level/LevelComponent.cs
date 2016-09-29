using Entitas;
using Entitas.CodeGenerator;

[Input,SingleEntity]
public sealed class LevelComponent : IComponent
{
    public int value;
}

