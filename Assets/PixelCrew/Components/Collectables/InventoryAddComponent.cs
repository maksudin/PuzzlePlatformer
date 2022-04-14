using System.Collections;
using PixelCrew.Creatures.Hero;
using PixelCrew.Model.Data;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Components
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId][SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add(GameObject go)
        {
            var hero = go.GetComponent<ICanAddInInventory>();
            hero?.AddInInventory(_id, _count);
        }
    }
}