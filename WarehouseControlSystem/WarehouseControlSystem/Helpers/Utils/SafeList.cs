// ----------------------------------------------------------------------------------
// Copyright © 2018, Oleg Lobakov, Contacts: <oleg.lobakov@gmail.com>
// Licensed under the GNU GENERAL PUBLIC LICENSE, Version 3.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// https://github.com/OlegLobakov/WarehouseControlSystem/blob/master/LICENSE
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace WarehouseControlSystem.Helpers.Utils
{
    /// <summary>
    /// Потокобезопасная коллекция
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafeList<T>
    {
        private List<T> _list = new List<T>();
        private object _sync = new object();

        public int Count
        {
            get
            {
                lock (_sync)
                {
                    return _list.Count();
                }
            }
        }

        public List<T> GetList()
        {
            lock (_sync)
            {
                List<T> rv = new List<T>();
                foreach (T item in _list)
                {
                    rv.Add(item);
                }
                return rv;
            }
        }

        public ObservableCollection<T> GetObCollection()
        {
            lock (_sync)
            {
                ObservableCollection<T> rv = new ObservableCollection<T>();
                foreach (T item in _list)
                {
                    rv.Add(item);
                }
                return rv;
            }
        }

        public void SetList(List<T> addlist)
        {
            if (addlist is List<T>)
            {
                lock (_sync)
                {
                    _list.Clear();
                    foreach (T item in addlist)
                    {
                        _list.Add(item);
                    }
                }
            }
        }

        public void Add(T value)
        {
            lock (_sync)
            {
                _list.Add(value);
            }
        }

        
        public void Remove(T value)
        {
            lock (_sync)
            {
                if (_list.Contains(value))
                {
                    _list.Remove(value);
                }
            }
        }

        public T Find(Predicate<T> predicate)
        {
            lock (_sync)
            {
                return _list.Find(predicate);
            }
        }

        public List<T> FindAll(Predicate<T> predicate)
        {
            lock (_sync)
            {
                return _list.FindAll(predicate);
            }
        }

        public T FirstOrDefault()
        {
            lock (_sync)
            {
                return _list.FirstOrDefault();
            }
        }

        public void Clear()
        {
            lock (_sync)
            {
                _list.Clear();
            }
        }
    }
}
