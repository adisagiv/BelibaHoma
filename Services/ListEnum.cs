using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Helper
{
    public class ListEnum<T> : IEnumerator
    {
        public List<T> Data;

        // Enumerators are positioned before the first element 
        // until the first MoveNext() call. 
        int _position = -1;

        public ListEnum(IEnumerable<T> data)
        {
            this.Data = new List<T>(data);
        }

        public bool MoveNext()
        {
            _position++;
            return (_position < Data.Count);
        }

        public void Reset()
        {
            _position = -1;
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public T Current
        {
            get
            {
                try
                {
                    return Data[_position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
