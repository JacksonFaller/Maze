using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class Element
    {
        public Transform[] ElementTransforms;
        public readonly List<Point> Positions;
        public int ElementsNumber;

        public Transform this[int index]
        {
            set { ElementTransforms[index] = value; }
            get { return ElementTransforms[index]; }
        }

        public Element(int count, Transform element)
        {
            Positions = new List<Point>(count);
            ElementTransforms = new Transform[count];
            for (int i = 0; i < count; i++)
            {
                ElementTransforms[i] = Object.Instantiate(element);
                Positions.Add(null);
            }
            ElementsNumber = count;
        }
    }
}
