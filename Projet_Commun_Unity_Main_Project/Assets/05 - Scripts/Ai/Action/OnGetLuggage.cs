using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/OnGetLuggage")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "OnGetLuggage", message: "[Agent] received luggage", category: "Events", id: "5f3a83c5016b947d14e844296ad96ff3")]
public sealed partial class OnGetLuggage : EventChannel<GameObject> { }

