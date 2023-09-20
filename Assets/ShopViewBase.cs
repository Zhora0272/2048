﻿using System.Collections.Generic;
using UnityEngine;

namespace Shop
{
    public class ShopViewBase : MonoBehaviour
    {
        [SerializeField] protected ShopEyeItem _prefabRectTransform;
        [SerializeField] protected RectTransform _actiavtedContent;
        [SerializeField] protected RectTransform _deactiavtedContent;

        protected List<ShopEyeItem> _shopEyeItems = new();
        protected IManager<ShopCustomizeManager, EyeCustomizeModel> _manager;

        public void SetManager(IManager<ShopCustomizeManager, EyeCustomizeModel> manager) => _manager = manager;
        private void Awake() => Init();
        protected virtual void InitData(int[] saveIndex)
        {
            
        }
        
        protected virtual void Init()
        {
            
        }
    }
}