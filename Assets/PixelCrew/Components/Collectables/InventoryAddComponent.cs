using PixelCrew.Creatures.Hero;
using PixelCrew.Model.Definitions;
using UnityEngine;

namespace PixelCrew.Components
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId, SerializeField] private string _id;
        [SerializeField] private int _count;

        public void Add()
        {
            var hero = FindObjectOfType<Hero>();
            hero?.AddInInventory(_id, _count);
        }
    }
}