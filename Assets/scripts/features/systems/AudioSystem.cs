using System;
using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class AudioSystem : IInitializeSystem, ISetPool
{
    readonly GameObject _container = new GameObject("AudioSource");
    Pool _pool;
    public void SetPool(Pool pool)
    {
        _pool = pool;
        var efx = _container.AddComponent<AudioSource>();
        var music = _container.AddComponent<AudioSource>();
        pool.GetGroup(InputMatcher.EfxSound).OnEntityAdded += (group, entity, index, component) =>
        {
            efx.clip = pool.audioCache.clips[entity.efxSound.clip];
            efx.volume = 0.7f;
            efx.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            efx.Play();
            _pool.DestroyEntity(entity);
        };
        pool.GetGroup(InputMatcher.Music).OnEntityAdded += (group, entity, index, component) =>
        {
            music.clip = pool.audioCache.clips[entity.music.clip];
            music.volume = 0.3f;
            music.loop = true;
            music.Play();
        };
        pool.GetGroup(InputMatcher.Music).OnEntityRemoved += (group, entity, index, component) =>
        {
            music.Stop();
        };
    }

    public void Initialize()
    {
        addAudio();
    }

    void addAudio()
    {
        var audioCacheDict = new Dictionary<Res.audios, AudioClip>();
        var names = Enum.GetNames(typeof(Res.audios));
        for (int i = 0; i < names.Length; i++)
        {
            audioCacheDict[(Res.audios)i] = Resources.Load<AudioClip>(Res.audioPath + (Res.audios)i);
        }
        _pool.CreateEntity()
            .AddAudioCache(audioCacheDict);
    }
}

