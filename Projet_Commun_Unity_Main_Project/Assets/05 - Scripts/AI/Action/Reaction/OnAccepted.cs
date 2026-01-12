using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/OnAccepted")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "OnAccepted", message: "NPC is accepted", category: "Events", id: "d76d1847f77abcdaba1ef7468039b659")]
public sealed partial class OnAccepted : EventChannel { }

