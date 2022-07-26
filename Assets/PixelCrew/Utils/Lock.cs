using System.Collections.Generic;

namespace Assets.PixelCrew.Utils
{
    public class Lock
    {
        private readonly List<object> _retained = new List<object>();

        public void Retain(object item)
        {
            _retained.Add(item);
        }

        public void Release(object item)
        {
            _retained.Remove(item);
        }

        public bool isLocked => _retained.Count > 0;
    }
}