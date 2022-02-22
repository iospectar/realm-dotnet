﻿////////////////////////////////////////////////////////////////////////////
//
// Copyright 2020 Realm Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Realms.Exceptions;

namespace Realms.Tests.Database
{
    [TestFixture, Preserve(AllMembers = true)]
    public class RealmDictionaryTests : RealmInstanceTest
    {
        #region Boolean

        public static IEnumerable<TestCaseData<bool>> BoolTestValues()
        {
            yield return new TestCaseData<bool>(true);
            yield return new TestCaseData<bool>(true, ("a", true));
            yield return new TestCaseData<bool>(false, ("b", false));
            yield return new TestCaseData<bool>(true, ("a", false), ("b", true));
            yield return new TestCaseData<bool>(false, ("a", true), ("b", false), ("c", true));
        }

        public static IEnumerable<TestCaseData<bool?>> NullableBoolTestValues()
        {
            yield return new TestCaseData<bool?>(true);
            yield return new TestCaseData<bool?>(true, ("a", true));
            yield return new TestCaseData<bool?>(true, ("b", false));
            yield return new TestCaseData<bool?>(false, ("c", null));
            yield return new TestCaseData<bool?>(true, ("a", false), ("b", true));
            yield return new TestCaseData<bool?>(null, ("a", true), ("b", false), ("c", null));
        }

        [TestCaseSource(nameof(BoolTestValues))]
        public Task Bool_Unmanaged(TestCaseData<bool> testData) => RunUnmanagedTestsAsync(o => o.BooleanDictionary, testData);

        [TestCaseSource(nameof(NullableBoolTestValues))]
        public Task NullableBool_Unmanaged(TestCaseData<bool?> testData) => RunUnmanagedTestsAsync(o => o.NullableBooleanDictionary, testData);

        [TestCaseSource(nameof(BoolTestValues))]
        public Task Bool_Managed(TestCaseData<bool> testData) => RunManagedTestsAsync(o => o.BooleanDictionary, testData);

        [TestCaseSource(nameof(NullableBoolTestValues))]
        public Task NullableBool_Managed(TestCaseData<bool?> testData) => RunManagedTestsAsync(o => o.NullableBooleanDictionary, testData);

        [Test]
        public Task Bool_Notifications() => RunManagedNotificationsTestsAsync(o => o.BooleanDictionary, BoolTestValues().Last());

        [Test]
        public Task NullableBool_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableBooleanDictionary, NullableBoolTestValues().Last());

        #endregion

        #region Byte

        public static IEnumerable<TestCaseData<byte>> ByteTestValues()
        {
            yield return new TestCaseData<byte>(5);
            yield return new TestCaseData<byte>(5, ("123", 123));
            yield return new TestCaseData<byte>(5, ("a", 1), ("b", 1), ("c", 1));
            yield return new TestCaseData<byte>(5, ("a", 1), ("b", 2), ("c", 3));
            yield return new TestCaseData<byte>(5, ("a", byte.MinValue), ("z", byte.MaxValue));
            yield return new TestCaseData<byte>(5, ("a", byte.MinValue), ("zero", 0), ("one", 1), ("z", byte.MaxValue));
        }

        public static IEnumerable<TestCaseData<byte?>> NullableByteTestValues()
        {
            yield return new TestCaseData<byte?>(5);
            yield return new TestCaseData<byte?>(null, ("123", 123));
            yield return new TestCaseData<byte?>(5, ("null", null));
            yield return new TestCaseData<byte?>(1, ("null1", null), ("null2", null));
            yield return new TestCaseData<byte?>(9, ("a", 1), ("b", null), ("c", 3));
            yield return new TestCaseData<byte?>(5, ("a", byte.MinValue), ("m", null), ("z", byte.MaxValue));
            yield return new TestCaseData<byte?>(5, ("a", byte.MinValue), ("zero", 0), ("null", null), ("one", 1), ("z", byte.MaxValue));
        }

        [TestCaseSource(nameof(ByteTestValues))]
        public Task Byte_Unmanaged(TestCaseData<byte> testData) => RunUnmanagedTestsAsync(o => o.ByteDictionary, testData);

        [TestCaseSource(nameof(NullableByteTestValues))]
        public Task NullableByte_Unmanaged(TestCaseData<byte?> testData) => RunUnmanagedTestsAsync(o => o.NullableByteDictionary, testData);

        [TestCaseSource(nameof(ByteTestValues))]
        public Task Byte_Managed(TestCaseData<byte> testData) => RunManagedTestsAsync(o => o.ByteDictionary, testData);

        [TestCaseSource(nameof(NullableByteTestValues))]
        public Task NullableByte_Managed(TestCaseData<byte?> testData) => RunManagedTestsAsync(o => o.NullableByteDictionary, testData);

        [Test]
        public Task Byte_Notifications() => RunManagedNotificationsTestsAsync(o => o.ByteDictionary, ByteTestValues().Last());

        [Test]
        public Task NullableByte_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableByteDictionary, NullableByteTestValues().Last());

        #endregion

        #region Int16

        public static IEnumerable<TestCaseData<short>> Int16TestValues()
        {
            yield return new TestCaseData<short>(9);
            yield return new TestCaseData<short>(-5, ("123", 123));
            yield return new TestCaseData<short>(9, ("123", -123));
            yield return new TestCaseData<short>(19, ("a", 1), ("b", 1), ("c", 1));
            yield return new TestCaseData<short>(9, ("a", 1), ("b", 2), ("c", 3));
            yield return new TestCaseData<short>(49, ("a", short.MinValue), ("z", short.MaxValue));
            yield return new TestCaseData<short>(9, ("a", short.MinValue), ("zero", 0), ("one", 1), ("z", short.MaxValue));
        }

        public static IEnumerable<TestCaseData<short?>> NullableInt16TestValues()
        {
            yield return new TestCaseData<short?>(7);
            yield return new TestCaseData<short?>(7, ("123", 123));
            yield return new TestCaseData<short?>(null, ("123", -123));
            yield return new TestCaseData<short?>(7, ("null", null));
            yield return new TestCaseData<short?>(7, ("null1", null), ("null2", null));
            yield return new TestCaseData<short?>(-15, ("a", 1), ("b", null), ("c", 3));
            yield return new TestCaseData<short?>(7, ("a", short.MinValue), ("m", null), ("z", short.MaxValue));
            yield return new TestCaseData<short?>(7, ("a", short.MinValue), ("zero", 0), ("null", null), ("one", 1), ("z", short.MaxValue));
        }

        [TestCaseSource(nameof(Int16TestValues))]
        public Task Int16_Unmanaged(TestCaseData<short> testData) => RunUnmanagedTestsAsync(o => o.Int16Dictionary, testData);

        [TestCaseSource(nameof(NullableInt16TestValues))]
        public Task NullableInt16_Unmanaged(TestCaseData<short?> testData) => RunUnmanagedTestsAsync(o => o.NullableInt16Dictionary, testData);

        [TestCaseSource(nameof(Int16TestValues))]
        public Task Int16_Managed(TestCaseData<short> testData) => RunManagedTestsAsync(o => o.Int16Dictionary, testData);

        [TestCaseSource(nameof(NullableInt16TestValues))]
        public Task NullableInt16_Managed(TestCaseData<short?> testData) => RunManagedTestsAsync(o => o.NullableInt16Dictionary, testData);

        [Test]
        public Task Int16_Notifications() => RunManagedNotificationsTestsAsync(o => o.Int16Dictionary, Int16TestValues().Last());

        [Test]
        public Task NullableInt16_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableInt16Dictionary, NullableInt16TestValues().Last());

        #endregion

        #region Int32

        public static IEnumerable<TestCaseData<int>> Int32TestValues()
        {
            yield return new TestCaseData<int>(999);
            yield return new TestCaseData<int>(999, ("123", 123));
            yield return new TestCaseData<int>(999, ("123", -123));
            yield return new TestCaseData<int>(999, ("a", 1), ("b", 1), ("c", 1));
            yield return new TestCaseData<int>(999, ("a", 1), ("b", 2), ("c", 3));
            yield return new TestCaseData<int>(999, ("a", int.MinValue), ("z", int.MaxValue));
            yield return new TestCaseData<int>(999, ("a", int.MinValue), ("zero", 0), ("one", 1), ("z", int.MaxValue));
        }

        public static IEnumerable<TestCaseData<int?>> NullableInt32TestValues()
        {
            yield return new TestCaseData<int?>(777);
            yield return new TestCaseData<int?>(null, ("123", 123));
            yield return new TestCaseData<int?>(777, ("123", -123));
            yield return new TestCaseData<int?>(777, ("null", null));
            yield return new TestCaseData<int?>(777, ("null1", null), ("null2", null));
            yield return new TestCaseData<int?>(777, ("a", 1), ("b", null), ("c", 3));
            yield return new TestCaseData<int?>(null, ("a", int.MinValue), ("m", null), ("z", int.MaxValue));
            yield return new TestCaseData<int?>(777, ("a", int.MinValue), ("zero", 0), ("null", null), ("one", 1), ("z", int.MaxValue));
        }

        [TestCaseSource(nameof(Int32TestValues))]
        public Task Int32_Unmanaged(TestCaseData<int> testData) => RunUnmanagedTestsAsync(o => o.Int32Dictionary, testData);

        [TestCaseSource(nameof(NullableInt32TestValues))]
        public Task NullableInt32_Unmanaged(TestCaseData<int?> testData) => RunUnmanagedTestsAsync(o => o.NullableInt32Dictionary, testData);

        [TestCaseSource(nameof(Int32TestValues))]
        public Task Int32_Managed(TestCaseData<int> testData) => RunManagedTestsAsync(o => o.Int32Dictionary, testData);

        [TestCaseSource(nameof(NullableInt32TestValues))]
        public Task NullableInt32_Managed(TestCaseData<int?> testData) => RunManagedTestsAsync(o => o.NullableInt32Dictionary, testData);

        [Test]
        public Task Int32_Notifications() => RunManagedNotificationsTestsAsync(o => o.Int32Dictionary, Int32TestValues().Last());

        [Test]
        public Task NullableInt32_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableInt32Dictionary, NullableInt32TestValues().Last());

        #endregion

        #region Int64

        public static IEnumerable<TestCaseData<long>> Int64TestValues()
        {
            yield return new TestCaseData<long>(123456789);
            yield return new TestCaseData<long>(123456789, ("123", 123));
            yield return new TestCaseData<long>(123456789, ("123", -123));
            yield return new TestCaseData<long>(123456789, ("a", 1), ("b", 1), ("c", 1));
            yield return new TestCaseData<long>(123456789, ("a", 1), ("b", 2), ("c", 3));
            yield return new TestCaseData<long>(123456789, ("a", long.MinValue), ("z", long.MaxValue));
            yield return new TestCaseData<long>(123456789, ("a", long.MinValue), ("zero", 0), ("one", 1), ("z", long.MaxValue));
        }

        public static IEnumerable<TestCaseData<long?>> NullableInt64TestValues()
        {
            yield return new TestCaseData<long?>(1234);
            yield return new TestCaseData<long?>(null, ("123", 123));
            yield return new TestCaseData<long?>(1234, ("123", -123));
            yield return new TestCaseData<long?>(1234, ("null", null));
            yield return new TestCaseData<long?>(1234, ("null1", null), ("null2", null));
            yield return new TestCaseData<long?>(null, ("a", 1), ("b", null), ("c", 3));
            yield return new TestCaseData<long?>(1234, ("a", long.MinValue), ("m", null), ("z", long.MaxValue));
            yield return new TestCaseData<long?>(1234, ("a", long.MinValue), ("zero", 0), ("null", null), ("one", 1), ("z", long.MaxValue));
        }

        [TestCaseSource(nameof(Int64TestValues))]
        public Task Int64_Unmanaged(TestCaseData<long> testData) => RunUnmanagedTestsAsync(o => o.Int64Dictionary, testData);

        [TestCaseSource(nameof(NullableInt64TestValues))]
        public Task NullableInt64_Unmanaged(TestCaseData<long?> testData) => RunUnmanagedTestsAsync(o => o.NullableInt64Dictionary, testData);

        [TestCaseSource(nameof(Int64TestValues))]
        public Task Int64_Managed(TestCaseData<long> testData) => RunManagedTestsAsync(o => o.Int64Dictionary, testData);

        [TestCaseSource(nameof(NullableInt64TestValues))]
        public Task NullableInt64_Managed(TestCaseData<long?> testData) => RunManagedTestsAsync(o => o.NullableInt64Dictionary, testData);

        [Test]
        public Task Int64_Notifications() => RunManagedNotificationsTestsAsync(o => o.Int64Dictionary, Int64TestValues().Last());

        [Test]
        public Task NullableInt64_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableInt64Dictionary, NullableInt64TestValues().Last());
        #endregion

        #region Float

        public static IEnumerable<TestCaseData<float>> FloatTestValues()
        {
            yield return new TestCaseData<float>(1.23f);
            yield return new TestCaseData<float>(1.23f, ("123", 123.123f));
            yield return new TestCaseData<float>(1.23f, ("123", -123.456f));
            yield return new TestCaseData<float>(1.23f, ("a", 1.1f), ("b", 1.1f), ("c", 1.1f));
            yield return new TestCaseData<float>(0.8f, ("a", 1), ("b", 2.2f), ("c", 3.3f));
            yield return new TestCaseData<float>(1.23f, ("a", float.MinValue), ("z", float.MaxValue));
            yield return new TestCaseData<float>(1.23f, ("a", float.MinValue), ("zero", 0.0f), ("one", 1.1f), ("z", float.MaxValue));
        }

        public static IEnumerable<TestCaseData<float?>> NullableFloatTestValues()
        {
            yield return new TestCaseData<float?>(999.888f);
            yield return new TestCaseData<float?>(999.888f, ("123", 123.123f));
            yield return new TestCaseData<float?>(999.888f, ("123", -123.456f));
            yield return new TestCaseData<float?>(null, ("null", null));
            yield return new TestCaseData<float?>(999.888f, ("null1", null), ("null2", null));
            yield return new TestCaseData<float?>(null, ("a", 1), ("b", null), ("c", 3.3f));
            yield return new TestCaseData<float?>(999.888f, ("a", float.MinValue), ("m", null), ("z", float.MaxValue));
            yield return new TestCaseData<float?>(999.888f, ("a", float.MinValue), ("zero", 0), ("null", null), ("one", 1.1f), ("z", float.MaxValue));
        }

        [TestCaseSource(nameof(FloatTestValues))]
        public Task Float_Unmanaged(TestCaseData<float> testData) => RunUnmanagedTestsAsync(o => o.SingleDictionary, testData);

        [TestCaseSource(nameof(NullableFloatTestValues))]
        public Task NullableFloat_Unmanaged(TestCaseData<float?> testData) => RunUnmanagedTestsAsync(o => o.NullableSingleDictionary, testData);

        [TestCaseSource(nameof(FloatTestValues))]
        public Task Float_Managed(TestCaseData<float> testData) => RunManagedTestsAsync(o => o.SingleDictionary, testData);

        [TestCaseSource(nameof(NullableFloatTestValues))]
        public Task NullableFloat_Managed(TestCaseData<float?> testData) => RunManagedTestsAsync(o => o.NullableSingleDictionary, testData);

        [Test]
        public Task Float_Notifications() => RunManagedNotificationsTestsAsync(o => o.SingleDictionary, FloatTestValues().Last());

        [Test]
        public Task NullableFloat_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableSingleDictionary, NullableFloatTestValues().Last());

        #endregion

        #region Double

        public static IEnumerable<TestCaseData<double>> DoubleTestValues()
        {
            yield return new TestCaseData<double>(789.123);
            yield return new TestCaseData<double>(789.123, ("123", 123.123));
            yield return new TestCaseData<double>(789.123, ("123", -123.456));
            yield return new TestCaseData<double>(789.123, ("a", 1.1), ("b", 1.1), ("c", 1.1));
            yield return new TestCaseData<double>(789.123, ("a", 1), ("b", 2.2), ("c", 3.3));
            yield return new TestCaseData<double>(789.123, ("a", 1), ("b", 2.2), ("c", 3.3), ("d", 4385948963486946854968945789458794538793438693486934869.238593285932859238952398));
            yield return new TestCaseData<double>(789.123, ("a", double.MinValue), ("z", double.MaxValue));
            yield return new TestCaseData<double>(789.123, ("a", double.MinValue), ("zero", 0.0), ("one", 1.1), ("z", double.MaxValue));
        }

        public static IEnumerable<TestCaseData<double?>> NullableDoubleTestValues()
        {
            yield return new TestCaseData<double?>(-123.789);
            yield return new TestCaseData<double?>(-123.789, ("123", 123.123));
            yield return new TestCaseData<double?>(null, ("123", -123.456));
            yield return new TestCaseData<double?>(-123.789, ("null", null));
            yield return new TestCaseData<double?>(-123.789, ("null1", null), ("null2", null));
            yield return new TestCaseData<double?>(-123.789, ("a", 1), ("b", null), ("c", 3.3));
            yield return new TestCaseData<double?>(null, ("a", 1), ("b", null), ("c", 3.3), ("d", 4385948963486946854968945789458794538793438693486934869.238593285932859238952398));
            yield return new TestCaseData<double?>(-123.789, ("a", double.MinValue), ("m", null), ("z", double.MaxValue));
            yield return new TestCaseData<double?>(-123.789, ("a", double.MinValue), ("zero", 0), ("null", null), ("one", 1.1), ("z", double.MaxValue));
        }

        [TestCaseSource(nameof(DoubleTestValues))]
        public Task Double_Unmanaged(TestCaseData<double> testData) => RunUnmanagedTestsAsync(o => o.DoubleDictionary, testData);

        [TestCaseSource(nameof(NullableDoubleTestValues))]
        public Task NullableDouble_Unmanaged(TestCaseData<double?> testData) => RunUnmanagedTestsAsync(o => o.NullableDoubleDictionary, testData);

        [TestCaseSource(nameof(DoubleTestValues))]
        public Task Double_Managed(TestCaseData<double> testData) => RunManagedTestsAsync(o => o.DoubleDictionary, testData);

        [TestCaseSource(nameof(NullableDoubleTestValues))]
        public Task NullableDouble_Managed(TestCaseData<double?> testData) => RunManagedTestsAsync(o => o.NullableDoubleDictionary, testData);

        [Test]
        public Task Double_Notifications() => RunManagedNotificationsTestsAsync(o => o.DoubleDictionary, DoubleTestValues().Last());

        [Test]
        public Task NullableDouble_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableDoubleDictionary, NullableDoubleTestValues().Last());

        #endregion

        #region Decimal

        public static IEnumerable<TestCaseData<decimal>> DecimalTestValues()
        {
            yield return new TestCaseData<decimal>(11.11m);
            yield return new TestCaseData<decimal>(11.11m, ("123", 123.123m));
            yield return new TestCaseData<decimal>(11.11m, ("123", -123.456m));
            yield return new TestCaseData<decimal>(11.11m, ("a", 1.1m), ("b", 1.1m), ("c", 1.1m));
            yield return new TestCaseData<decimal>(11.11m, ("a", 1), ("b", 2.2m), ("c", 3.3m));
            yield return new TestCaseData<decimal>(11.11m, ("a", 1), ("b", 2.2m), ("c", 3.3m), ("d", 43859489538793438693486934869.238436346943634634634634634634634634634593285932859238952398m));
            yield return new TestCaseData<decimal>(11.11m, ("a", decimal.MinValue), ("z", decimal.MaxValue));
            yield return new TestCaseData<decimal>(11.11m, ("a", decimal.MinValue), ("zero", 0.0m), ("one", 1.1m), ("z", decimal.MaxValue));
        }

        public static IEnumerable<TestCaseData<decimal?>> NullableDecimalTestValues()
        {
            yield return new TestCaseData<decimal?>(12.12m);
            yield return new TestCaseData<decimal?>(null, ("123", 123.123m));
            yield return new TestCaseData<decimal?>(12.12m, ("123", -123.456m));
            yield return new TestCaseData<decimal?>(12.12m, ("null", null));
            yield return new TestCaseData<decimal?>(12.12m, ("null1", null), ("null2", null));
            yield return new TestCaseData<decimal?>(12.12m, ("a", 1), ("b", null), ("c", 3.3m));
            yield return new TestCaseData<decimal?>(12.12m, ("a", 1), ("b", null), ("c", 3.3m), ("d", 43859489538793438693486934869.238436346943634634634634634634634634634593285932859238952398m));
            yield return new TestCaseData<decimal?>(12.12m, ("a", decimal.MinValue), ("m", null), ("z", decimal.MaxValue));
            yield return new TestCaseData<decimal?>(null, ("a", decimal.MinValue), ("zero", 0), ("null", null), ("one", 1.1m), ("z", decimal.MaxValue));
        }

        [TestCaseSource(nameof(DecimalTestValues))]
        public Task Decimal_Unmanaged(TestCaseData<decimal> testData) => RunUnmanagedTestsAsync(o => o.DecimalDictionary, testData);

        [TestCaseSource(nameof(NullableDecimalTestValues))]
        public Task NullableDecimal_Unmanaged(TestCaseData<decimal?> testData) => RunUnmanagedTestsAsync(o => o.NullableDecimalDictionary, testData);

        [TestCaseSource(nameof(DecimalTestValues))]
        public Task Decimal_Managed(TestCaseData<decimal> testData) => RunManagedTestsAsync(o => o.DecimalDictionary, testData);

        [TestCaseSource(nameof(NullableDecimalTestValues))]
        public Task NullableDecimal_Managed(TestCaseData<decimal?> testData) => RunManagedTestsAsync(o => o.NullableDecimalDictionary, testData);

        [Test]
        public Task Decimal_Notifications() => RunManagedNotificationsTestsAsync(o => o.DecimalDictionary, DecimalTestValues().Last());

        [Test]
        public Task NullableDecimal_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableDecimalDictionary, NullableDecimalTestValues().Last());

        #endregion

        #region Decimal128

        public static IEnumerable<TestCaseData<Decimal128>> Decimal128TestValues()
        {
            yield return new TestCaseData<Decimal128>(1.5m);
            yield return new TestCaseData<Decimal128>(1.5m, ("123", 123.123m));
            yield return new TestCaseData<Decimal128>(1.5m, ("123", -123.456m));
            yield return new TestCaseData<Decimal128>(1.5m, ("a", 1.1m), ("b", 1.1m), ("c", 1.1m));
            yield return new TestCaseData<Decimal128>(1.5m, ("a", 1), ("b", 2.2m), ("c", 3.3m));
            yield return new TestCaseData<Decimal128>(1.5m, ("a", 1), ("b", 2.2m), ("c", 3.3m), ("d", 43859489538793438693486934869.238436346943634634634634634634634634634593285932859238952398m));
            yield return new TestCaseData<Decimal128>(1.5m, ("a", decimal.MinValue), ("a1", Decimal128.MinValue), ("z", decimal.MaxValue), ("z1", Decimal128.MaxValue));
            yield return new TestCaseData<Decimal128>(1.5m, ("a", Decimal128.MinValue), ("zero", 0.0m), ("one", 1.1m), ("z", Decimal128.MaxValue));
        }

        public static IEnumerable<TestCaseData<Decimal128?>> NullableDecimal128TestValues()
        {
            yield return new TestCaseData<Decimal128?>(-9.7m);
            yield return new TestCaseData<Decimal128?>(-9.7m, ("123", 123.123m));
            yield return new TestCaseData<Decimal128?>(-9.7m, ("123", -123.456m));
            yield return new TestCaseData<Decimal128?>(-9.7m, ("null", null));
            yield return new TestCaseData<Decimal128?>(-9.7m, ("null1", null), ("null2", null));
            yield return new TestCaseData<Decimal128?>(-9.7m, ("a", 1), ("b", null), ("c", 3.3m));
            yield return new TestCaseData<Decimal128?>(-9.7m, ("a", 1), ("b", null), ("c", 3.3m), ("d", 43859489538793438693486934869.238436346943634634634634634634634634634593285932859238952398m));
            yield return new TestCaseData<Decimal128?>(-9.7m, ("a", decimal.MinValue), ("a1", Decimal128.MinValue), ("m", null), ("z", decimal.MaxValue), ("z1", Decimal128.MaxValue));
            yield return new TestCaseData<Decimal128?>(-9.7m, ("a", Decimal128.MinValue), ("zero", 0), ("null", null), ("one", 1.1m), ("z", Decimal128.MaxValue));
        }

        [TestCaseSource(nameof(Decimal128TestValues))]
        public Task Decimal128_Unmanaged(TestCaseData<Decimal128> testData) => RunUnmanagedTestsAsync(o => o.Decimal128Dictionary, testData);

        [TestCaseSource(nameof(NullableDecimal128TestValues))]
        public Task NullableDecimal128_Unmanaged(TestCaseData<Decimal128?> testData) => RunUnmanagedTestsAsync(o => o.NullableDecimal128Dictionary, testData);

        [TestCaseSource(nameof(Decimal128TestValues))]
        public Task Decimal128_Managed(TestCaseData<Decimal128> testData) => RunManagedTestsAsync(o => o.Decimal128Dictionary, testData);

        [TestCaseSource(nameof(NullableDecimal128TestValues))]
        public Task NullableDecimal128_Managed(TestCaseData<Decimal128?> testData) => RunManagedTestsAsync(o => o.NullableDecimal128Dictionary, testData);

        [Test]
        public Task Decimal128_Notifications() => RunManagedNotificationsTestsAsync(o => o.Decimal128Dictionary, Decimal128TestValues().Last());

        [Test]
        public Task NullableDecimal128_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableDecimal128Dictionary, NullableDecimal128TestValues().Last());

        #endregion

        #region ObjectId

        private static readonly ObjectId ObjectId0 = TestHelpers.GenerateRepetitiveObjectId(0);
        private static readonly ObjectId ObjectId1 = TestHelpers.GenerateRepetitiveObjectId(1);
        private static readonly ObjectId ObjectId2 = TestHelpers.GenerateRepetitiveObjectId(2);
        private static readonly ObjectId ObjectIdMax = TestHelpers.GenerateRepetitiveObjectId(byte.MaxValue);

        public static IEnumerable<TestCaseData<ObjectId>> ObjectIdTestValues()
        {
            yield return new TestCaseData<ObjectId>(ObjectId.GenerateNewId());
            yield return new TestCaseData<ObjectId>(ObjectId.GenerateNewId(), ("123", ObjectId1));
            yield return new TestCaseData<ObjectId>(ObjectId.GenerateNewId(), ("123", ObjectId2));
            yield return new TestCaseData<ObjectId>(ObjectId.GenerateNewId(), ("a", ObjectId1), ("b", ObjectId1), ("c", ObjectId1));
            yield return new TestCaseData<ObjectId>(ObjectId.GenerateNewId(), ("a", ObjectId0), ("b", ObjectId1), ("c", ObjectId2));
            yield return new TestCaseData<ObjectId>(ObjectId.GenerateNewId(), ("a", ObjectId0), ("z", ObjectIdMax));
            yield return new TestCaseData<ObjectId>(ObjectId.GenerateNewId(), ("a", ObjectId0), ("zero", ObjectId1), ("one", ObjectId2), ("z", ObjectIdMax));
        }

        public static IEnumerable<TestCaseData<ObjectId?>> NullableObjectIdTestValues()
        {
            yield return new TestCaseData<ObjectId?>(ObjectId.GenerateNewId());
            yield return new TestCaseData<ObjectId?>(ObjectId.GenerateNewId(), ("123", ObjectId1));
            yield return new TestCaseData<ObjectId?>(ObjectId.GenerateNewId(), ("123", ObjectId2));
            yield return new TestCaseData<ObjectId?>(ObjectId.GenerateNewId(), ("null", null));
            yield return new TestCaseData<ObjectId?>(ObjectId.GenerateNewId(), ("null1", null), ("null2", null));
            yield return new TestCaseData<ObjectId?>(null, ("a", ObjectId0), ("b", null), ("c", ObjectId2));
            yield return new TestCaseData<ObjectId?>(ObjectId.GenerateNewId(), ("a", ObjectId2), ("b", null), ("c", ObjectId1), ("d", ObjectId0));
            yield return new TestCaseData<ObjectId?>(ObjectId.GenerateNewId(), ("a", ObjectId0), ("m", null), ("z", ObjectIdMax));
            yield return new TestCaseData<ObjectId?>(null, ("a", ObjectId0), ("zero", ObjectId1), ("null", null), ("one", ObjectId2), ("z", ObjectIdMax));
        }

        [TestCaseSource(nameof(ObjectIdTestValues))]
        public Task ObjectId_Unmanaged(TestCaseData<ObjectId> testData) => RunUnmanagedTestsAsync(o => o.ObjectIdDictionary, testData);

        [TestCaseSource(nameof(NullableObjectIdTestValues))]
        public Task NullableObjectId_Unmanaged(TestCaseData<ObjectId?> testData) => RunUnmanagedTestsAsync(o => o.NullableObjectIdDictionary, testData);

        [TestCaseSource(nameof(ObjectIdTestValues))]
        public Task ObjectId_Managed(TestCaseData<ObjectId> testData) => RunManagedTestsAsync(o => o.ObjectIdDictionary, testData);

        [TestCaseSource(nameof(NullableObjectIdTestValues))]
        public Task NullableObjectId_Managed(TestCaseData<ObjectId?> testData) => RunManagedTestsAsync(o => o.NullableObjectIdDictionary, testData);

        [Test]
        public Task ObjectId_Notifications() => RunManagedNotificationsTestsAsync(o => o.ObjectIdDictionary, ObjectIdTestValues().Last());

        [Test]
        public Task NullableObjectId_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableObjectIdDictionary, NullableObjectIdTestValues().Last());

        #endregion

        #region DateTimeOffset

        private static readonly DateTimeOffset Date0 = new DateTimeOffset(0, TimeSpan.Zero);
        private static readonly DateTimeOffset Date1 = new DateTimeOffset(1999, 3, 4, 5, 30, 0, TimeSpan.Zero);
        private static readonly DateTimeOffset Date2 = new DateTimeOffset(2030, 1, 3, 9, 25, 34, TimeSpan.FromHours(3));

        public static IEnumerable<TestCaseData<DateTimeOffset>> DateTimeOffsetTestValues()
        {
            yield return new TestCaseData<DateTimeOffset>(DateTimeOffset.UtcNow);
            yield return new TestCaseData<DateTimeOffset>(DateTimeOffset.UtcNow, ("123", Date1));
            yield return new TestCaseData<DateTimeOffset>(DateTimeOffset.UtcNow, ("123", Date2));
            yield return new TestCaseData<DateTimeOffset>(DateTimeOffset.UtcNow, ("a", Date1), ("b", Date1), ("c", Date1));
            yield return new TestCaseData<DateTimeOffset>(DateTimeOffset.UtcNow, ("a", Date0), ("b", Date1), ("c", Date2));
            yield return new TestCaseData<DateTimeOffset>(DateTimeOffset.UtcNow, ("a", DateTimeOffset.MinValue), ("z", DateTimeOffset.MaxValue));
            yield return new TestCaseData<DateTimeOffset>(DateTimeOffset.UtcNow, ("a", DateTimeOffset.MinValue), ("zero", Date1), ("one", Date2), ("z", DateTimeOffset.MaxValue));
        }

        public static IEnumerable<TestCaseData<DateTimeOffset?>> NullableDateTimeOffsetTestValues()
        {
            yield return new TestCaseData<DateTimeOffset?>(null);
            yield return new TestCaseData<DateTimeOffset?>(DateTimeOffset.UtcNow, ("123", Date1));
            yield return new TestCaseData<DateTimeOffset?>(DateTimeOffset.UtcNow, ("123", Date2));
            yield return new TestCaseData<DateTimeOffset?>(DateTimeOffset.UtcNow, ("null", null));
            yield return new TestCaseData<DateTimeOffset?>(DateTimeOffset.UtcNow, ("null1", null), ("null2", null));
            yield return new TestCaseData<DateTimeOffset?>(DateTimeOffset.UtcNow, ("a", Date0), ("b", null), ("c", Date2));
            yield return new TestCaseData<DateTimeOffset?>(DateTimeOffset.UtcNow, ("a", Date2), ("b", null), ("c", Date1), ("d", Date0));
            yield return new TestCaseData<DateTimeOffset?>(DateTimeOffset.UtcNow, ("a", DateTimeOffset.MinValue), ("m", null), ("z", DateTimeOffset.MaxValue));
            yield return new TestCaseData<DateTimeOffset?>(DateTimeOffset.UtcNow, ("a", DateTimeOffset.MinValue), ("zero", Date1), ("null", null), ("one", Date2), ("z", DateTimeOffset.MaxValue));
        }

        [TestCaseSource(nameof(DateTimeOffsetTestValues))]
        public Task DateTimeOffset_Unmanaged(TestCaseData<DateTimeOffset> testData) => RunUnmanagedTestsAsync(o => o.DateTimeOffsetDictionary, testData);

        [TestCaseSource(nameof(NullableDateTimeOffsetTestValues))]
        public Task NullableDateTimeOffset_Unmanaged(TestCaseData<DateTimeOffset?> testData) => RunUnmanagedTestsAsync(o => o.NullableDateTimeOffsetDictionary, testData);

        [TestCaseSource(nameof(DateTimeOffsetTestValues))]
        public Task DateTimeOffset_Managed(TestCaseData<DateTimeOffset> testData) => RunManagedTestsAsync(o => o.DateTimeOffsetDictionary, testData);

        [TestCaseSource(nameof(NullableDateTimeOffsetTestValues))]
        public Task NullableDateTimeOffset_Managed(TestCaseData<DateTimeOffset?> testData) => RunManagedTestsAsync(o => o.NullableDateTimeOffsetDictionary, testData);

        [Test]
        public Task DateTimeOffset_Notifications() => RunManagedNotificationsTestsAsync(o => o.DateTimeOffsetDictionary, DateTimeOffsetTestValues().Last());

        [Test]
        public Task NullableDateTimeOffset_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableDateTimeOffsetDictionary, NullableDateTimeOffsetTestValues().Last());

        #endregion

        #region String

        public static IEnumerable<TestCaseData<string>> StringTestValues()
        {
            yield return new TestCaseData<string>(string.Empty);
            yield return new TestCaseData<string>(string.Empty, ("123", "abc"));
            yield return new TestCaseData<string>(string.Empty, ("123", "ced"));
            yield return new TestCaseData<string>(string.Empty, ("a", "AbCdEfG"), ("b", "HiJklMn"), ("c", "OpQrStU"));
            yield return new TestCaseData<string>(string.Empty, ("a", "vwxyz"), ("b", string.Empty), ("c", " "));
            yield return new TestCaseData<string>(string.Empty, ("a", string.Empty), ("z", "aa bb cc dd ee ff gg hh ii jj kk ll mm nn oo pp qq rr ss tt uu vv ww xx yy zz"));
            yield return new TestCaseData<string>(string.Empty, ("a", string.Empty), ("zero", "lorem ipsum"), ("one", "-1234567890"), ("z", "lololo"));
        }

        public static IEnumerable<TestCaseData<string>> NullableStringTestValues()
        {
            yield return new TestCaseData<string>(string.Empty);
            yield return new TestCaseData<string>(null, ("123", "abc"));
            yield return new TestCaseData<string>(string.Empty, ("123", "ced"));
            yield return new TestCaseData<string>(string.Empty, ("null", null));
            yield return new TestCaseData<string>(string.Empty, ("null1", null), ("null2", null));
            yield return new TestCaseData<string>(string.Empty, ("a", "AbCdEfG"), ("b", null), ("c", "OpQrStU"));
            yield return new TestCaseData<string>(null, ("a", "vwxyz"), ("b", null), ("c", string.Empty), ("d", " "));
            yield return new TestCaseData<string>(string.Empty, ("a", string.Empty), ("m", null), ("z", "aa bb cc dd ee ff gg hh ii jj kk ll mm nn oo pp qq rr ss tt uu vv ww xx yy zz"));
            yield return new TestCaseData<string>(string.Empty, ("a", string.Empty), ("zero", "lorem ipsum"), ("null", null), ("one", "-1234567890"), ("z", "lololo"));
        }

        [TestCaseSource(nameof(StringTestValues))]
        public Task String_Unmanaged(TestCaseData<string> testData) => RunUnmanagedTestsAsync(o => o.StringDictionary, testData);

        [TestCaseSource(nameof(NullableStringTestValues))]
        public Task NullableString_Unmanaged(TestCaseData<string> testData) => RunUnmanagedTestsAsync(o => o.NullableStringDictionary, testData);

        [TestCaseSource(nameof(StringTestValues))]
        public Task String_Managed(TestCaseData<string> testData) => RunManagedTestsAsync(o => o.StringDictionary, testData);

        [TestCaseSource(nameof(NullableStringTestValues))]
        public Task NullableString_Managed(TestCaseData<string> testData) => RunManagedTestsAsync(o => o.NullableStringDictionary, testData);

        [Test]
        public Task String_Notifications() => RunManagedNotificationsTestsAsync(o => o.StringDictionary, StringTestValues().Last());

        [Test]
        public Task NullableString_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableStringDictionary, NullableStringTestValues().Last());

        #endregion

        #region Binary

        public static IEnumerable<TestCaseData<byte[]>> BinaryTestValues()
        {
            yield return new TestCaseData<byte[]>(Array.Empty<byte>());
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("123", new byte[] { 1, 2, 3 }));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("123", new byte[] { 4, 5, 6 }));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("a", new byte[] { 2, 54, 98, 123 }), ("b", new byte[] { byte.MinValue, byte.MaxValue, 0 }), ("c", new byte[] { 1, 1, 1 }));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("a", new byte[] { 1 }), ("b", Array.Empty<byte>()), ("c", new byte[] { 0 }));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("a", Array.Empty<byte>()), ("z", new byte[] { 11, 22, 33, 44, 55, 66, 77, 88, 99 }));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("a", Array.Empty<byte>()), ("zero", new byte[] { byte.MinValue }), ("one", new byte[] { byte.MaxValue }), ("z", new byte[] { 99, 77, 55, 33, 128 }));
        }

        public static IEnumerable<TestCaseData<byte[]>> NullableBinaryTestValues()
        {
            yield return new TestCaseData<byte[]>(Array.Empty<byte>());
            yield return new TestCaseData<byte[]>(null, ("123", new byte[] { 1, 2, 3 }));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("123", new byte[] { 4, 5, 6 }));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("null", null));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("null1", null), ("null2", null));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("a", new byte[] { byte.MinValue, byte.MaxValue, 0 }), ("b", null), ("c", new byte[] { 1, 1, 1 }));
            yield return new TestCaseData<byte[]>(null, ("a", new byte[] { 1 }), ("b", null), ("c", Array.Empty<byte>()), ("d", new byte[] { 0 }));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("a", Array.Empty<byte>()), ("m", null), ("z", new byte[] { 11, 22, 33, 44, 55, 66, 77, 88, 99 }));
            yield return new TestCaseData<byte[]>(Array.Empty<byte>(), ("a", Array.Empty<byte>()), ("zero", new byte[] { byte.MinValue }), ("null", null), ("one", new byte[] { byte.MaxValue }), ("z", new byte[] { 99, 77, 55, 33, 128 }));
        }

        [TestCaseSource(nameof(BinaryTestValues))]
        public Task Binary_Unmanaged(TestCaseData<byte[]> testData) => RunUnmanagedTestsAsync(o => o.BinaryDictionary, testData);

        [TestCaseSource(nameof(NullableBinaryTestValues))]
        public Task NullableBinary_Unmanaged(TestCaseData<byte[]> testData) => RunUnmanagedTestsAsync(o => o.NullableBinaryDictionary, testData);

        [TestCaseSource(nameof(BinaryTestValues))]
        public Task Binary_Managed(TestCaseData<byte[]> testData) => RunManagedTestsAsync(o => o.BinaryDictionary, testData);

        [TestCaseSource(nameof(NullableBinaryTestValues))]
        public Task NullableBinary_Managed(TestCaseData<byte[]> testData) => RunManagedTestsAsync(o => o.NullableBinaryDictionary, testData);

        [Test]
        public Task Binary_Notifications() => RunManagedNotificationsTestsAsync(o => o.BinaryDictionary, BinaryTestValues().Last());

        [Test]
        public Task NullableBinary_Notifications() => RunManagedNotificationsTestsAsync(o => o.NullableBinaryDictionary, NullableBinaryTestValues().Last());

        #endregion

        #region Objects

        public static IEnumerable<TestCaseData<IntPropertyObject>> ObjectTestValues()
        {
            yield return new TestCaseData<IntPropertyObject>(null);
            yield return new TestCaseData<IntPropertyObject>(obj => obj == null ? null : new IntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, null, ("123", new IntPropertyObject { Int = 1 }));
            yield return new TestCaseData<IntPropertyObject>(obj => obj == null ? null : new IntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, new IntPropertyObject(), ("123", new IntPropertyObject { Int = 1 }));
            yield return new TestCaseData<IntPropertyObject>(obj => obj == null ? null : new IntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, new IntPropertyObject(), ("null", null));
            yield return new TestCaseData<IntPropertyObject>(obj => obj == null ? null : new IntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, new IntPropertyObject(), ("null1", null), ("null2", null));

            var sameObject = new IntPropertyObject();
            yield return new TestCaseData<IntPropertyObject>(obj => obj == null ? null : new IntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, new IntPropertyObject(), ("a", sameObject), ("b", null), ("c", sameObject));
        }

        [TestCaseSource(nameof(ObjectTestValues))]
        public Task Object_Unmanaged(TestCaseData<IntPropertyObject> testData) => RunUnmanagedTestsAsync(o => o.ObjectDictionary, testData);

        [TestCaseSource(nameof(ObjectTestValues))]
        public Task Object_Managed(TestCaseData<IntPropertyObject> testData) => RunManagedTestsAsync(o => o.ObjectDictionary, testData);

        [Test]
        public Task Object_Notifications() => RunManagedNotificationsTestsAsync(o => o.ObjectDictionary, ObjectTestValues().Last());

        #endregion

        #region Embedded Objects

        public static IEnumerable<TestCaseData<EmbeddedIntPropertyObject>> EmbeddedObjectTestValues()
        {
            yield return new TestCaseData<EmbeddedIntPropertyObject>(null);
            yield return new TestCaseData<EmbeddedIntPropertyObject>(obj => obj == null ? null : new EmbeddedIntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, null, ("123", new EmbeddedIntPropertyObject { Int = 1 }));
            yield return new TestCaseData<EmbeddedIntPropertyObject>(obj => obj == null ? null : new EmbeddedIntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, new EmbeddedIntPropertyObject(), ("123", new EmbeddedIntPropertyObject { Int = 1 }));
            yield return new TestCaseData<EmbeddedIntPropertyObject>(obj => obj == null ? null : new EmbeddedIntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, new EmbeddedIntPropertyObject(), ("null", null));
            yield return new TestCaseData<EmbeddedIntPropertyObject>(obj => obj == null ? null : new EmbeddedIntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, new EmbeddedIntPropertyObject(), ("null1", null), ("null2", null));
            yield return new TestCaseData<EmbeddedIntPropertyObject>(obj => obj == null ? null : new EmbeddedIntPropertyObject { Int = obj.Int }, (x, y) => x?.Int == y?.Int, new EmbeddedIntPropertyObject(), ("a", new EmbeddedIntPropertyObject { Int = 1 }), ("b", null), ("c", new EmbeddedIntPropertyObject { Int = 2 }));
        }

        [TestCaseSource(nameof(EmbeddedObjectTestValues))]
        public Task EmbeddedObject_Unmanaged(TestCaseData<EmbeddedIntPropertyObject> testData) => RunUnmanagedTestsAsync(o => o.EmbeddedObjectDictionary, testData);

        [TestCaseSource(nameof(EmbeddedObjectTestValues))]
        public Task EmbeddedObject_Managed(TestCaseData<EmbeddedIntPropertyObject> testData) => RunManagedTestsAsync(o => o.EmbeddedObjectDictionary, testData);

        [Test]
        public Task EmbeddedObject_Notifications() => RunManagedNotificationsTestsAsync(o => o.EmbeddedObjectDictionary, EmbeddedObjectTestValues().Last());

        #endregion

        #region RealmValue

        public static IEnumerable<TestCaseData<RealmValue>> RealmValueTestValues()
        {
            yield return new TestCaseData<RealmValue>(
                "sampleValue",
                ("nullKey", RealmValue.Null),
                ("intKey", RealmValue.Create(10, RealmValueType.Int)),
                ("boolKey", RealmValue.Create(true, RealmValueType.Bool)),
                ("stringKey", RealmValue.Create("abc", RealmValueType.String)),
                ("dataKey", RealmValue.Create(new byte[] { 0, 1, 2 }, RealmValueType.Data)),
                ("dateKey", RealmValue.Create(DateTimeOffset.FromUnixTimeSeconds(1616137641), RealmValueType.Date)),
                ("floatKey", RealmValue.Create(1.5f, RealmValueType.Float)),
                ("doubleKey", RealmValue.Create(2.5d, RealmValueType.Double)),
                ("decimalKey", RealmValue.Create(5m, RealmValueType.Decimal128)),
                ("objectIdKey", RealmValue.Create(new ObjectId("5f63e882536de46d71877979"), RealmValueType.ObjectId)),
                ("guidKey", RealmValue.Create(new Guid("{F2952191-A847-41C3-8362-497F92CB7D24}"), RealmValueType.Guid)),
                ("objectKey", RealmValue.Create(new IntPropertyObject { Int = 10 }, RealmValueType.Object)));
        }

        [TestCaseSource(nameof(RealmValueTestValues))]
        public Task RealmValue_Unmanaged(TestCaseData<RealmValue> testData) => RunUnmanagedTestsAsync(o => o.RealmValueDictionary, testData);

        [TestCaseSource(nameof(RealmValueTestValues))]
        public Task RealmValue_Managed(TestCaseData<RealmValue> testData) => RunManagedTestsAsync(o => o.RealmValueDictionary, testData);

        [Test]
        public Task RealmValue_Notifications() => RunManagedNotificationsTestsAsync(o => o.RealmValueDictionary, RealmValueTestValues().Last());

        #endregion

        [Test]
        public void CanBeQueried()
        {
            var first = new DictionariesObject();
            first.StringDictionary.Add("a", "b");
            first.StringDictionary.Add("c", "d");

            var second = new DictionariesObject();
            second.StringDictionary.Add("ab", "cd");

            _realm.Write(() =>
            {
                _realm.Add(first);
                _realm.Add(second);
            });

            var equals = _realm.All<DictionariesObject>().Filter($"{nameof(StringDictionary)}.@keys == 'a'");
            Assert.That(equals, Is.EquivalentTo(new[] { first }));

            var valueContains = _realm.All<DictionariesObject>().Filter($"{nameof(StringDictionary)}.@values contains 'd'");
            Assert.That(valueContains, Is.EquivalentTo(new[] { first, second }));

            var startsWith = _realm.All<DictionariesObject>().Filter($"{nameof(StringDictionary)}.@keys beginswith 'f'");
            Assert.That(startsWith, Is.Empty);
        }

        [TestCase(".", "contains '.'")]
        [TestCase("$", "starts with '$'")]
        [TestCase("$foo", "starts with '$'")]
        [TestCase("foo.bar", "contains '.'")]
        [TestCase("foo.", "contains '.'")]
        public void InvalidKeys_ThrowNotSupportedException(string key, string expectedErrorFragment)
        {
            _realm.Write(() =>
            {
                var dict = _realm.Add(new DictionariesObject()).StringDictionary;
                Assert.That(() => dict[key] = "abc", Throws.TypeOf<NotSupportedException>().And.Message.Contains(expectedErrorFragment));
                Assert.That(() => dict.Add(key, "abc"), Throws.TypeOf<NotSupportedException>().And.Message.Contains(expectedErrorFragment));
                Assert.That(() => dict.Add(new KeyValuePair<string, string>(key, "abc")), Throws.TypeOf<NotSupportedException>().And.Message.Contains(expectedErrorFragment));

                var unmanaged = new DictionariesObject();
                unmanaged.StringDictionary[key] = "foo-bar";

                Assert.That(() => _realm.Add(unmanaged), Throws.TypeOf<NotSupportedException>().And.Message.Contains(expectedErrorFragment));
            });
        }

        [Test]
        public void NullKey_ThrowsArgumentNullException()
        {
            _realm.Write(() =>
            {
                var dict = _realm.Add(new DictionariesObject()).StringDictionary;
                Assert.That(() => dict[null] = "abc", Throws.TypeOf<ArgumentNullException>());
                Assert.That(() => dict.Add(null, "abc"), Throws.TypeOf<ArgumentNullException>());
                Assert.That(() => dict.Add(new KeyValuePair<string, string>(null, "abc")), Throws.TypeOf<ArgumentNullException>());
            });
        }

        [TestCase("a$")]
        [TestCase("a$$$$$$$$$")]
        [TestCase("_$_$_")]
        public void Key_MayContainDollarSign(string key)
        {
            _realm.Write(() =>
            {
                var dict = _realm.Add(new DictionariesObject()).StringDictionary;
                dict[key] = "abc";
                dict.Clear();

                dict.Add(key, "abc");
                dict.Clear();

                dict.Add(new KeyValuePair<string, string>(key, "abc"));
            });
        }

        [Test]
        public void ObjectFromAnotherRealm_ThrowsRealmException()
        {
            var realm2 = GetRealm(CreateConfiguration(Guid.NewGuid().ToString()));

            var item = new IntPropertyObject { Int = 5 };
            var embeddedItem = new EmbeddedIntPropertyObject { Int = 10 };

            var obj1 = _realm.Write(() =>
            {
                return _realm.Add(new DictionariesObject());
            });

            var obj2 = realm2.Write(() =>
            {
                return realm2.Add(new DictionariesObject());
            });

            _realm.Write(() =>
            {
                obj1.ObjectDictionary["foo"] = item;
                obj1.EmbeddedObjectDictionary["foo"] = embeddedItem;
            });

            Assert.That(() => realm2.Write(() => obj2.ObjectDictionary["foo"] = item), Throws.TypeOf<RealmException>().And.Message.Contains("object that is already in another realm"));
            Assert.That(() => realm2.Write(() => obj2.EmbeddedObjectDictionary["foo"] = embeddedItem), Throws.TypeOf<RealmException>().And.Message.Contains("embedded object that is already managed"));
        }

        private static async Task RunUnmanagedTestsAsync<T>(Func<DictionariesObject, IDictionary<string, T>> accessor, TestCaseData<T> testData)
        {
            var testObject = new DictionariesObject();
            var dictionary = accessor(testObject);

            testData.Seed(dictionary);

            await RunTestsCore(testData, dictionary);
        }

        private async Task RunManagedTestsAsync<T>(Func<DictionariesObject, IDictionary<string, T>> accessor, TestCaseData<T> testData)
        {
            var testObject = new DictionariesObject();
            var dictionary = accessor(testObject);

            testData.Seed(dictionary);

            _realm.Write(() =>
            {
                _realm.Add(testObject);
            });

            var managedDictionary = accessor(testObject);
            Assert.That(dictionary, Is.Not.SameAs(managedDictionary));

            await RunTestsCore(testData, managedDictionary);
            await testData.AssertThreadSafeReference(managedDictionary);

            testData.AssertNullKeys(managedDictionary);
        }

        private async Task RunManagedNotificationsTestsAsync<T>(Func<DictionariesObject, IDictionary<string, T>> accessor, TestCaseData<T> testData)
        {
            var testObject = new DictionariesObject();
            var dictionary = accessor(testObject);

            testData.Seed(dictionary);

            _realm.Write(() =>
            {
                _realm.Add(testObject);
            });

            var managedDictionary = accessor(testObject);
            Assert.That(dictionary, Is.Not.SameAs(managedDictionary));

            await testData.AssertNotifications_Realm(managedDictionary);
            await testData.AssertNotifications_CollectionChanged(managedDictionary);
            await testData.AssertKeyNotifications(managedDictionary);
        }

        private static async Task RunTestsCore<T>(TestCaseData<T> testData, IDictionary<string, T> dictionary)
        {
            testData.AssertCount(dictionary);
            testData.AssertContainsKey(dictionary);
            testData.AssertKeys(dictionary);
            testData.AssertValues(dictionary);
            testData.AssertIterator(dictionary);
            testData.AssertTryGetValue(dictionary);
            testData.AssertAccessor(dictionary);
            testData.AssertSet(dictionary);
            testData.AssertAdd(dictionary);
            testData.AssertRemove(dictionary);
            await testData.AssertFreeze(dictionary);
        }

        private static TestCaseData<RealmInteger<T>> ToInteger<T>(TestCaseData<T> data)
            where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
        {
            return new TestCaseData<RealmInteger<T>>(data.SampleValue, data.InitialValues.ToIntegerTuple());
        }

        private static TestCaseData<RealmInteger<T>?> ToInteger<T>(TestCaseData<T?> data)
            where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
        {
            return new TestCaseData<RealmInteger<T>?>(data.SampleValue, data.InitialValues.ToIntegerTuple());
        }

        public class TestCaseData<T>
        {
            private readonly Func<T, T> _cloneFunc;
            private readonly Func<T, T, bool> _equalityFunc;

            private T _sampleValue;

            public T SampleValue => _cloneFunc(_sampleValue);

            private (string Key, T Value)[] _initialValues;

            public (string Key, T Value)[] InitialValues => _initialValues.Select(kvp => (kvp.Key, _cloneFunc(kvp.Value))).ToArray();

            public TestCaseData(T sampleValue, params (string Key, T Value)[] initialValues)
                : this(x => x, null, sampleValue, initialValues)
            {
            }

            public TestCaseData(Func<T, T> cloneFunc, Func<T, T, bool> equalityFunc, T sampleValue, params (string Key, T Value)[] initialValues)
            {
                _cloneFunc = cloneFunc;
                _equalityFunc = equalityFunc;

                _sampleValue = sampleValue;

                _initialValues = initialValues.ToArray();
            }

            public override string ToString()
            {
                if (typeof(T) == typeof(byte[]))
                {
                    return string.Join(", ", _initialValues.Select(kvp => $"{kvp.Key}-{TestHelpers.ByteArrayToTestDescription(kvp.Value)}"));
                }

                return string.Join(", ", _initialValues.Select(kvp => $"{kvp.Key}-{kvp.Value}"));
            }

            public void AssertCount(IDictionary<string, T> target)
            {
                var reference = GetReferenceDictionary();

                Assert.That(target.Count, Is.EqualTo(reference.Count));
                Assert.That(target, IsEquivalentTo(reference));
            }

            public void AssertContainsKey(IDictionary<string, T> target)
            {
                foreach (var (key, _) in InitialValues)
                {
                    Assert.That(target.ContainsKey(key));
                }

                Assert.That(target.ContainsKey(Guid.NewGuid().ToString()), Is.False);
            }

            public void AssertAccessor(IDictionary<string, T> target)
            {
                foreach (var (key, value) in InitialValues)
                {
                    Assert.That(target[key], IsEqualTo(value));
                }

                T val;
                var missingKey = Guid.NewGuid().ToString();
                Assert.Throws<KeyNotFoundException>(() => val = target[missingKey], $"The given key '{missingKey}' was not present in the dictionary.");
            }

            public void AssertKeys(IDictionary<string, T> target)
            {
                Assert.That(target.Keys, Is.EquivalentTo(InitialValues.Select(x => x.Key)));
            }

            public void AssertValues(IDictionary<string, T> target)
            {
                Assert.That(target.Values, IsEquivalentTo(InitialValues.Select(x => x.Value)));
            }

            public void AssertTryGetValue(IDictionary<string, T> target)
            {
                foreach (var (key, value) in InitialValues)
                {
                    var hasKey = target.TryGetValue(key, out var storedValue);
                    Assert.That(hasKey, Is.True);
                    Assert.That(storedValue, IsEqualTo(value));
                }
            }

            public void AssertAdd(IDictionary<string, T> target)
            {
                var expectedCount = target.Count;

                var key1 = Guid.NewGuid().ToString();
                var value1 = SampleValue;

                WriteIfNecessary(target, () =>
                {
                    target.Add(key1, value1);
                });

                expectedCount++;

                Assert.That(target.Count, Is.EqualTo(expectedCount));
                Assert.That(target[key1], IsEqualTo(value1));

                var key2 = Guid.NewGuid().ToString();
                var value2 = _cloneFunc(target.First().Value);
                WriteIfNecessary(target, () =>
                {
                    target.Add(key2, value2);
                });

                expectedCount++;

                Assert.That(target.Count, Is.EqualTo(expectedCount));
                Assert.That(target[key2], IsEqualTo(value2));

                Assert.Throws<ArgumentException>(() =>
                {
                    WriteIfNecessary(target, () =>
                    {
                        target.Add(key2, SampleValue);
                    });
                }, target is RealmDictionary<T> ? $"An item with the key '{key2}' has already been added." : "An item with the same key has already been added.");
            }

            public void AssertRemove(IDictionary<string, T> target)
            {
                Seed(target);

                var expectedCount = target.Count;

                if (target.Any())
                {
                    if (TryGetDifferentValue(target, SampleValue, out var result))
                    {
                        // Removing a KVP with existing key but the wrong value should return false
                        var didRemoveWrongValue = WriteIfNecessary(target, () =>
                        {
                            return target.Remove(new KeyValuePair<string, T>(result.Key, SampleValue));
                        });

                        Assert.That(didRemoveWrongValue, Is.False);
                        Assert.That(target.ContainsKey(result.Key), Is.True);
                        Assert.That(target.Count, Is.EqualTo(expectedCount));
                    }

                    var kvp = target.Last();
                    var didRemoveExisting = WriteIfNecessary(target, () =>
                    {
                        return target.Remove(new KeyValuePair<string, T>(kvp.Key, kvp.Value));
                    });

                    expectedCount--;

                    Assert.That(didRemoveExisting, Is.True);
                    Assert.That(target.ContainsKey(kvp.Key), Is.False);
                    Assert.That(target.Count, Is.EqualTo(expectedCount));
                }

                if (target.Any())
                {
                    var key = target.First().Key;
                    var didRemoveExisting = WriteIfNecessary(target, () =>
                    {
                        return target.Remove(key);
                    });

                    expectedCount--;

                    Assert.That(didRemoveExisting, Is.True);
                    Assert.That(target.ContainsKey(key), Is.False);
                    Assert.That(target.Count, Is.EqualTo(expectedCount));
                }

                var newKey = Guid.NewGuid().ToString();
                var didRemoveNonExisting = WriteIfNecessary(target, () =>
                {
                    return target.Remove(newKey);
                });

                Assert.That(didRemoveNonExisting, Is.False);
                Assert.That(target.ContainsKey(newKey), Is.False);
                Assert.That(target.Count, Is.EqualTo(expectedCount));

                didRemoveNonExisting = WriteIfNecessary(target, () =>
                {
                    return target.Remove(new KeyValuePair<string, T>(newKey, SampleValue));
                });

                Assert.That(didRemoveNonExisting, Is.False);
                Assert.That(target.ContainsKey(newKey), Is.False);
                Assert.That(target.Count, Is.EqualTo(expectedCount));
            }

            public void AssertSet(IDictionary<string, T> target)
            {
                var expectedCount = target.Count;

                if (target.Any())
                {
                    var key = target.First().Key;
                    WriteIfNecessary(target, () =>
                    {
                        target[key] = SampleValue;
                    });

                    Assert.That(target.ContainsKey(key));
                    Assert.That(target[key], IsEqualTo(SampleValue));
                    Assert.That(target.Count, Is.EqualTo(expectedCount));
                }

                var newKey = Guid.NewGuid().ToString();
                WriteIfNecessary(target, () =>
                {
                    target[newKey] = SampleValue;
                });

                expectedCount++;

                Assert.That(target.ContainsKey(newKey));
                Assert.That(target[newKey], IsEqualTo(SampleValue));
                Assert.That(target.Count, Is.EqualTo(expectedCount));
            }

            public void AssertIterator(IDictionary<string, T> target)
            {
                var referenceDict = GetReferenceDictionary();

                foreach (var kvp in target)
                {
                    Assert.That(referenceDict.ContainsKey(kvp.Key));
                    Assert.That(referenceDict[kvp.Key], IsEqualTo(kvp.Value));

                    referenceDict.Remove(kvp.Key);
                }

                Assert.That(referenceDict, Is.Empty);
            }

            public async Task AssertKeyNotifications(IDictionary<string, T> target)
            {
                Assert.That(target, Is.TypeOf<RealmDictionary<T>>());

                Seed(target);

                target.AsRealmCollection().Realm.Refresh();

                var callbacks = new List<DictionaryChangeSet>();
                using var token = target.SubscribeForKeyNotifications((collection, changes, error) =>
                {
                    Assert.That(error, Is.Null);

                    if (changes != null)
                    {
                        callbacks.Add(changes);
                    }
                });

                var newKey = Guid.NewGuid().ToString();
                WriteIfNecessary(target, () =>
                {
                    target.Add(newKey, SampleValue);
                });

                var changes = await EnsureRefreshed(callbacks, 1);

                Assert.That(changes.DeletedKeys.Length, Is.EqualTo(0));
                Assert.That(changes.InsertedKeys.Length, Is.EqualTo(1));
                Assert.That(changes.ModifiedKeys.Length, Is.EqualTo(0));

                Assert.That(changes.InsertedKeys.Single(), Is.EqualTo(newKey));
                Assert.That(target[changes.InsertedKeys.Single()], IsEqualTo(SampleValue));

                if (!TryGetDifferentValue(target, SampleValue, out var result))
                {
                    Assert.Fail("Couldn't find a unique value to replace - fix the test!");
                }

                var keyToUpdate = result.Key;
                WriteIfNecessary(target, () =>
                {
                    target[keyToUpdate] = SampleValue;
                });

                changes = await EnsureRefreshed(callbacks, 2);

                Assert.That(changes.DeletedKeys.Length, Is.EqualTo(0));
                Assert.That(changes.InsertedKeys.Length, Is.EqualTo(0));
                Assert.That(changes.ModifiedKeys.Length, Is.EqualTo(1));

                Assert.That(changes.ModifiedKeys.Single(), Is.EqualTo(keyToUpdate));
                Assert.That(target[changes.ModifiedKeys.Single()], IsEqualTo(SampleValue));

                var keyToDelete = result.Key;
                WriteIfNecessary(target, () =>
                {
                    target.Remove(keyToDelete);
                });

                changes = await EnsureRefreshed(callbacks, 3);

                Assert.That(changes.DeletedKeys.Length, Is.EqualTo(1));
                Assert.That(changes.InsertedKeys.Length, Is.EqualTo(0));
                Assert.That(changes.ModifiedKeys.Length, Is.EqualTo(0));

                Assert.That(changes.DeletedKeys.Single(), Is.EqualTo(keyToDelete));
                Assert.That(target.ContainsKey(keyToDelete), Is.False);

                // Verify we stop receiving notifications after we dispose of the token
                token.Dispose();

                WriteIfNecessary(target, () =>
                {
                    target.Add(Guid.NewGuid().ToString(), SampleValue);
                });

                target.AsRealmCollection().Realm.Refresh();

                Assert.That(callbacks.Count, Is.EqualTo(3));
            }

            public async Task AssertNotifications_Realm(IDictionary<string, T> target)
            {
                Assert.That(target, Is.TypeOf<RealmDictionary<T>>());

                Seed(target);
                target.AsRealmCollection().Realm.Refresh();

                var callbacks = new List<ChangeSet>();
                using var token = target.SubscribeForNotifications((collection, changes, error) =>
                {
                    Assert.That(error, Is.Null);

                    if (changes != null)
                    {
                        callbacks.Add(changes);
                    }
                });

                await AssertNotificationsCore(target, callbacks, changes =>
                {
                    Assert.That(changes.InsertedIndices.Length, Is.EqualTo(1));
                    Assert.That(changes.ModifiedIndices, Is.Empty);
                    Assert.That(changes.NewModifiedIndices, Is.Empty);
                    Assert.That(changes.DeletedIndices, Is.Empty);
                    Assert.That(changes.Moves, Is.Empty);

                    return changes.InsertedIndices[0];
                }, changes =>
                {
                    Assert.That(changes.InsertedIndices, Is.Empty);
                    Assert.That(changes.ModifiedIndices, Is.Empty);
                    Assert.That(changes.NewModifiedIndices, Is.Empty);
                    Assert.That(changes.DeletedIndices.Length, Is.EqualTo(1));
                    Assert.That(changes.Moves, Is.Empty);

                    return changes.DeletedIndices[0];
                }, changes =>
                {
                    Assert.That(changes.ModifiedIndices.Length, Is.EqualTo(1));
                    Assert.That(changes.NewModifiedIndices.Length, Is.EqualTo(1));
                    Assert.That(changes.InsertedIndices, Is.Empty);
                    Assert.That(changes.DeletedIndices, Is.Empty);
                    Assert.That(changes.Moves, Is.Empty);

                    return (changes.ModifiedIndices[0], changes.NewModifiedIndices[0]);
                });
            }

            public async Task AssertNotifications_CollectionChanged(IDictionary<string, T> target)
            {
                Assert.That(target, Is.TypeOf<RealmDictionary<T>>());

                Seed(target);

                target.AsRealmCollection().Realm.Refresh();

                var callbacks = new List<NotifyCollectionChangedEventArgs>();
                target.AsRealmCollection().CollectionChanged += HandleCollectionChanged;

                // CollectionChangedEventArgs don't communicate object modifications.
                await AssertNotificationsCore(target, callbacks, changes =>
                {
                    Assert.That(changes.Action, Is.EqualTo(NotifyCollectionChangedAction.Add));
                    Assert.That(changes.NewItems.Count, Is.EqualTo(1));

                    return changes.NewStartingIndex;
                }, changes =>
                {
                    Assert.That(changes.Action, Is.EqualTo(NotifyCollectionChangedAction.Remove));
                    Assert.That(changes.OldItems.Count, Is.EqualTo(1));

                    return changes.OldStartingIndex;
                }, assertModification: null);

                target.AsRealmCollection().CollectionChanged -= HandleCollectionChanged;

                void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
                {
                    Assert.That(sender, Is.EqualTo(target));

                    callbacks.Add(e);
                }
            }

            private async Task AssertNotificationsCore<TArgs>(
                IDictionary<string, T> target,
                List<TArgs> callbacks,
                Func<TArgs, int> assertInsertion,
                Func<TArgs, int> assertDeletion,
                Func<TArgs, (int OldIndex, int NewIndex)> assertModification)
            {
                var newKey = Guid.NewGuid().ToString();
                WriteIfNecessary(target, () =>
                {
                    target.Add(newKey, SampleValue);
                });

                var changes = await EnsureRefreshed(callbacks, 1);
                var insertedIndex = assertInsertion(changes);

                Assert.That(target.ElementAt(insertedIndex).Key, Is.EqualTo(newKey));
                Assert.That(target.ElementAt(insertedIndex).Value, IsEqualTo(SampleValue));

                Assert.That(target.AsRealmCollection()[insertedIndex].Key, Is.EqualTo(newKey));
                Assert.That(target.AsRealmCollection()[insertedIndex].Value, IsEqualTo(SampleValue));

                WriteIfNecessary(target, () =>
                {
                    target.Remove(newKey);
                });

                changes = await EnsureRefreshed(callbacks, 2);

                var deletedIndex = assertDeletion(changes);
                Assert.That(deletedIndex, Is.EqualTo(insertedIndex));

                if (assertModification == null)
                {
                    // INotifyCollectionChanged doesn't communicate object modifications, so let's stop the test
                    // if the caller didn't provide modification assertion callback.
                    return;
                }

                if (!TryGetDifferentValue(target, SampleValue, out var result))
                {
                    Assert.Fail("Couldn't find a unique value to replace - fix the test!");
                }

                var keyToUpdate = result.Key;
                WriteIfNecessary(target, () =>
                {
                    target[keyToUpdate] = SampleValue;
                });

                changes = await EnsureRefreshed(callbacks, 3);

                var (oldIndex, newIndex) = assertModification(changes);

                Assert.That(oldIndex, Is.EqualTo(result.Index));
                Assert.That(target.ElementAt(newIndex).Key, Is.EqualTo(keyToUpdate));
                Assert.That(target.ElementAt(newIndex).Value, IsEqualTo(SampleValue));

                Assert.That(target.AsRealmCollection()[newIndex].Key, Is.EqualTo(keyToUpdate));
                Assert.That(target.AsRealmCollection()[newIndex].Value, IsEqualTo(SampleValue));
            }

            public async Task AssertFreeze(IDictionary<string, T> target)
            {
                Seed(target);

                if (target is RealmDictionary<T>)
                {
                    var frozenDict = target.Freeze();
                    var referenceDict = GetReferenceDictionary();

                    Assert.That(frozenDict, IsEquivalentTo(referenceDict));
                    Assert.That(frozenDict, Is.TypeOf<RealmDictionary<T>>());

                    var frozenCollection = frozenDict.AsRealmCollection();
                    Assert.That(frozenCollection.IsValid);
                    Assert.That(frozenCollection.IsFrozen);

                    Assert.Throws<RealmFrozenException>(() =>
                    {
                        WriteIfNecessary(frozenDict, () =>
                        {
                            frozenDict.Add(Guid.NewGuid().ToString(), SampleValue);
                        });
                    });

                    await Task.Run(() =>
                    {
                        // Ensure the frozen collection can be passed between threads
                        Assert.That(frozenDict, IsEquivalentTo(referenceDict));
                    });

                    var newKey = Guid.NewGuid().ToString();
                    WriteIfNecessary(target, () =>
                    {
                        target.Add(newKey, SampleValue);
                    });

                    Assert.That(frozenDict.ContainsKey(newKey), Is.False);
                    Assert.That(frozenDict.Count, Is.EqualTo(target.Count - 1));

                    WriteIfNecessary(target, () =>
                    {
                        target.Remove(newKey);
                    });

                    frozenDict.AsRealmCollection().Realm.Dispose();
                }
                else
                {
                    // We can't freeze unmanaged dictionaries.
                    Assert.Throws<RealmException>(() => target.Freeze());
                }
            }

            public async Task AssertThreadSafeReference(IDictionary<string, T> target)
            {
                Assert.That(target, Is.TypeOf<RealmDictionary<T>>());

                Seed(target);

                var tsr = ThreadSafeReference.Create(target);
                var originalThreadId = Environment.CurrentManagedThreadId;

                await Task.Run(() =>
                {
                    Assert.That(Environment.CurrentManagedThreadId, Is.Not.EqualTo(originalThreadId));

                    using var bgRealm = Realm.GetInstance(target.AsRealmCollection().Realm.Config);
                    var bgDict = bgRealm.ResolveReference(tsr);

                    Assert.That(bgDict, IsEquivalentTo(GetReferenceDictionary()));
                });
            }

            public void AssertNullKeys(IDictionary<string, T> target)
            {
                Assert.That(target, Is.TypeOf<RealmDictionary<T>>());

                Assert.Throws<KeyNotFoundException>(() => _ = target[null], "The given key 'null' was not present in the dictionary.");
                Assert.Throws<ArgumentNullException>(() => target[null] = SampleValue, "A persisted dictionary cannot store null keys.");
                Assert.Throws<ArgumentNullException>(() => target.Add(null, SampleValue), "A persisted dictionary cannot store null keys.");
                Assert.Throws<ArgumentNullException>(() => target.Add(new KeyValuePair<string, T>(null, SampleValue)), "A persisted dictionary cannot store null keys.");

                Assert.That(target.ContainsKey(null), Is.False);
                Assert.That(target.Remove(null), Is.False);
                Assert.That(target.Remove(new KeyValuePair<string, T>(null, SampleValue)), Is.False);

                var hasKey = target.TryGetValue(null, out var foundValue);
                Assert.That(hasKey, Is.False);
                Assert.That(foundValue, Is.EqualTo(default(T)));
            }

            public void Seed(IDictionary<string, T> target, IEnumerable<(string Key, T Value)> values = null)
            {
                WriteIfNecessary(target, () =>
                {
                    target.Clear();
                    foreach (var (key, value) in values ?? InitialValues)
                    {
                        target.Add(key, value);
                    }
                });
            }

            private EqualConstraint IsEqualTo(T other)
            {
                if (_equalityFunc == null)
                {
                    return Is.EqualTo(other);
                }

                return Is.EqualTo(other).Using((Comparison<T>)comparison);

                int comparison(T a, T b) => _equalityFunc(a, b) ? 0 : 1;
            }

            private CollectionItemsEqualConstraint IsEquivalentTo(IDictionary<string, T> dict)
            {
                if (_equalityFunc == null)
                {
                    return Is.EquivalentTo(dict);
                }

                var comparer = new Func<KeyValuePair<string, T>, KeyValuePair<string, T>, bool>((first, second) => first.Key == second.Key && _equalityFunc(first.Value, second.Value));

                return Is.EquivalentTo(dict).Using(comparer);
            }

            private CollectionItemsEqualConstraint IsEquivalentTo(IEnumerable<T> other)
            {
                if (_equalityFunc == null)
                {
                    return Is.EquivalentTo(other);
                }

                return Is.EquivalentTo(other).Using(_equalityFunc);
            }

            private static void WriteIfNecessary(IDictionary<string, T> collection, Action writeAction)
            {
                Transaction transaction = null;
                try
                {
                    if (collection is RealmDictionary<T> realmDictionary)
                    {
                        transaction = realmDictionary.Realm.BeginWrite();
                    }

                    writeAction();

                    transaction?.Commit();
                }
                catch
                {
                    transaction?.Rollback();
                    throw;
                }
            }

            private static TResult WriteIfNecessary<TResult>(IDictionary<string, T> collection, Func<TResult> writeFunc)
            {
                Transaction transaction = null;

                try
                {
                    if (collection is RealmDictionary<T> realmDictionary)
                    {
                        transaction = realmDictionary.Realm.BeginWrite();
                    }

                    var result = writeFunc();

                    transaction?.Commit();

                    return result;
                }
                catch
                {
                    transaction?.Rollback();
                    throw;
                }
            }

            private static bool AreValuesEqual(T first, T second)
            {
                if (Equals(first, second))
                {
                    return true;
                }

                if (first is byte[] firstArr && second is byte[] secondArr)
                {
                    return Enumerable.SequenceEqual(firstArr, secondArr);
                }

                return false;
            }

            private static bool TryGetDifferentValue(IDictionary<string, T> collection, T valueToCompare, out (int Index, string Key, T Value) result)
            {
                var index = 0;
                foreach (var kvp in collection)
                {
                    if (!AreValuesEqual(kvp.Value, valueToCompare))
                    {
                        result = (index, kvp.Key, kvp.Value);
                        return true;
                    }

                    index++;
                }

                result = (-1, null, default(T));
                return false;
            }

            private static async Task<TArgs> EnsureRefreshed<TArgs>(IList<TArgs> callbacks, int expectedCallbackCount)
            {
                await TestHelpers.WaitForConditionAsync(() => callbacks.Count == expectedCallbackCount);

                Assert.That(callbacks.Count, Is.EqualTo(expectedCallbackCount));

                return callbacks[expectedCallbackCount - 1];
            }

            public IDictionary<string, T> GetReferenceDictionary() => InitialValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
