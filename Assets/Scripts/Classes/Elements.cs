using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class Elements : IEnumerable<Element>
    {
        private Element[] _elements;
        private Text _counter;

        public int ElementsCount { get; private set; }

        public Element this[int index]
        {
            get { return _elements[index]; }
        }

        public Elements(int count, Transform element, Text counter)
        {
            _elements = new Element[count];
            _counter = counter;
            _counter.text = count.ToString();
            for (int i = 0; i < count; i++)
                _elements[i] = new Element(Object.Instantiate(element));
            ElementsCount = count;
        }

        public void IncrementCounter()
        {
            ElementsCount++;
            UpdateCounter();
        }

        public void DecrementCounter()
        {
            ElementsCount--;
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            _counter.text = ElementsCount.ToString();
        }

        public IEnumerable<Element> ToEnumerable()
        {
            for(int i = 0; i < _elements.Length; i++)
            {
                yield return _elements[i];
            }
        }

        public IEnumerator<Element> GetEnumerator()
        {
            return ToEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _elements.GetEnumerator();
        }
    }

    public class Element
    {
        public Transform Transform { get; set; }
        public Point Position { get; set; }

        public Element()
        {
        }

        public Element(Transform transform, Point position)
        {
            Transform = transform;
            Position = position;
        }

        public Element(Transform transform) : this(transform, null)
        {
        }
    }
}
