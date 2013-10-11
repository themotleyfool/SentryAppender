using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using SharpRaven.Log4Net.Web;

namespace SharpRaven.Log4Net
{
    public abstract class ExtraAppender
    {
        private static readonly IList<ExtraAppender> appenders;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        static ExtraAppender()
        {
            appenders = new List<ExtraAppender>
            {
                new HttpContextExtraAppender()
            };
        }


        public static object AppendTo(object extra)
        {
            return appenders.Aggregate(extra, (current, appender) => appender.Append(current));
        }


        protected abstract object Append(object extra);


        protected object Append(object extra, string key, string value)
        {
            if (extra == null)
                return null;

            var builder = GetTypeBuilder(extra.GetHashCode());

            CreateProperty(builder, key, typeof (string));

            /*var constructor = builder.DefineDefaultConstructor(
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.RTSpecialName);*/

            var properties = extra.GetType().GetProperties();

            foreach (var property in properties)
            {
                CreateProperty(builder, property.Name, property.PropertyType);
            }

            var t = builder.CreateType();
            var clone = Activator.CreateInstance(t);

            foreach (var property in properties)
            {
                var propertyValue = GetPropertyValue(extra, property.Name, false);
                SetProperty(clone, property.Name, propertyValue, false);
            }

            SetProperty(clone, key, value, false);

            return clone;
        }


        private static TypeBuilder GetTypeBuilder(int randomValue)
        {
            var an = new AssemblyName("DynamicAssembly" + randomValue.ToString());
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");

            var tb = moduleBuilder.DefineType("DynamicType"
                                              ,
                                              TypeAttributes.Public |
                                              TypeAttributes.Class |
                                              TypeAttributes.AutoClass |
                                              TypeAttributes.AnsiClass |
                                              TypeAttributes.BeforeFieldInit |
                                              TypeAttributes.AutoLayout
                                              ,
                                              typeof (object));
            return tb;
        }


        private static void CreateProperty(TypeBuilder builder, string propertyName, Type propertyType)
        {
            var fieldBuilder = builder.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);
            var propertyBuilder = builder.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);

            var getPropertyBuiler = CreatePropertyGetter(builder, fieldBuilder);
            var setPropertyBuiler = CreatePropertySetter(builder, fieldBuilder);

            propertyBuilder.SetGetMethod(getPropertyBuiler);
            propertyBuilder.SetSetMethod(setPropertyBuiler);
        }


        private static MethodBuilder CreatePropertyGetter(TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            var getMethodBuilder = typeBuilder.DefineMethod("get_" + fieldBuilder.Name,
                                                            MethodAttributes.Public |
                                                            MethodAttributes.SpecialName |
                                                            MethodAttributes.HideBySig,
                                                            fieldBuilder.FieldType,
                                                            Type.EmptyTypes);

            var il = getMethodBuilder.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, fieldBuilder);
            il.Emit(OpCodes.Ret);

            return getMethodBuilder;
        }


        private static MethodBuilder CreatePropertySetter(TypeBuilder typeBuilder, FieldBuilder fieldBuilder)
        {
            var setMethodBuilder = typeBuilder.DefineMethod("set_" + fieldBuilder.Name,
                                                            MethodAttributes.Public |
                                                            MethodAttributes.SpecialName |
                                                            MethodAttributes.HideBySig,
                                                            null,
                                                            new[] { fieldBuilder.FieldType });

            var il = setMethodBuilder.GetILGenerator();

            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, fieldBuilder);
            il.Emit(OpCodes.Ret);

            return setMethodBuilder;
        }


        private static object SetProperty(object Target, string Name, object value, bool ignoreIfTargetIsNull)
        {
            if (ignoreIfTargetIsNull && Target == null)
                return null;

            object[] values = { value };

            var oldProperty = GetPropertyValue(Target, Name, false);

            var targetProperty = Target.GetType().GetProperty(Name);

            if (targetProperty == null)
            {
                throw new Exception("Object " + Target + "   does not have Target Property " + Name);
            }


            targetProperty.GetSetMethod().Invoke(Target, values);


            return oldProperty;
        }


        private static object GetPropertyValue(object Target, string Name, bool throwError)
        {
            var targetProperty = Target.GetType().GetProperty(Name);

            if (targetProperty == null)
            {
                if (throwError)
                {
                    throw new Exception("Object " + Target + "   does not have Target Property " + Name);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return targetProperty.GetGetMethod().Invoke(Target, null);
            }
        }
    }
}