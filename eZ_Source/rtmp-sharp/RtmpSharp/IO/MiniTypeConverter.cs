// Decompiled with JetBrains decompiler
// Type: RtmpSharp.IO.MiniTypeConverter
// Assembly: rtmp-sharp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 8588136F-A4B9-4004-9712-4EA13AA4AF9D
// Assembly location: C:\Users\Hesa\Desktop\eZ_Source\bin\Debug\rtmp-sharp.dll

using Complete;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace RtmpSharp.IO
{
    internal static class MiniTypeConverter
    {
        private static readonly MethodInfo EnumerableToArrayMethod = typeof(MiniTypeConverter).GetMethod("EnumerableToArray", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly ConcurrentDictionary<Type, MethodInfo> EnumerableToArrayCache = new ConcurrentDictionary<Type, MethodInfo>();
        private static readonly ConcurrentDictionary<Type, MiniTypeConverter.AdderMethodInfo> AdderMethodCache = new ConcurrentDictionary<Type, MiniTypeConverter.AdderMethodInfo>();

        private static T[] EnumerableToArray<T>(IEnumerable enumerable)
        {
            return enumerable.Cast<T>().ToArray<T>();
        }

        public static object ConvertTo(object value, Type targetType)
        {
            if (value == null)
                return MiniTypeConverter.CreateDefaultValue(targetType);
            Type type1 = value.GetType();
            if (type1 == targetType || targetType.IsInstanceOfType(value))
                return value;
            if (type1.IsConvertible() && targetType.IsConvertible())
            {
                if (!targetType.IsEnum)
                    return MiniTypeConverter.ConvertObject(type1, targetType, value);
                string str = value as string;
                if (str != null)
                    return Enum.Parse(targetType, str, true);
                return Enum.ToObject(targetType, value);
            }
            IEnumerable source1 = value as IEnumerable;
            if (targetType.IsArray && source1 != null)
            {
                Type elementType = type1.GetElementType();
                Type destinationElementType = targetType.GetElementType();
                IEnumerable<object> source2 = source1.Cast<object>();
                if (!destinationElementType.IsAssignableFrom(elementType))
                    source2 = source2.Select<object, object>((Func<object, object>)(x => MiniTypeConverter.ConvertTo(x, destinationElementType)));
                return MiniTypeConverter.EnumerableToArrayCache.GetOrAdd(destinationElementType, (Func<Type, MethodInfo>)(type => MiniTypeConverter.EnumerableToArrayMethod.MakeGenericMethod(type))).Invoke((object)null, new object[1] { (object)source2 });
            }
            IDictionary<string, object> dictionary1 = value as IDictionary<string, object>;
            Type interfaceType1 = MiniTypeConverter.TryGetInterfaceType(targetType, typeof(IDictionary<,>));
            if (dictionary1 != null && interfaceType1 != (Type)null)
            {
                object instance = MethodFactory.CreateInstance(targetType);
                MiniTypeConverter.AdderMethodInfo orAdd = MiniTypeConverter.AdderMethodCache.GetOrAdd(interfaceType1, (Func<Type, MiniTypeConverter.AdderMethodInfo>)(type => new MiniTypeConverter.AdderMethodInfo(type)));
                foreach (KeyValuePair<string, object> keyValuePair in (IEnumerable<KeyValuePair<string, object>>)dictionary1)
                    orAdd.Method.Invoke(instance, new object[2]
                    {
            MiniTypeConverter.ConvertTo((object) keyValuePair.Key, orAdd.TypeGenericParameters[0]),
            MiniTypeConverter.ConvertTo(keyValuePair.Value, orAdd.TypeGenericParameters[1])
                    });
                return instance;
            }
            IDictionary dictionary2 = value as IDictionary;
            if (typeof(IDictionary).IsAssignableFrom(targetType) && dictionary2 != null)
            {
                IDictionary instance = (IDictionary)MethodFactory.CreateInstance(targetType);
                foreach (DictionaryEntry dictionaryEntry in dictionary2)
                    instance.Add(dictionaryEntry.Key, dictionaryEntry.Value);
                return (object)instance;
            }
            Type interfaceType2 = MiniTypeConverter.TryGetInterfaceType(targetType, typeof(IList<>));
            if (interfaceType2 != (Type)null && source1 != null)
            {
                object instance = MethodFactory.CreateInstance(targetType);
                MiniTypeConverter.AdderMethodInfo orAdd = MiniTypeConverter.AdderMethodCache.GetOrAdd(interfaceType2, (Func<Type, MiniTypeConverter.AdderMethodInfo>)(type => new MiniTypeConverter.AdderMethodInfo(type)));
                foreach (object obj in source1)
                    orAdd.Method.Invoke(instance, new object[1]
                    {
            MiniTypeConverter.ConvertTo(obj, orAdd.TypeGenericParameters[0])
                    });
                return instance;
            }
            if (typeof(IList).IsAssignableFrom(targetType) && source1 != null)
            {
                IList instance = (IList)MethodFactory.CreateInstance(targetType);
                foreach (object obj in source1)
                    instance.Add(obj);
                return (object)instance;
            }
            if (targetType == typeof(Guid))
            {
                string input = value as string;
                if (input != null)
                    return (object)Guid.Parse(input);
                byte[] b = value as byte[];
                if (b != null)
                    return (object)new Guid(b);
            }
            if (!targetType.IsNullable())
                return MiniTypeConverter.ConvertObject(type1, targetType, value);
            Type underlyingType = Nullable.GetUnderlyingType(targetType);
            return Convert.ChangeType(value, underlyingType, (IFormatProvider)CultureInfo.InvariantCulture);
        }

        private static object ConvertObject(Type sourceType, Type targetType, object value)
        {
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(sourceType);

                if (converter.CanConvertTo(targetType))
                    return converter.ConvertTo(null, CultureInfo.InvariantCulture, value, targetType);
                return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex.StackTrace);
            }
            return null;
        }

        private static object CreateDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            return (object)null;
        }

        private static Type TryGetInterfaceType(Type targetType, Type type)
        {
            return ((IEnumerable<Type>)targetType.GetInterfaces()).Where<Type>((Func<Type, bool>)(x => x.IsGenericType)).FirstOrDefault<Type>((Func<Type, bool>)(x => typeof(IDictionary<,>) == x.GetGenericTypeDefinition()));
        }

        private struct AdderMethodInfo
        {
            public readonly MethodInfo Method;
            public readonly Type[] TypeGenericParameters;

            public AdderMethodInfo(Type genericType)
            {
                this.Method = genericType.GetMethod("Add");
                this.TypeGenericParameters = genericType.GetGenericArguments();
            }
        }
    }
}