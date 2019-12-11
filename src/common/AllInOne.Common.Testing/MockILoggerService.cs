using AllInOne.Common.Logging;
using Moq;
using System.Collections;
using System.Collections.Generic;

namespace AllInOne.Common.Testing
{
    public class MockILoggerService<T> : IList<string> where T : class
    {
        public IList<string> Logs { get; set; } = new List<string>();

        public Mock<ILoggerService<T>> Mock { get; set; }

        public ILoggerService<T> Object { get { return Mock.Object; } }

        public int Count => Logs.Count;

        public bool IsReadOnly => Logs.IsReadOnly;

        public string this[int index] { get => Logs[index]; set => Logs[index] = value; }

        public MockILoggerService()
        {
            Mock = new Mock<ILoggerService<T>>();
            Mock
                .Setup(x => x.LogInformation(It.IsAny<string>()))
                .Callback<string>(s => Add(s));
            Mock
                .Setup(x => x.LogError(It.IsAny<string>()))
                .Callback<string>(s => Add(s));
            Mock
                .Setup(x => x.LogWarning(It.IsAny<string>()))
                .Callback<string>(s => Add(s));
            Mock
                .Setup(x => x.LogTrace(It.IsAny<string>()))
                .Callback<string>(s => Add(s));
            Mock
                .Setup(x => x.LogDebug(It.IsAny<string>()))
                .Callback<string>(s => Add(s));
        }

        public int IndexOf(string item)
        {
            return Logs.IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            Logs.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            Logs.RemoveAt(index);
        }

        public void Add(string item)
        {
            Logs.Add(item);
        }

        public void Clear()
        {
            Logs.Clear();
        }

        public bool Contains(string item)
        {
            return Logs.Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            Logs.CopyTo(array, arrayIndex);
        }

        public bool Remove(string item)
        {
            return Logs.Remove(item);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return Logs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Logs.GetEnumerator();
        }
    }
}
