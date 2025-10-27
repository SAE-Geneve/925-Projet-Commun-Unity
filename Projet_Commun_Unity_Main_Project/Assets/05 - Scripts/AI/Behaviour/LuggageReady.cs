using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/LuggageReady")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "LuggageReady", message: "[self] luggage is [ready]", category: "Events", id: "0ea9f89c45964dd4ccb49901f16103f9")]
public sealed partial class LuggageReady : EventChannel<GameObject, bool> { }

