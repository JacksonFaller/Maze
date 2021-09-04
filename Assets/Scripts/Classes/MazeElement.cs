using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Classes
{
    [Serializable]
    public class MazeElement
    {
        [SerializeField]
        public Transform _transform;

        [SerializeField]
        private int _count;

        [SerializeField]
        private Text _counter;

        public int Count { get => _count; set => _count = value; }
        public Transform Transform { get => _transform; set => _transform = value; }

        public Text Counter { get => _counter; set => _counter = value; }
    }
}
