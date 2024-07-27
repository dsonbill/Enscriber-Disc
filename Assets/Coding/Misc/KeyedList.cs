using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Misc
{
    public static partial class Extensions
    {
        public static KeyedList<T> Concat<T>(this IKeyedList<T> kList1, IKeyedList<T> kList2)
        {
            kList1.Compress();
            kList1.Compress();

            KeyedList<T> output = new KeyedList<T>();

            for (int i = 0; i < kList1.Count; i++)
            {
                output.Container.Add(i, kList1.Container[i]);
            }

            int offset = kList1.Count;
            for (int i = offset; (i - offset) < kList1.Count; i++)
            {
                output.Add(i, kList1[i - offset]);
            }

            return output;
        }
    }

    public interface IKeyedList<T> : IEnumerable<T>
    {
        T this[int i] { get; set; }

        Dictionary<int, T> Container { get; set; }

        int HighestCount { get; set; }
        int EnumeratorIndex { get; set; }
        int EnumeratorPosition { get; set; }
        int Count { get; }

        bool ContainsKey(int key);
        bool ContainsValue(T value);

        int Add(T value);
        void Add(int index, T value);
        void Remove(int index);
        void Write(int index, T value);

        void Compress();
        IKeyedList<T> Subset(int startIndex, int length, bool preserveIndex = false);
        IKeyedList<T> Subset(int startIndex, int length, int count, bool preserveIndex = false);
        void Overlay(IKeyedList<T> keyedList, KeyedList<T>.OverlayMode mode);
        void OverlayFromDictionary<T2>(Dictionary<T2, T> dictionary, KeyedList<T>.OverlayMode mode);
        void CopyFromDictionary<T2>(Dictionary<T2, T> dictionary, bool ignoreNonintKeys = false);
        Dictionary<string, T> CopyToStringDictionary();
    }

    public class KeyedListEnumerator<T> : IEnumerator<T>
    {
        public Func<Dictionary<int, T>> GetFunc { get; set; }
        public Func<int> GetIndex { get; set; }
        public Func<int> GetPosition { get; set; }
        public Action<int> SetIndex { get; set; }
        public Action<int> SetPosition { get; set; }
        public Func<int> GetHighestCount { get; set; }
        public Action<int> SetHighestCount { get; set; }
        public Func<int, bool> ContainsKey { get; set; }

        public T Current
        {
            get
            {
                return GetFunc()[GetIndex()];
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return GetFunc()[GetIndex()];
            }
        }

        public bool MoveNext()
        {
            SetPosition(GetPosition() + 1);
            if (GetPosition() >= GetFunc().Count)
            {
                return false;
            }

            for (int i = GetIndex() + 1; i < GetHighestCount(); i++)
            {
                if (ContainsKey(i))
                {
                    SetIndex(i);
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            SetIndex(-1);
            SetPosition(-1);
        }

        public void Dispose()
        {
            Reset();
        }
    }

    [DataContract]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class KeyedList<T> : IKeyedList<T>
    {
        public enum OverlayMode
        {
            Overwrite,
            Skip,
            Warn,
            Exception
        }

        [DataMember(Name = "KeyedList")]
        protected Dictionary<string, T> SerializationContainer = new Dictionary<string, T>();

        [OnSerializing]
        void SetValuesOnSerializing(StreamingContext context)
        {
            SerializationContainer = CopyToStringDictionary();
        }

        [OnDeserialized]
        void SetValuesOnDeserialized(StreamingContext context)
        {
            CopyFromDictionary(SerializationContainer);
        }

        public Dictionary<int, T> Container { get; set; }
        public int HighestCount { get; set; }
        public int EnumeratorIndex { get; set; }
        public int EnumeratorPosition { get; set; }

        public T this[int i]
        {
            get { return Container[i]; }
            set { Write(i, value); }
        }

        public KeyedList()
        {
            Container = new Dictionary<int, T>();
            EnumeratorIndex = -1;
            EnumeratorPosition = -1;
        }

        public int Count
        {
            get
            {
                return Container.Count;
            }
        }

        public bool ContainsKey(int key)
        {
            return Container.ContainsKey(key);
        }

        public bool ContainsValue(T value)
        {
            return Container.ContainsValue(value);
        }

        public int Add(T value)
        {
            if (Container.Count < HighestCount)
            {
                Container.Add(HighestCount, value);
            }
            else
            {
                int index = Container.Count;
                Container.Add(Container.Count, value);
                return index;
                
            }
            if (HighestCount < Container.Count)
            {
                int index = HighestCount;
                HighestCount = Container.Count;
                return index;
            }
            return 0;
        }

        public void Add(int index, T value)
        {
            if (Container.ContainsKey(index))
            {
                throw new ArgumentOutOfRangeException("KeyedList Already Contains An Object At Specified Key: " + index);
            }

            if (HighestCount < index + 1)
            {
                HighestCount = index + 1;
            }
            Container.Add(index, value);
        }

        public void Remove(int index)
        {
            if (!Container.ContainsKey(index))
            {
                throw new ArgumentOutOfRangeException("KeyedList Does Not Contain An Object At Specified Key: " + index);
            }
            Container.Remove(index);
        }

        public void Write(int index, T value)
        {
            if (HighestCount < index + 1)
            {
                HighestCount = index + 1;
            }
            Container[index] = value;
        }

        public void Compress()
        {
            Dictionary<int, T> tempDictionary = new Dictionary<int, T>();
            for (int i = 0; i < HighestCount; i++)
            {
                if (Container.ContainsKey(i))
                {
                    tempDictionary.Add(tempDictionary.Count, Container[i]);
                }
            }
            Container = tempDictionary;
            HighestCount = Container.Count;
        }

        public IKeyedList<T> Subset(int startIndex, int length, bool preserveIndex = false)
        {
            return Subset(startIndex, length, Count, preserveIndex);
        }

        public IKeyedList<T> Subset(int startIndex, int length, int count, bool preserveIndex = false)
        {
            KeyedList<T> output = new KeyedList<T>();

            int currentCount = 0;
            for (int i = startIndex; i < length; i++)
            {
                if (Container.ContainsKey(i))
                {
                    if (preserveIndex)
                    {
                        output.Add(i, Container[i]);
                    }
                    else
                    {
                        output.Add(Container[i]);
                    }

                    currentCount++;
                    if (currentCount == count)
                    {
                        return output;
                    }
                }
            }

            return output;
        }

        public void Overlay(IKeyedList<T> keyedList, KeyedList<T>.OverlayMode mode)
        {
            if (mode == KeyedList<T>.OverlayMode.Overwrite)
            {
                foreach (T value in keyedList)
                {
                    Write(keyedList.EnumeratorIndex, value);
                }
            }

            if (mode == KeyedList<T>.OverlayMode.Skip)
            {
                foreach (T value in keyedList)
                {
                    if (ContainsKey(keyedList.EnumeratorIndex))
                    {
                        continue;
                    }
                    Add(keyedList.EnumeratorIndex, value);
                }
            }

            if (mode == KeyedList<T>.OverlayMode.Warn)
            {
                foreach (T value in keyedList)
                {
                    if (ContainsKey(keyedList.EnumeratorIndex))
                    {
                        Console.WriteLine("WARNING: KeyedList Overlay is Overwriting Values at Key: " + keyedList.EnumeratorIndex);
                    }
                    Write(keyedList.EnumeratorIndex, value);
                }
            }

            if (mode == KeyedList<T>.OverlayMode.Exception)
            {
                foreach (T value in keyedList)
                {
                    if (ContainsKey(keyedList.EnumeratorIndex))
                    {
                        throw new Exception("KeyedList Overlay Tried To Perform Illegal Overwrite at Key: " + keyedList.EnumeratorIndex);
                    }
                    Add(keyedList.EnumeratorIndex, value);
                }
            }
        }

        public void OverlayFromDictionary<T2>(Dictionary<T2, T> dictionary, KeyedList<T>.OverlayMode mode)
        {
            T2[] keys = new T2[dictionary.Count];
            dictionary.Keys.CopyTo(keys, 0);

            if (mode == KeyedList<T>.OverlayMode.Overwrite)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    int index = Convert.ToInt32(keys[i]);
                    Write(index, dictionary[keys[i]]);
                }
            }

            if (mode == KeyedList<T>.OverlayMode.Skip)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    int index = Convert.ToInt32(keys[i]);
                    if (ContainsKey(index))
                    {
                        continue;
                    }
                    Add(index, dictionary[keys[i]]);
                }
            }

            if (mode == KeyedList<T>.OverlayMode.Warn)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    int index = Convert.ToInt32(keys[i]);
                    if (ContainsKey(index))
                    {
                        Console.WriteLine("WARNING: KeyedList Overlay is Overwriting Values at Key: " + index);
                    }
                    Write(index, dictionary[keys[i]]);
                }
            }

            if (mode == KeyedList<T>.OverlayMode.Exception)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    int index = Convert.ToInt32(keys[i]);
                    if (ContainsKey(index))
                    {
                        throw new Exception("KeyedList Overlay Tried To Perform Illegal Overwrite at Key: " + index);
                    }
                    Write(index, dictionary[keys[i]]);
                }
            }
        }

        public void CopyFromDictionary<T2>(Dictionary<T2, T> dictionary, bool ignoreNonintKeys = false)
        {
            this.Container = new Dictionary<int, T>();

            T2[] keys = new T2[dictionary.Count];
            dictionary.Keys.CopyTo(keys, 0);
            for (int i = 0; i < keys.Length; i++)
            {
                if (!ignoreNonintKeys)
                {
                    int index = Convert.ToInt32(keys[i]);
                    Add(index, dictionary[keys[i]]);
                }
                else
                {
                    try
                    {
                        int index = Convert.ToInt32(keys[i]);
                        Add(index, dictionary[keys[i]]);
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
        }

        public Dictionary<string, T> CopyToStringDictionary()
        {
            Dictionary<string, T> outputDictionary = new Dictionary<string, T>();

            for (int i = 0; i < HighestCount; i++)
            {
                if (Container.ContainsKey(i))
                {
                    outputDictionary.Add(i.ToString(), Container[i]);
                }
            }

            return outputDictionary;
        }

        public IEnumerator<T> GetEnumerator()
        {
            KeyedListEnumerator<T> enumerator = new KeyedListEnumerator<T>();
            enumerator.GetFunc = () => { return Container; };
            enumerator.GetIndex = () => { return EnumeratorIndex; };
            enumerator.SetIndex = (int index) => { EnumeratorIndex = index; };
            enumerator.GetPosition = () => { return EnumeratorPosition; };
            enumerator.SetPosition = (int position) => { EnumeratorPosition = position; };
            enumerator.GetHighestCount = () => { return HighestCount; };
            enumerator.SetHighestCount = (int highestCount) => { HighestCount = highestCount; };
            enumerator.ContainsKey = (int index) => { return ContainsKey(index); };
            return enumerator as IEnumerator<T>;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            KeyedListEnumerator<T> enumerator = new KeyedListEnumerator<T>();
            enumerator.GetFunc = () => { return Container; };
            enumerator.GetIndex = () => { return EnumeratorIndex; };
            enumerator.SetIndex = (int index) => { EnumeratorIndex = index; };
            enumerator.GetPosition = () => { return EnumeratorPosition; };
            enumerator.SetPosition = (int position) => { EnumeratorPosition = position; };
            enumerator.GetHighestCount = () => { return HighestCount; };
            enumerator.SetHighestCount = (int highestCount) => { HighestCount = highestCount; };
            enumerator.ContainsKey = (int index) => { return ContainsKey(index); };
            return enumerator as IEnumerator<T>;
        }
    }
}
