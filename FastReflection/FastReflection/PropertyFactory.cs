using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Linq.Expressions;

namespace FastReflection
{
    internal static class PropertyFactory
    {
        const char SEPARATOR = '.';
        static readonly BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        static readonly Dictionary<CacheKey, IFastProperty> Cache = new Dictionary<CacheKey, IFastProperty>();
        static readonly object locker = new object();

        internal static IFastProperty Create(Type type, string path)
        {
            if (type == null)
                throw new ArgumentNullException("type", "The type was not specified.");

            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path was not specified.","path");

            var key = new CacheKey(type, path);
            lock (locker)
            {
                IFastProperty property;
                if (!Cache.TryGetValue(key, out property))
                {
                    // create property
                    property = Make(type, path);
                    Cache[key] = property;
                }

                return property;
            }
        }

        private static IFastProperty Make(Type type, string path)
        {
            var segments = ParsePath(type, path);
            IFastGetter getter = CreateGetter(type, segments);
            IFastSetter setter = CreateSetter(type, segments);

            return new FastProperty(getter, setter);
        }

        private static IFastGetter CreateGetter(Type origin, PropertyInfo[] segments)
        {
            if (!segments[segments.Length - 1].CanRead)
                return null;

            var instance = Expression.Parameter(typeof(object), "instance");

            // get base property expression
            var target = segments[segments.Length - 1];
            var expression = GetBaseAccessExpression(origin, instance, segments);
            // add last property access
            expression = Expression.Property(expression, target);
            // convert return to object which is return type of method
            expression = Expression.Convert(expression, typeof(object));

            return new FastGetter(origin, Expression.Lambda<Func<object, object>>(expression, instance).Compile());
        }

        private static IFastSetter CreateSetter(Type origin, PropertyInfo[] segments)
        {
            var target = segments[segments.Length - 1];
            if (!target.CanWrite)
                return null;

            var instance = Expression.Parameter(typeof(object), "instance");
            var value = Expression.Parameter(typeof(object), "value");

            var expression = GetBaseAccessExpression(origin, instance, segments);
            // convert object parameter to type of the property
            var cast = Expression.Convert(value, target.PropertyType);
            // call set method on final property
            expression = Expression.Call(expression, target.GetSetMethod(), cast);

            return new FastSetter(origin, target.PropertyType, Expression.Lambda<Action<object,object>>(expression, instance, value).Compile());
        }

        private static Expression GetBaseAccessExpression(Type origin, Expression instance, PropertyInfo[] segments)
        {
            // start by converting the instance parameter to the type
            Expression expression = Expression.Convert(instance, origin);
            int end = segments.Length - 1;
            for (int i = 0; i < end; i++)
            {
                // loop through properties and build up accessing
                expression = Expression.Property(expression, segments[i]);
            }

            return expression;
        }

        private static PropertyInfo[] ParsePath(Type origin, string path)
        {
            var segments = path.Split(SEPARATOR);
            var results = new List<PropertyInfo>(segments.Length);
            var current = origin;

            for (int i = 0; i < segments.Length; i++)
            {
                var property = current.GetProperty(segments[i], BINDING_FLAGS);
                results.Add(property);
                current = property.PropertyType;
            }

            return results.ToArray();
        }

        struct CacheKey : IEquatable<CacheKey>
        {
            private Type Type;
            private string Path;
            private int Hash;

            public CacheKey(Type type, string path)
            {
                Type = type;
                Path = path;
                Hash = ((Type.GetHashCode() * 17) ^ Path.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                if (!(obj is CacheKey))
                    return false;

                return Equals((CacheKey)obj);
            }

            public bool Equals(CacheKey other)
            {
                return Type.Equals(other.Type) && Path.Equals(other.Path);
            }

            public override int GetHashCode()
            {
                return Hash;
            }

            public static bool operator ==(CacheKey a, CacheKey b)
            {
                return a.Type.Equals(b.Type) && a.Path.Equals(b.Path);
            }

            public static bool operator !=(CacheKey a, CacheKey b)
            {
                return !(a == b);
            }
        }
    }
}
