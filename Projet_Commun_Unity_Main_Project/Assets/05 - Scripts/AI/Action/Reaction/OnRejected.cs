using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/OnRejected")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "OnRejected", message: "NPC is rejected", category: "Events", id: "3fa29981ed14072cbde986d0a64825de")]
public sealed partial class OnRejected : EventChannel { }

