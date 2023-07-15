using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/ItemsSetProvider", fileName = "ItemsSetProvider")]
public class ItemsSetProvider : ScriptableObject
{
    [SerializeField] 
        private Item[] _itemPrefabs;
    
        public Item[] GetItemsSet()
        {
            return _itemPrefabs;
        }
}
