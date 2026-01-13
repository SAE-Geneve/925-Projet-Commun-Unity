using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/HasArrived")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "HasArrived", message: "When the npc arrived at his destination", category: "Events", id: "e219016a576cf505baabc9491e15d01a")]
public sealed partial class HasArrived : EventChannel { }

