﻿/* Copyright 2015 Realm Inc - All Rights Reserved
 * Proprietary and Confidential
 */
 
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using Mono.Cecil;
using NUnit.Framework;
using Realms;

namespace Tests
{
    [TestFixture]
    public class WeaverTests
    {
        #region helpers

        private static dynamic GetAutoPropertyBackingFieldValue(object o, string propertyName)
        {
            var propertyField = ((Type) o.GetType())
                .GetField($"<{propertyName}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            var fieldValue = propertyField.GetValue(o);
            return fieldValue;
        }

        private static void SetAutoPropertyBackingFieldValue(object o, string propertyName, object propertyValue)
        {
            var propertyField = ((Type) o.GetType())
                .GetField($"<{propertyName}>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance);
            propertyField.SetValue(o, propertyValue);
        }

        public static object GetPropertyValue(object o, string propName)
        {
            return o.GetType().GetProperty(propName).GetValue(o, null);
        }

        public static void SetPropertyValue(object o, string propName, object propertyValue)
        {
            o.GetType().GetProperty(propName).SetValue(o, propertyValue);
        }

        #endregion

        private Assembly _assembly;
        private string _newAssemblyPath;
        private string _assemblyPath;

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            var projectPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\AssemblyToProcess\AssemblyToProcess.csproj"));
            _assemblyPath = Path.Combine(Path.GetDirectoryName(projectPath), @"bin\Debug\AssemblyToProcess.dll");
#if (!DEBUG)
            _assemblyPath = _assemblyPath.Replace("Debug", "Release");
#endif

            _newAssemblyPath = _assemblyPath.Replace(".dll", ".processed.dll");
            File.Copy(_assemblyPath, _newAssemblyPath, true);

            var moduleDefinition = ModuleDefinition.ReadModule(_newAssemblyPath);
            var weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
                LogErrorPoint = (s, point) => { throw new Exception(s); }
            };

            weavingTask.Execute();
            moduleDefinition.Write(_newAssemblyPath);

            _assembly = Assembly.LoadFile(_newAssemblyPath);

            // Try accessing assembly to ensure that the assembly is still valid.
            try
            {
                _assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                foreach (var item in e.LoaderExceptions)
                    Debug.WriteLine("Loader exception: " + item.Message.ToString());

                Assert.Fail("Load failure");
            }
        }

        [TestCase("Int32Property", 100)]
        [TestCase("Int64Property", 100)]
        [TestCase("SingleProperty", 123.123f)]
        [TestCase("DoubleProperty", 123.123)]
        [TestCase("BooleanProperty", true)]
        [TestCase("StringProperty", "str")] 
        [TestCase("NullableInt32Property", 100)]
        [TestCase("NullableInt64Property", 100L)]
        [TestCase("NullableSingleProperty", 123.123f)] 
        [TestCase("NullableDoubleProperty", 123.123)] 
        [TestCase("NullableBooleanProperty", true)]
        public void GetValueUnmanagedShouldGetBackindField(string propertyName, object propertyValue)
        {
            // Arrange
            var o = (dynamic)Activator.CreateInstance(_assembly.GetType("AssemblyToProcess.AllTypesObject"));
            SetAutoPropertyBackingFieldValue(o, propertyName, propertyValue);

            // Act
            var returnedValue = GetPropertyValue(o, propertyName);

            // Assert
            Assert.That(o.LogList, Is.EqualTo(new List<string> { "IsManaged" }));
            Assert.That(returnedValue, Is.EqualTo(propertyValue));
        }

        [TestCase("Int32", 100)]
        [TestCase("Int64", 100)]
        [TestCase("Single", 123.123f)]
        [TestCase("Double", 123.123)]
        [TestCase("Boolean", true)]
        [TestCase("String", "str")] 
        [TestCase("NullableInt32", 100)]
        [TestCase("NullableInt64", 100L)]
        [TestCase("NullableSingle", 123.123f)] 
        [TestCase("NullableDouble", 123.123)] 
        [TestCase("NullableBoolean", true)]
        public void SetValueUnmanagedShouldSetBackingField(string typeName, object propertyValue)
        {
            // Arrange
            var propertyName = typeName + "Property";
            var o = (dynamic)Activator.CreateInstance(_assembly.GetType("AssemblyToProcess.AllTypesObject"));

            // Act
            SetPropertyValue(o, propertyName, propertyValue);

            // Assert
            Assert.That(o.LogList, Is.EqualTo(new List<string> { "IsManaged" }));
            Assert.That(GetAutoPropertyBackingFieldValue(o, propertyName), Is.EqualTo(propertyValue));
        }

        [TestCase("Int32", 100)]
        [TestCase("Int64", 100L)]
        [TestCase("Single", 123.123f)]
        [TestCase("Double", 123,123)]
        [TestCase("Boolean", true)]
        [TestCase("String", "str")] 
        [TestCase("NullableInt32", 100)]
        [TestCase("NullableInt64", 100L)]
        [TestCase("NullableSingle", 123.123f)] 
        [TestCase("NullableDouble", 123.123)] 
        [TestCase("NullableBoolean", true)]
        public void GetValueManagedShouldGetQueryDatabase(string typeName, object propertyValue)
        {
            // Arrange
            var propertyName = typeName + "Property";
            var o = (dynamic)Activator.CreateInstance(_assembly.GetType("AssemblyToProcess.AllTypesObject"));
            o.IsManaged = true;

            // Act
            GetPropertyValue(o, propertyName);

            // Assert
            Assert.That(o.LogList, Is.EqualTo(new List<string>
            {
                "IsManaged",
                "RealmObject.Get" + typeName + "Value(propertyName = \"" + propertyName + "\")"
            }));
        }

        [TestCase("Int32", 100, 0, false)]
        [TestCase("Int64", 100L, 0L, false)]
        [TestCase("Single", 123.123f, 0.0f, null)]
        [TestCase("Double", 123.123, 0.0, null)]
        [TestCase("Boolean", true, false)]
        [TestCase("String", "str", null, false)] 
        [TestCase("NullableInt32", 100, null, false)]
        [TestCase("NullableInt64", 100L, null, false)]
        [TestCase("NullableSingle", 123.123f, null, null)] 
        [TestCase("NullableDouble", 123.123, null, null)] 
        [TestCase("NullableBoolean", true, null, null)]
        public void SetValueManagedShouldUpdateDatabase(string typeName, object propertyValue, object defaultPropertyValue, bool? setUnique)
        {
            // Arrange
            var propertyName = typeName + "Property";
            var o = (dynamic)Activator.CreateInstance(_assembly.GetType("AssemblyToProcess.AllTypesObject"));
            o.IsManaged = true;

            // Act
            SetPropertyValue(o, propertyName, propertyValue);

            // Assert
            Assert.That(o.LogList, Is.EqualTo(new List<string>
            {
                "IsManaged",
                "RealmObject.Set" + typeName + "Value(propertyName = \"" + propertyName + "\", value = " + propertyValue + (setUnique != null ? $", setUnique = {setUnique.Value})" : ")")
            }));
            Assert.That(GetAutoPropertyBackingFieldValue(o, propertyName), Is.EqualTo(defaultPropertyValue));
        }


        [TestCase("Int32", 100, 0)]
        [TestCase("Int64", 100L, 0L)]
        [TestCase("String", "str", null)] 
        public void SettingIndexedPropertyShouldCallSetUnique(string typeName, object propertyValue, object defaultPropertyValue)
        {
            // Arrange
            var propertyName = typeName + "Property";
            var o = (dynamic)Activator.CreateInstance(_assembly.GetType("AssemblyToProcess.Indexed" + typeName + "Object"));
            o.IsManaged = true;

            // Act
            SetPropertyValue(o, propertyName, propertyValue);

            // Assert
            Assert.That(o.LogList, Is.EqualTo(new List<string>
            {
                "IsManaged",
                "RealmObject.Set" + typeName + "Value(propertyName = \"" + propertyName + "\", value = " + propertyValue + ", setUnique = True)"
            }));
            Assert.That(GetAutoPropertyBackingFieldValue(o, propertyName), Is.EqualTo(defaultPropertyValue));
            
        }

        [Test]
        public void ShouldNotWeaveIgnoredProperties()
        {
            // Arrange
            var o = (dynamic)Activator.CreateInstance(_assembly.GetType("AssemblyToProcess.Person"));

            // Act
            o.IsOnline = true;

            // Assert
            Assert.That(o.LogList, Is.Empty);
        }

        [Test]
        public void ShouldFollowMapToAttribute()
        {
            // Arrange
            var o = (dynamic)Activator.CreateInstance(_assembly.GetType("AssemblyToProcess.Person"));
            o.IsManaged = true;

            // Act
            o.Email = "a@b.com";

            // Assert
            Assert.That(o.LogList, Is.EqualTo(new List<string>
            {
                "IsManaged",
                "RealmObject.SetStringValue(propertyName = \"Email\", value = a@b.com, setUnique = False)"
            }));
        }

        [Test]
        public void ShouldAddWovenAttribute()
        {
            // Arrange and act
            var personType = _assembly.GetType("AssemblyToProcess.Person");

            // Assert
            Assert.That(personType.CustomAttributes.Any(a => a.AttributeType.Name == "WovenAttribute"));
        }

        [Test, Ignore("Introduce once preserving default constructors is implemented")]
        public void ShouldAddPreserveAttribute()
        {
            // Arrange and act
            var personType = _assembly.GetType("AssemblyToProcess.Person");
            var ctor = personType.GetConstructor(Type.EmptyTypes);

            // Assert
            Assert.That(ctor.GetCustomAttributes(typeof (PreserveAttribute)).Any());
        }

#if(DEBUG)
        [Test]
        public void PeVerify()
        {
            Verifier.Verify(_assemblyPath,_newAssemblyPath);
        }
#endif
    }
}