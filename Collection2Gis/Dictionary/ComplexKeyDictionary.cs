using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Collection2Gis.Dictionary
{
    public sealed class ComplexKeyDictionary<TId, TName, TValue> : IEnumerable<TValue>
    {
        private readonly Dictionary<TId, List<TValue>> _idDictionary = new Dictionary<TId, List<TValue>>();
        private readonly object _locker = new object();

        private readonly Dictionary<ComplexKey<TId, TName>, TValue> _mainDictionary =
            new Dictionary<ComplexKey<TId, TName>, TValue>();

        private readonly Dictionary<TName, List<TValue>> _nameDictionary = new Dictionary<TName, List<TValue>>();

        public TValue this[TId id, TName name]
        {
            get => GetValue(new ComplexKey<TId, TName>(id, name));

            set => SetValue(new ComplexKey<TId, TName>(id, name), value);
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return _mainDictionary.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            var e = (_mainDictionary as IEnumerable).GetEnumerator();
            return e;
        }

        /// <summary>
        ///     Получение количества значений в коллекции.
        /// </summary>
        /// <returns>Количество элементов коллекции.</returns>
        public int Count()
        {
            return _mainDictionary.Count;
        }

        /// <summary>
        ///     Добавление значения по полному ключу.
        /// </summary>
        /// <param name="value">Значение с ключем.</param>
        public void Add(KeyValuePair<ComplexKey<TId, TName>, TValue> value)
        {
            AddItem(value.Key, value.Value);
        }

        /// <summary>
        ///     Добавление значения по полному ключу.
        /// </summary>
        /// <param name="complexKey">Ключ.</param>
        /// <param name="value">Значение.</param>
        public void Add(ComplexKey<TId, TName> complexKey, TValue value)
        {
            AddItem(complexKey, value);
        }

        /// <summary>
        ///     Добавление значения по составному ключу и значению.
        /// </summary>
        /// <param name="id">Идентификатор ключа.</param>
        /// <param name="name">Имя ключа.</param>
        /// <param name="value">Значение.</param>
        public void Add(TId id, TName name, TValue value)
        {
            Add(new ComplexKey<TId, TName>(id, name), value);
        }

        /// <summary>
        ///     Очистка коллекции.
        /// </summary>
        public void ClearAll()
        {
            Console.WriteLine("Поток блокирован.");
            lock (_locker)
            {
                _mainDictionary.Clear();
                _idDictionary.Clear();
                _nameDictionary.Clear();
            }

            Console.WriteLine("Поток разблокирован.");
        }

        /// <summary>
        ///     Удаление значения по полному ключу.
        /// </summary>
        /// <param name="value">Значение с ключем.</param>
        public void Remove(KeyValuePair<ComplexKey<TId, TName>, TValue> value)
        {
            RemoveItem(value.Key);
        }

        /// <summary>
        ///     Удаление значения по полному ключу.
        /// </summary>
        /// <param name="complexKey">Ключ.</param>
        public void Remove(ComplexKey<TId, TName> complexKey)
        {
            RemoveItem(complexKey);
        }

        /// <summary>
        ///     Удаление значения по полному ключу.
        /// </summary>
        /// <param name="complexKey">Ключ.</param>
        public void Remove(TId id, TName name)
        {
            var complexKey = new ComplexKey<TId, TName>(id, name);
            RemoveItem(complexKey);
        }

        /// <summary>
        ///     Удаление всех значений по идентификатору ключа.
        /// </summary>
        /// <param name="idKey">Идентификатор ключа.</param>
        public void RemoveById(TId idKey)
        {
            RemoveItems(idKey);
        }

        /// <summary>
        ///     Удаление всех значений по имени ключа.
        /// </summary>
        /// <param name="nameKey">Имя ключа.</param>
        public void RemoveByName(TName nameKey)
        {
            RemoveItems(nameKey);
        }

        /// <summary>
        ///     Получение всех значений по идентификатору ключа.
        /// </summary>
        /// <param name="idKey">Идентификатор ключа.</param>
        /// <returns>Коллекция значений.</returns>
        /// <exception cref="Exception"></exception>
        public List<TValue> GetValuesById(TId idKey)
        {
            if (idKey != null)
            {
                if (_idDictionary.ContainsKey(idKey)) return _idDictionary[idKey];

                throw new Exception($"Данного ключа нет в коллекции {nameof(_idDictionary)}");
            }

            throw new ArgumentNullException(nameof(idKey));
        }

        /// <summary>
        ///     Получение всех значений по имени ключа.
        /// </summary>
        /// <param name="nameKey">Имя ключа.</param>
        /// <returns>Коллекция значений.</returns>
        /// <exception cref="Exception"></exception>
        public List<TValue> GetValuesByName(TName nameKey)
        {
            if (nameKey != null)
            {
                if (_nameDictionary.ContainsKey(nameKey)) return _nameDictionary[nameKey];

                throw new Exception($"Данного ключа нет в коллекции {nameof(_nameDictionary)}");
            }

            throw new ArgumentNullException(nameof(nameKey));
        }

        /// <summary>
        ///     Получение значения по идентификатору и имени ключа.
        /// </summary>
        /// <param name="idKey">Идентификатор ключа.</param>
        /// <param name="nameKey">Имя ключа.</param>
        /// <returns>Значение из коллекции.</returns>
        /// <exception cref="Exception"></exception>
        public TValue GetValueByKey(TId idKey, TName nameKey)
        {
            if (idKey != null && nameKey != null)
                return _mainDictionary
                    .FirstOrDefault(item => item.Key.Id.Equals(idKey)
                                            && item.Key.Name.Equals(nameKey)).Value;

            throw new Exception("Указанный ключ пустой!");
        }

        /// <summary>
        ///     Получение значения по полному ключу.
        /// </summary>
        /// <param name="complexKey">Полный ключ.</param>
        /// <returns>Значение из коллекции.</returns>
        /// <exception cref="Exception"></exception>
        public TValue GetValueByKey(ComplexKey<TId, TName> complexKey)
        {
            if (ComplexKeyIsNull(complexKey) == false)
                return _mainDictionary
                    .FirstOrDefault(item => item.Key.Id.Equals(complexKey.Id)
                                            && item.Key.Name.Equals(complexKey.Name)).Value;

            throw new Exception("Указанный ключ пустой!");
        }

        private bool ComplexKeyIsNull(ComplexKey<TId, TName> complexKey)
        {
            if (complexKey == null) throw new ArgumentNullException(nameof(complexKey));

            if (complexKey.Id == null) throw new ArgumentNullException(nameof(complexKey.Id));

            if (complexKey.Name == null) throw new ArgumentNullException(nameof(complexKey.Name));

            return false;
        }

        private void RemoveItems(TName nameKey)
        {
            if (nameKey == null) throw new ArgumentNullException(nameof(nameKey));
            Console.WriteLine("Поток блокирован");
            lock (_locker)
            {
                var complexKeys = _mainDictionary
                    .Where(x => x.Key.Name.Equals(nameKey))
                    .Select(x => x.Key)
                    .ToArray();

                foreach (var key in complexKeys) _mainDictionary.Remove(key);

                RemoveItemPartionalDictionary(_nameDictionary, nameKey);
            }

            Console.WriteLine("Поток разблокирован");
        }

        private void RemoveItems(TId idKey)
        {
            if (idKey == null) throw new ArgumentNullException(nameof(idKey));
            Console.WriteLine("Поток блокирован");
            lock (_locker)
            {
                var complexKeys = _mainDictionary
                    .Where(x => x.Key.Id.Equals(idKey))
                    .Select(x => x.Key)
                    .ToArray();

                foreach (var key in complexKeys) _mainDictionary.Remove(key);

                RemoveItemPartionalDictionary(_idDictionary, idKey);
            }

            Console.WriteLine("Поток разблокирован");
        }

        private void RemoveItem(ComplexKey<TId, TName> complexKey)
        {
            if (ComplexKeyIsNull(complexKey) == false)
            {
                if (_mainDictionary.ContainsKey(complexKey))
                {
                    Console.WriteLine("Поток блокирован");
                    lock (_locker)
                    {
                        _mainDictionary.Remove(complexKey);
                        RemoveItemPartionalDictionary(_idDictionary, complexKey.Id);
                        RemoveItemPartionalDictionary(_nameDictionary, complexKey.Name);
                    }

                    Console.WriteLine("Поток разблокирован");
                }
                else
                {
                    throw new Exception("Запись с данным ключем не найдена!");
                }
            }
        }

        private void RemoveItemPartionalDictionary<T>(Dictionary<T, List<TValue>> dictionary, T key)
        {
            if (dictionary.ContainsKey(key)) dictionary.Remove(key);
        }

        private void AddItem(ComplexKey<TId, TName> complexKey, TValue value)
        {
            if (ComplexKeyIsNull(complexKey) == false)
            {
                if (_mainDictionary.ContainsKey(complexKey)) throw new Exception("Запись с данным ключем уже имеется!");
                Console.WriteLine("Поток блокирован");
                lock (_locker)
                {
                    _mainDictionary.Add(complexKey, value);
                    AddItemPartionalDictionary(_idDictionary, complexKey.Id, value);
                    AddItemPartionalDictionary(_nameDictionary, complexKey.Name, value);
                }

                Console.WriteLine("Поток разблокирован");
            }
        }

        private void SetValue(ComplexKey<TId, TName> complexKey, TValue value)
        {
            if (ComplexKeyIsNull(complexKey) == false)
            {
                if (_mainDictionary.ContainsKey(complexKey))
                {
                    Console.WriteLine("Поток блокирован");
                    lock (_locker)
                    {
                        _mainDictionary[complexKey] = value;
                        AddItemPartionalDictionary(_idDictionary, complexKey.Id, value);
                        AddItemPartionalDictionary(_nameDictionary, complexKey.Name, value);
                    }

                    Console.WriteLine("Поток разблокирован");
                }
                else
                {
                    Console.WriteLine("Поток блокирован");
                    lock (_locker)
                    {
                        AddItem(complexKey, value);
                    }

                    Console.WriteLine("Поток разблокирован");
                }
            }
        }

        /// <summary>
        ///     Дженерик метод для добавления значения по идентификатору или имени ключа.
        /// </summary>
        /// <param name="dictionary">Коллекция в которую нужно занести значение.</param>
        /// <param name="key">Идентификатор или имя</param>
        /// <param name="value">Значение.</param>
        /// <typeparam name="T">Указываемый тип TId или TName.</typeparam>
        private void AddItemPartionalDictionary<T>(Dictionary<T, List<TValue>> dictionary, T key, TValue value)
        {
            if (dictionary.TryGetValue(key, out var idValue))
                idValue.Add(value);
            else
                dictionary.Add(key, new List<TValue> { value });
        }

        private TValue GetValue(ComplexKey<TId, TName> complexKey)
        {
            if (ComplexKeyIsNull(complexKey) == false)
            {
                if (_mainDictionary.ContainsKey(complexKey))
                    return _mainDictionary[complexKey];
                throw new Exception("Запись с данным ключем не найдена!");
            }

            throw new Exception("Указанный ключ пустой!");
        }
    }
}
