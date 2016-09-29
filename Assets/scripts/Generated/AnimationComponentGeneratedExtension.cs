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
        public AnimationComponent animation { get { return (AnimationComponent)GetComponent(CoreComponentIds.Animation); } }

        public bool hasAnimation { get { return HasComponent(CoreComponentIds.Animation); } }

        public Entity AddAnimation(string newTrigger) {
            var component = CreateComponent<AnimationComponent>(CoreComponentIds.Animation);
            component.trigger = newTrigger;
            return AddComponent(CoreComponentIds.Animation, component);
        }

        public Entity ReplaceAnimation(string newTrigger) {
            var component = CreateComponent<AnimationComponent>(CoreComponentIds.Animation);
            component.trigger = newTrigger;
            ReplaceComponent(CoreComponentIds.Animation, component);
            return this;
        }

        public Entity RemoveAnimation() {
            return RemoveComponent(CoreComponentIds.Animation);
        }
    }
}

    public partial class CoreMatcher {
        static IMatcher _matcherAnimation;

        public static IMatcher Animation {
            get {
                if (_matcherAnimation == null) {
                    var matcher = (Matcher)Matcher.AllOf(CoreComponentIds.Animation);
                    matcher.componentNames = CoreComponentIds.componentNames;
                    _matcherAnimation = matcher;
                }

                return _matcherAnimation;
            }
        }
    }
