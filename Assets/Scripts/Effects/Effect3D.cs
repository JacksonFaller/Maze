using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Effects
{
    [RequireComponent(typeof(Collider), typeof(MeshRenderer))]
    class Effect3D : EffectBase
    {
        private Collider _collider;
        private MeshRenderer _meshRenderer;

        void Awake()
        {
            _collider = GetComponent<Collider>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        void OnTriggerEnter(Collider other)
        {
            Collected(other.gameObject);
            _collider.enabled = false;
            _meshRenderer.enabled = false;
        }
    }
}
