using System;
using System.Collections.Generic;
using System.Reflection;

namespace MB.Json.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    internal static class ReflectionHelper
    {
        private readonly static Type _includeBaseAttributeType = typeof(SerializableAttribute);
        private static readonly Type _nonSerializableAttributeType = typeof(NonSerializedAttribute);
        /// <summary>
        /// ��ȡ ����ϵ�л���������Ϣ��
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<PropertyInfo> GetSerializablePropertys(Type type)
        {
            List<PropertyInfo> proList = new List<PropertyInfo>(10);
            proList.AddRange(type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly));
            RemoveNonSerializablePropertys(proList);
            if (type.BaseType != null && type.GetCustomAttributes(_includeBaseAttributeType, false).Length > 0)
            {
                proList.AddRange(GetSerializablePropertys(type.BaseType));
            }
            return proList;
        }
        /// <summary>
        /// Remove ��������ϵ�л�������.
        /// </summary>
        /// <param name="proList"></param>
        private static void RemoveNonSerializablePropertys(List<PropertyInfo> proList)
        {
            for(int i = 0; i < proList.Count; ++i)
            {
                if (!ShouldSerializeProInfo(proList[i]))
                {
                    proList.RemoveAt(i--);
                }
            }
        }
        /// <summary>
        /// �ж�
        /// </summary>
        /// <param name="proInfo"></param>
        /// <returns></returns>
        public static bool ShouldSerializeProInfo(ICustomAttributeProvider proInfo)
        {
            return proInfo.GetCustomAttributes(_nonSerializableAttributeType, true).Length <= 0;
        }
        /// <summary>
        /// �������������Ͳ��Ҷ�Ӧ��������Ϣ��
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyInfo FindProperty(Type type, string name)
        {
            PropertyInfo field = FindPropertyThroughoutHierarchy(type, name);
            if (field == null)
            {
                throw new ArgumentException(type.FullName + " doesn't have a field named: " + name);
            }
            return field;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static PropertyInfo FindPropertyThroughoutHierarchy(Type type, string name)
        {
            PropertyInfo field = type.GetProperty(name,BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            if (field == null && type.GetCustomAttributes(_includeBaseAttributeType, false).Length > 0)
            {
                field = FindPropertyThroughoutHierarchy(type.BaseType, name);
            }
            return field;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="proInfo"></param>
        /// <param name="object"></param>
        /// <returns></returns>
        public static object GetValue(PropertyInfo proInfo, object @object)
        {
            object value = proInfo.GetValue(@object,null);
            return (proInfo.PropertyType.IsEnum) ? (int) value : value;            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ConstructorInfo GetDefaultConstructor(Type type)
        {
            ConstructorInfo constructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[0], null);
            if (constructor == null)
            {
                throw new JsonException(type.FullName + " must have a parameterless constructor");
            }
            return constructor;
        }
    }
}
