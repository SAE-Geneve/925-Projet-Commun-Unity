using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/PlayerIt")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "PlayerIt", message: "[Npc] hit [player]", category: "Events", id: "0f1998aea5727c11b6812aaa0ea5db14")]
public sealed partial class PlayerIt : EventChannel<GameObject, GameObject> { }

