using UnityEngine;

namespace PixelCrew.Utils
{
    public static class GameObjectExtensions
    {

        public static bool IsInLayer(this GameObject go, LayerMask layer)
        {
            // По сути в layers используется битовая маска, по которой
            // можно определить что с чем коллайдится.

            // (GameObject.layer возвращает целое число
            // Например 3 значит после сдвига 1 << 3 == 1000
            // И мы прибавляем результат к нашему layer с помощью операнда "|")
            return layer == (layer | 1 << go.layer);
        }

        public static TInterfaceType GetInterface<TInterfaceType>(this GameObject go)
        {
            var components = go.GetComponents<Component>();
            foreach (var component in components)
                if (component is TInterfaceType type)
                    return type;

            return default;
        }

    }
}


