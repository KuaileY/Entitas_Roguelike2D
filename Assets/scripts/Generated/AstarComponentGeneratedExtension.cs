//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGenerator.ComponentExtensionsGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Entitas;

namespace Entitas {
    public partial class Entity {
        static readonly AstarComponent astarComponent = new AstarComponent();

        public bool isAstar {
            get { return HasComponent(CoreComponentIds.Astar); }
            set {
                if (value != isAstar) {
                    if (value) {
                        AddComponent(CoreComponentIds.Astar, astarComponent);
                    } else {
                        RemoveComponent(CoreComponentIds.Astar);
                    }
                }
            }
        }

        public Entity IsAstar(bool value) {
            isAstar = value;
            return this;
        }
    }
}

    public partial class CoreMatcher {
        static IMatcher _matcherAstar;

        public static IMatcher Astar {
            get {
                if (_matcherAstar == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.Astar);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherAstar = matcher;
                }

                return _matcherAstar;
            }
        }
    }