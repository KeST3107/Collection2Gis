namespace Collection2Gis.Dictionary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public sealed class ComplexKeyDictionary<TId, TName, TValue> : IEnumerable<TValue>
    {
        private readonly object locker = new object();

        private readonly Dictionary<ComplexKey<TId, TName>, TValue> mainDictionary =
            new Dictionary<ComplexKey<TId, TName>, TValue>();

        private readonly Dictionary<TId, List<TValue>> idDictionary = new Dictionary<TId, List<TValue>>();

        private readonly Dictionary<TName, List<TValue>> nameDictionary = new Dictionary<TName, List<TValue>>();

        /// <summary>
        /// Получение количества значений в коллекции
        /// </summary>
        /// <returns>Количество элементов коллекции</returns>
        public int Count() => mainDictionary.Count;

        public TValue this[TId id, TName name]
        {
            get => GetValue(new ComplexKey<TId, TName>(id, name));

            set => SetValue(new ComplexKey<TId, TName>(id, name), value);
        }

        /// <summary>
        /// Добавление значения по полному ключу
        /// </summary>
        /// <param name="value">Значение с ключем</param>
        public void Add(KeyValuePair<ComplexKey<TId, TName>, TValue> value)
        {
            AddItem(value.Key, value.Value);
        }

        /// <summary>
        /// Добавление значения по полному ключу
        /// </summary>
        /// <param name="complexKey">Ключ</param>
        /// <param name="value">Значение</param>
        public void Add(ComplexKey<TId, TName> complexKey, TValue value)
        {
            AddItem(complexKey, value);
        }

        /// <summary>
        /// Добавление значения по составному ключу и значению
        /// </summary>
        /// <param name="id">ID ключа</param>
        /// <param name="name">NAME ключа</param>
        /// <param name="value">Значение</param>
        public void Add(TId id, TName name, TValue value)
        {
            Add(new ComplexKey<TId, TName>(id, name), value);
        }

        /// <summary>
        /// Очистка коллекции
        /// </summary>
        public void ClearAll()
        {
            lock (locker)
            {
                mainDictionary.Clear();
                idDictionary.Clear();
                nameDictionary.Clear();
            }
        }

        /// <summary>
        /// Удаление значения по полному ключу
        /// </summary>
        /// <param name="value">Значение с ключем</param>
        public void Remove(KeyValuePair<ComplexKey<TId, TName>, TValue> value)
        {
            RemoveItem(value.Key);
        }

        /// <summary>
        /// Удаление значения по полному ключу
        /// </summary>
        /// <param name="complexKey">Ключ</param>
        public void Remove(ComplexKey<TId, TName> complexKey)
        {
            RemoveItem(complexKey);
        }

        /// <summary>
        /// Удаление значения по полному ключу
        /// </summary>
        /// <param name="complexKey">Ключ</param>
        public void Remove(TId id, TName name)
        {
            var complexKey = new ComplexKey<TId, TName>(id, name);
            RemoveItem(complexKey);
        }

        /// <summary>
        /// Удаление всех значений по Id ключа
        /// </summary>
        /// <param name="idKey">Id ключа</param>
        public void RemoveAll(TId idKey)
        {
            RemoveItems(idKey);
        }

        /// <summary>
        /// Удаление всех значений по Name ключа
        /// </summary>
        /// <param name="nameKey">Name ключа</param>
        public void RemoveAll(TName nameKey)
        {
            RemoveItems(nameKey);
        }

        /// <summary>
        /// Получение всех значений по Id ключа
        /// </summary>
        /// <param name="idKey">Id ключа</param>
        /// <returns>Коллекция List</returns>
        /// <exception cref="Exception"></exception>
        public List<TValue> GetItemsById(TId idKey)
        {
            if (idKey != null)
            {
                if (idDictionary.ContainsKey(idKey))
                {
                    return idDictionary[idKey];
                }

                throw new Exception($"Данного ключа нет в коллекции {nameof(idDictionary)}");
            }

            throw new ArgumentNullException(nameof(idKey));
        }

        /// <summary>
        /// Получение всех значений по Name ключа
        /// </summary>
        /// <param name="nameKey">Name ключа</param>
        /// <returns>Коллекция List</returns>
        /// <exception cref="Exception"></exception>
        public List<TValue> GetItemsByName(TName nameKey)
        {
            if (nameKey != null)
            {
                if (nameDictionary.ContainsKey(nameKey))
                {
                    return nameDictionary[nameKey];
                }

                throw new Exception($"Данного ключа нет в коллекции {nameof(nameDictionary)}");
            }

            throw new ArgumentNullException(nameof(nameKey));
        }

        /// <summary>
        /// Получение значения по Id и Name ключа
        /// </summary>
        /// <param name="idKey">Id ключа</param>
        /// <param name="nameKey">Name ключа</param>
        /// <returns>Значение из коллекции</returns>
        /// <exception cref="Exception"></exception>
        public TValue GetValueByKey(TId idKey, TName nameKey)
        {
            if (idKey != null && nameKey != null)
            {
                return mainDictionary
                    .FirstOrDefault(item => item.Key.Id.Equals(idKey)
                                            && item.Key.Name.Equals(nameKey)).Value;
            }

            throw new Exception("Указанный ключ пустой!");
        }

        /// <summary>
        /// Получение значения по полному ключу
        /// </summary>
        /// <param name="complexKey">Полный ключ</param>
        /// <returns>Значение из коллекции</returns>
        /// <exception cref="Exception"></exception>
        public TValue GetValueByKey(ComplexKey<TId, TName> complexKey)
        {
            if (ComplexKeyIsNull(complexKey) == false)
            {
                return mainDictionary
                    .FirstOrDefault(item => item.Key.Id.Equals(complexKey.Id)
                                            && item.Key.Name.Equals(complexKey.Name)).Value;
            }

            throw new Exception("Указанный ключ пустой!");
        }

        private bool ComplexKeyIsNull(ComplexKey<TId, TName> complexKey)
        {
            if (complexKey == null)
            {
                throw new ArgumentNullException(nameof(complexKey));
            }

            if (complexKey.Id == null)
            {
                throw new ArgumentNullException(nameof(complexKey.Id));
            }

            if (complexKey.Name == null)
            {
                throw new ArgumentNullException(nameof(complexKey.Name));
            }

            return false;
        }

        private void RemoveItems(TName nameKey)
        {
            if (nameKey == null)
            {
                throw new ArgumentNullException(nameof(nameKey));
            }

            lock (locker)
            {
                var complexKeys = mainDictionary
                    .Where(x => x.Key.Name.Equals(nameKey))
                    .Select(x => x.Key)
                    .ToArray();

                foreach (var key in complexKeys)
                {
                    mainDictionary.Remove(key);
                }

                RemoveItemPartionalDictionary(nameDictionary, nameKey);
            }
        }

        private void RemoveItems(TId idKey)
        {
            if (idKey == null)
            {
                throw new ArgumentNullException(nameof(idKey));
            }

            lock (locker)
            {
                var complexKeys = mainDictionary
                    .Where(x => x.Key.Id.Equals(idKey))
                    .Select(x => x.Key)
                    .ToArray();

                foreach (var key in complexKeys)
                {
                    mainDictionary.Remove(key);
                }

                RemoveItemPartionalDictionary(idDictionary, idKey);
            }
        }

        private void RemoveItem(ComplexKey<TId, TName> complexKey)
        {
            if (ComplexKeyIsNull(complexKey) == false)
            {
                if (mainDictionary.ContainsKey(complexKey))
                {
                    lock (locker)
                    {
                        mainDictionary.Remove(complexKey);
                        RemoveItemPartionalDictionary(idDictionary, complexKey.Id);
                        RemoveItemPartionalDictionary(nameDictionary, complexKey.Name);
                    }
                }
                else
                {
                    throw new Exception("Запись с данным ключем не найдена!");
                }
            }
        }

        private void RemoveItemPartionalDictionary<T>(Dictionary<T, List<TValue>> dictionary, T key)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
            }
        }

        private void AddItem(ComplexKey<TId, TName> complexKey, TValue value)
        {
            if (ComplexKeyIsNull(complexKey) == false)
            {
                if (mainDictionary.ContainsKey(complexKey))
                {
                    throw new Exception("Запись с данным ключем уже имеется!");
                }

                lock (locker)
                {
                    mainDictionary.Add(complexKey, value);
                    AddItemPartionalDictionary(idDictionary, complexKey.Id, value);
                    AddItemPartionalDictionary(nameDictionary, complexKey.Name, value);
                }

            }
        }

        private void SetValue(ComplexKey<TId, TName> complexKey, TValue value)
        {
            if (ComplexKeyIsNull(complexKey) == false)
            {
                if (mainDictionary.ContainsKey(complexKey))
                {
                    lock (locker)
                    {
                        mainDictionary[complexKey] = value;
                        AddItemPartionalDictionary(idDictionary, complexKey.Id, value);
                        AddItemPartionalDictionary(nameDictionary, complexKey.Name, value);
                    }
                }
                else
                {
                    AddItem(complexKey, value);
                }
            }
        }

        /// <summary>
        /// Дженерик метод для добавления значения по ID или NAME ключу
        /// </summary>
        /// <param name="dictionary">Коллекция в которую нужно занести значение</param>
        /// <param name="key">ID или NAME</param>
        /// <param name="value">Значение</param>
        /// <typeparam name="T">Уазываемый тип TId или TName</typeparam>
        private void AddItemPartionalDictionary<T>(Dictionary<T, List<TValue>> dictionary, T key, TValue value)
        {
            if (dictionary.TryGetValue(key, out var idValue))
            {
                idValue.Add(value);
            }

            dictionary.Add(key, new List<TValue> {value});
        }

        private TValue GetValue(ComplexKey<TId, TName> complexKey)
        {
            if (ComplexKeyIsNull(complexKey) == false)
            {
                if (mainDictionary.ContainsKey(complexKey))
                {
                    return mainDictionary[complexKey];
                }
                else
                {
                    throw new Exception("Запись с данным ключем не найдена!");
                }
            }

            throw new Exception("Указанный ключ пустой!");
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return mainDictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var e = (mainDictionary as IEnumerable).GetEnumerator();
            return e;
        }
    }
}
