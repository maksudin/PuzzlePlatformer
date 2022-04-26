using System.Collections;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Repository
{
    public class DefRepository<TDefType> : ScriptableObject where TDefType : IHaveId
    {
        [SerializeField] protected TDefType[] Collection;

        public TDefType Get(string id)
        {
            foreach (var itemDef in Collection)
            {
                if (itemDef.Id == id)
                    return itemDef;
            }
            return default;
        }
    }
}