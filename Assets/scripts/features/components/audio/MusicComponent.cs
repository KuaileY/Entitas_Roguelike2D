using Entitas;
using Entitas.CodeGenerator;

[Input,SingleEntity]
public sealed class MusicComponent : IComponent
{
    public Res.audios clip;
}

