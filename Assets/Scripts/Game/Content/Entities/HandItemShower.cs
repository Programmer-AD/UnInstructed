using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uninstructed.Game.Content.Enums;
using Uninstructed.Game.Main;
using UnityEngine;

namespace Uninstructed.Game.Content.Entities
{
    public class HandItemShower : MonoBehaviour
    {
        [SerializeField]
        private Entity entity;

        private new SpriteRenderer renderer;
        private ItemType showingType;


        public void Reset()
        {
            entity = null;
        }

        public void Start()
        {
            renderer = GetComponent<SpriteRenderer>();
        }

        public void Update()
        {
            ShowItem(entity.HandItem);
        }

        private void ShowItem(Item showItem)
        {
            if (showItem == null)
            {
                if (showingType != ItemType.Null)
                {
                    renderer.color = Color.clear;
                    showingType = ItemType.Null;
                }
            }
            else if (showingType != showItem.Type)
            {
                var itemRenderer = showItem.gameObject.GetComponent<SpriteRenderer>();

                renderer.color = Color.white;
                renderer.sprite = itemRenderer.sprite;
                showingType = showItem.Type;
            }
        }
    }
}
