using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace UnityTemplateProjects.Util
{
    public class ButtonSelector : MonoBehaviour
    {
        public Sprite unselected;
        public Sprite selected;

        private SpriteRenderer _renderer;

        public List<Transform> path;

        [NonSerialized]
        public bool isSelected = false;

        public float duration = 5f;

        public UnityEvent onClick;

        private Vector3[] vector3s;
        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (path.Count >= 0)
            {
                 vector3s = new Vector3[path.Count];

                for (int i = 0; i < path.Count; i++)
                {
                    vector3s[i] = path[i].position;
                }

                transform.DOPath(vector3s, duration).OnComplete(doSuperParth);
            }
        }

        private void doSuperParth()
        {
            transform.DOPath(vector3s, duration).OnComplete(doSuperParth);
        }

        public void ToogleFocus()
        {
            isSelected = !isSelected;

            _renderer.sprite = isSelected ? selected : unselected;
        }

        public void Interact()
        {
            onClick.Invoke();
        }
    }
}