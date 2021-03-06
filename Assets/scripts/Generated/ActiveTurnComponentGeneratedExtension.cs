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
        static readonly ActiveTurnComponent activeTurnComponent = new ActiveTurnComponent();

        public bool isActiveTurn {
            get { return HasComponent(InputComponentIds.ActiveTurn); }
            set {
                if (value != isActiveTurn) {
                    if (value) {
                        AddComponent(InputComponentIds.ActiveTurn, activeTurnComponent);
                    } else {
                        RemoveComponent(InputComponentIds.ActiveTurn);
                    }
                }
            }
        }

        public Entity IsActiveTurn(bool value) {
            isActiveTurn = value;
            return this;
        }
    }

    public partial class Pool {
        public Entity activeTurnEntity { get { return GetGroup(InputMatcher.ActiveTurn).GetSingleEntity(); } }

        public bool isActiveTurn {
            get { return activeTurnEntity != null; }
            set {
                var entity = activeTurnEntity;
                if (value != (entity != null)) {
                    if (value) {
                        CreateEntity().isActiveTurn = true;
                    } else {
                        DestroyEntity(entity);
                    }
                }
            }
        }
    }
}

    public partial class InputMatcher {
        static IMatcher _matcherActiveTurn;

        public static IMatcher ActiveTurn {
            get {
                if (_matcherActiveTurn == null) {
                    var matcher = (Matcher)Matcher.AllOf(InputComponentIds.ActiveTurn);
                    matcher.componentNames = InputComponentIds.componentNames;
                    _matcherActiveTurn = matcher;
                }

                return _matcherActiveTurn;
            }
        }
    }
