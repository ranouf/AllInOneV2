using System.Reflection;

namespace AllInOne.Common.Testing
{
    public class PrivateObject
    {
        private readonly object _object;

        public PrivateObject(object o)
        {
            _object = o;
        }

        public object Invoke(string methodName, params object[] args)
        {
            var method = _object.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            return method.Invoke(_object, args);
        }

        public void SetProperty(string propertyName, object value)
        {
            var property = _object.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            property.SetValue(_object, value);
        }

        public object GetProperty(string propertyName)
        {
            var property = _object.GetType().GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance);
            return property.GetValue(_object);
        }

        public object GetField(string fieldName)
        {
            var field = _object.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return field.GetValue(_object);
        }
    }
}
