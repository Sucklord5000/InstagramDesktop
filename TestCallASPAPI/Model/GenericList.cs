using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestCallASPAPI.Model
{
    public class GenericList<T> : INotifyPropertyChanged, ICollection<T>
    {
        private T[] Generic;
        public void Add(T input)
        {
            Array.Resize(ref Generic, this.Generic.Length + 1);
            this.Generic[this.Generic.Length - 1] = input;
        }
        public T this[int index]
        {
            get
            {
                return Generic[index];
            }
            set
            {
                this.Generic[index] = value;
            }
        }
        public T[] ItemsSource
        {
            get
            {
                return this.Generic;
            }
            set
            {
                this.Generic = value;
                OnPropertyChanged(nameof(ItemsSource));
            }
        }

        public int Count
        {
            get
            {
                return Generic.Length;
            }
        }

        public bool IsReadOnly
        {
            get
            {
               return Generic.IsReadOnly;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        public void Clear()
        {
            this.Generic = null;
        }

        public bool Contains(T item)
        {
           return Generic.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Generic.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            T[] temp = Generic.Where(x => !x.Equals(item)).ToArray();
            Clear();
            Generic = new T[temp.Length];
            Array.Copy(temp, this.Generic, 0);
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var it in this.Generic)
            {
                if (it == null)
                    break;
                else
                    yield return it;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
