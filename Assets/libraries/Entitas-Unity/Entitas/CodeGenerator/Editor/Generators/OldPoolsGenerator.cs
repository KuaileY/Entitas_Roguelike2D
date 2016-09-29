﻿using System.Linq;
using System;

namespace Entitas.CodeGenerator {

    [Obsolete("Since 0.32.0. It's recommended to use the new PoolsGenerator. Use the OldPoolsGeneratorFor only for existing project.")]
    public class OldPoolsGenerator : IPoolCodeGenerator {

        const string CLASS_TEMPLATE = @"using Entitas;

public static class Pools {{
{0}{1}
}}";

        const string ALL_POOLS_GETTER = @"
    static Pool[] _allPools;

    public static Pool[] allPools {{
        get {{
            if (_allPools == null) {{
                _allPools = new [] {{ {0} }};
            }}
            return _allPools;
        }}
    }}  ";

        const string GETTER = @"

    static Pool _{0};

    public static Pool {0} {{
        get {{
            if (_{0} == null) {{
                _{0} = new Pool({1}.TotalComponents, 0, new PoolMetaData(""{2}Pool"", {1}.componentNames, {1}.componentTypes));
                #if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
                if (UnityEngine.Application.isPlaying) {{
                    var poolObserver = new Entitas.Unity.VisualDebugging.PoolObserver(_{0});
                    UnityEngine.Object.DontDestroyOnLoad(poolObserver.entitiesContainer);
                }}
                #endif
            }}

            return _{0};
        }}
    }}";
        public CodeGenFile[] Generate(string[] poolNames) {
            var allPools = string.Format(ALL_POOLS_GETTER,
                string.Join(", ", poolNames.Select(poolName => poolName.LowercaseFirst()).ToArray()));

            var getters = poolNames.Aggregate(string.Empty, (acc, poolName) =>
                                acc + string.Format(GETTER, poolName.LowercaseFirst(), poolName.PoolPrefix() + CodeGenerator.DEFAULT_COMPONENT_LOOKUP_TAG,
                                poolName.IsDefaultPoolName() ? string.Empty : poolName.PoolPrefix() + " "));

            return new [] { new CodeGenFile(
                    "Pools",
                    string.Format(CLASS_TEMPLATE, allPools, getters).ToUnixLineEndings(),
                    GetType().FullName
                )
            };
        }
    }
}