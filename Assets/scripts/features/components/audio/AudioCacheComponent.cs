using System.Collections.Generic;
using Entitas;
using Entitas.CodeGenerator;
using UnityEngine;

[Input, SingleEntity]
public sealed class AudioCacheComponent : IComponent
{
    public Dictionary<Res.audios, AudioClip> clips;
}

