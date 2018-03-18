using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Effects
{
    interface IEffectEvents : IEventSystemHandler
    {
        void OnPickup();
        void OnExpire();
    }
}
