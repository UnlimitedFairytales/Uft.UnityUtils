#nullable enable

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Uft.UnityUtils.Common
{
    public static class Reflec
    {
        static readonly BindingFlags Default = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        static object GetMemberValue(object o, string memberName, bool isProperty)
        {
            var type = o.GetType();
            Func<MemberInfo?> lambda1;
            Func<MemberInfo, object> lambda2;
            if (isProperty)
            {
                lambda1 = () => type.GetProperty(memberName, Default | BindingFlags.GetProperty);
                lambda2 = (i) => ((PropertyInfo)i).GetValue(o, null);
            }
            else
            {
                lambda1 = () => type.GetField(memberName, Default | BindingFlags.GetField);
                lambda2 = (i) => ((FieldInfo)i).GetValue(o);
            }
            var info = lambda1();
            while (info == null && type.BaseType != null)
            {
                type = type.BaseType;
                info = lambda1();
            }
            if (info == null) throw new ArgumentException(memberName, nameof(memberName));
            return lambda2(info);
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static object GetProperty(object o, string memberName)
        {
            return GetMemberValue(o, memberName, true);
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="e"></param>
        /// <param name="value"></param>
        public static void SetProperty<T>(object o, Expression<Func<T>> e, T value)
        {
            var memberName = ((MemberExpression)e.Body).Member.Name;
            var type = o.GetType();
            PropertyInfo? info = type.GetProperty(memberName, Default | BindingFlags.GetProperty);
            while ((info == null || !info.CanWrite) && type.BaseType != null)
            {
                type = type.BaseType;
                info = type.GetProperty(memberName, Default | BindingFlags.GetProperty);
            }
            if (info == null) throw new ArgumentException(memberName, nameof(memberName)); // NOTE: 引数とメッセージにズレはあるが、通じるのでヨシ
            info.SetValue(o, value, null);
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="memberName"></param>
        /// <returns></returns>
        public static object GetField(object o, string memberName)
        {
            return GetMemberValue(o, memberName, false);
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="memberName"></param>
        /// <param name="value"></param>
        public static void SetField(object o, string memberName, object value)
        {
            FieldInfo? info = o.GetType().GetField(memberName, Default | BindingFlags.GetField )
                ?? throw new ArgumentException(memberName, nameof(memberName));
            info.SetValue(o, value);
        }

        public static string ToFieldName(string name)
        {
            var post = 1 < name.Length ? name[1..] : "";
            return "_" + name[0].ToString().ToLowerInvariant() + post;
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <param name="e"></param>
        /// <param name="value"></param>
        public static void SetFieldForProperty<T>(object o, Expression<Func<T>> e, T value)
        {
            var memberName = ToFieldName(((MemberExpression)e.Body).Member.Name);

            var type = o.GetType();
            FieldInfo? info = type.GetField(memberName, Default | BindingFlags.GetField);
            while (info == null && type.BaseType != null)
            {
                type = type.BaseType;
                info = type.GetField(memberName, Default | BindingFlags.GetField);
            }
            if (info == null) throw new ArgumentException(memberName, nameof(memberName));
            info.SetValue(o, value);
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="memberName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static object InvokePrivateMethod(object o, string memberName, object[] parameters)
        {
            MethodInfo info = o.GetType().GetMethod(memberName, Default | BindingFlags.InvokeMethod)
                ?? throw new ArgumentException(memberName, nameof(memberName));
            return info.Invoke(o, parameters);
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <typeparam name="TSearchType"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static List<TSearchType> GetInstanceFieldByType<TSearchType>(object o)
        {
            var infos = GetInstanceFieldInfosFromType(o, typeof(TSearchType));
            var list = new List<TSearchType>();
            foreach (var f in infos)
            {
                var t = f.FieldType;
                if (t.IsArray || t == typeof(List<TSearchType>))
                {
                    var val = (IEnumerable<TSearchType>)f.GetValue(o);
                    if (val != null)
                    {
                        list.AddRange(val);
                    }
                }
                else
                {
                    list.Add((TSearchType)f.GetValue(o));
                }
            }
            return list;
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <param name="o"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public static List<FieldInfo> GetInstanceFieldInfosFromType(object o, Type searchType)
        {
            var type = o.GetType();
            List<FieldInfo> fieldInfos = new();
            while (type != null)
            {
                var fs = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                if (fs != null)
                {
                    fieldInfos.AddRange(fs);
                }
                type = type.BaseType;
            }
            for (int i = fieldInfos.Count - 1; 0 <= i; i--)
            {
                var elt = fieldInfos[i].FieldType.GetElementType();
                var gat = fieldInfos[i].FieldType.GetGenericArguments();
                if (fieldInfos[i].FieldType != searchType &&
                    elt != searchType &&
                    !(fieldInfos[i].FieldType.IsGenericType && fieldInfos[i].FieldType.GetGenericTypeDefinition() == typeof(List<>) && gat[0] == searchType))
                {
                    fieldInfos.RemoveAt(i);
                }
            }
            return fieldInfos;
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type? GetTypeFromName(String name)
        {
            List<Type> types = GetAllTypes();
            foreach (Type type in types)
            {
                if (type.FullName == name || type.AssemblyQualifiedName == name || type.Name == name)
                    return type;
            }
            return null;
        }

        /// <summary>
        /// This function is very heavy! (about 100 or 1000 times for simple assignment expression.)
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetAllTypes()
        {
            List<Type> assemblyTypes = new();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                assemblyTypes.AddRange(assembly.GetTypes());
            }
            return assemblyTypes;
        }
    }
}
