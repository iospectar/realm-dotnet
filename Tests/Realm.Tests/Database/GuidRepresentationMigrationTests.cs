////////////////////////////////////////////////////////////////////////////
//
// Copyright 2022 Realm Inc.
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
using System.Linq;
using System.Runtime.InteropServices;
using MongoDB.Bson;
using NUnit.Framework;
using Realms.Tests.Sync;

namespace Realms.Tests.Database
{
    [TestFixture, Preserve(AllMembers = true)]
    public class GuidRepresentationMigrationTests : SyncTestBase
    {
        private RealmConfiguration _configuration;

        [DllImport(InteropConfig.DLL_NAME, EntryPoint = "_realm_flip_guid_for_testing", CallingConvention = CallingConvention.Cdecl)]
        private static extern void flip_guid_for_testing([In, Out] byte[] guid_bytes);

        [Test]
        public void Migration_FlipGuid_ShouldProduceCorrectRepresentation()
        {
            var guid = Guid.NewGuid();
            var expected = GuidConverter.ToBytes(guid, GuidRepresentation.Standard);
            var actual = guid.ToByteArray();

            NativeCommon.Initialize(); // ensure we can find the wrappers binary
            flip_guid_for_testing(actual);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Migration_FromLittleEndianGuidFile([Values(true, false)] bool useLegacyRepresentation)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Realm.UseLegacyGuidRepresentation = useLegacyRepresentation;
#pragma warning restore CS0618 // Type or member is obsolete

            TestHelpers.CopyBundledFileToDocuments("guids.realm", _configuration.DatabasePath);

            var expected = GetGuidObjects().ToArray();
            using var realm = GetRealm(_configuration);

            var actual = realm.All<GuidType>().ToArray();

            Assert.That(actual.Length, Is.EqualTo(expected.Length));

            foreach (var expectedObj in expected)
            {
                var actualObj = actual.Single(o => o.Id == expectedObj.Id);

                AssertEqual(expectedObj, actualObj);

                var actualFound = realm.Find<GuidType>(expectedObj.Id);
                Assert.That(actualObj, Is.EqualTo(actualFound));
            }
        }

        [Test]
        public void PopulatingANewFile([Values(true, false)] bool useLegacyRepresentation)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Realm.UseLegacyGuidRepresentation = useLegacyRepresentation;
#pragma warning restore CS0618 // Type or member is obsolete

            var expected = GetGuidObjects().ToArray();
            using var realm = GetRealm(_configuration);

            realm.Write(() =>
            {
                realm.Add(GetGuidObjects());
            });

            var actual = realm.All<GuidType>().ToArray();

            Assert.That(actual.Length, Is.EqualTo(expected.Length));

            foreach (var expectedObj in expected)
            {
                var actualObj = actual.Single(o => o.Id == expectedObj.Id);

                AssertEqual(expectedObj, actualObj);

                var actualFound = realm.Find<GuidType>(expectedObj.Id);
                Assert.That(actualObj, Is.EqualTo(actualFound));
            }
        }

        [Test]
        public void GuidRepresentationMatchesQuery([Values(true, false)] bool useLegacyRepresentation)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Realm.UseLegacyGuidRepresentation = useLegacyRepresentation;
#pragma warning restore CS0618 // Type or member is obsolete

            using var realm = GetRealm(_configuration);

            var guidString = "43f327c6-686d-4066-b7a6-f8b05be7057f";
            var guid = Guid.Parse(guidString);
            var query = (RealmResults<GuidType>)realm.All<GuidType>().Where(t => t.RegularProperty == guid);
            var description = query.ResultsHandle.Description;

            AssertQueryDescription(description, guidString, useLegacyRepresentation);
        }

        [Test]
        public void FlexibleSync_Subscriptions_MatchesGuid([Values(true, false)] bool useLegacyRepresentation)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            Realm.UseLegacyGuidRepresentation = useLegacyRepresentation;
#pragma warning restore CS0618 // Type or member is obsolete

            var config = GetFakeFLXConfig();
            config.Schema = new[] { typeof(GuidType), typeof(EmbeddedGuidType) };
            using var realm = GetRealm(config);

            var guidString = "981b8fa2-c496-43b0-b401-48ce08b38e00";
            var guid = Guid.Parse(guidString);

            realm.Subscriptions.Update(() =>
            {
                var query = (RealmResults<GuidType>)realm.All<GuidType>().Where(t => t.RegularProperty == guid);
                realm.Subscriptions.Add(query);
            });

            var description = realm.Subscriptions.Single().Query;

            AssertQueryDescription(description, guidString, useLegacyRepresentation);
        }

        protected override void CustomSetUp()
        {
            _configuration = new RealmConfiguration(Guid.NewGuid().ToString())
            {
                Schema = new[] { typeof(GuidType), typeof(EmbeddedGuidType) }
            };
        }

        private static void AssertQueryDescription(string query, string guidString, bool useLegacyRepresentation)
        {
            if (useLegacyRepresentation)
            {
                // Flip the byte order by converting to byte array using little endian but read it as big endian
                guidString = GuidConverter.FromBytes(Guid.Parse(guidString).ToByteArray(), GuidRepresentation.Standard).ToString();
            }

            Assert.That(query, Does.Contain(guidString));
        }

        private static void AssertEqual(GuidType expected, GuidType actual)
        {
            Assert.That(actual.Id, Is.EqualTo(expected.Id));

            Assert.That(actual.RegularProperty, Is.EqualTo(expected.RegularProperty));
            CollectionAssert.AreEqual(actual.GuidList, expected.GuidList);
            CollectionAssert.AreEquivalent(actual.GuidSet, expected.GuidSet);
            CollectionAssert.AreEquivalent(actual.GuidDict, expected.GuidDict);

            Assert.That(actual.OptionalProperty, Is.EqualTo(expected.OptionalProperty));
            CollectionAssert.AreEqual(actual.OptionalList, expected.OptionalList);
            CollectionAssert.AreEquivalent(actual.OptionalSet, expected.OptionalSet);
            CollectionAssert.AreEquivalent(actual.OptionalDict, expected.OptionalDict);

            Assert.That(actual.MixedProperty, Is.EqualTo(expected.MixedProperty));
            CollectionAssert.AreEqual(actual.MixedList, expected.MixedList);
            CollectionAssert.AreEquivalent(actual.MixedSet, expected.MixedSet);
            CollectionAssert.AreEquivalent(actual.MixedDict, expected.MixedDict);

            Assert.That(actual.LinkProperty?.Id, Is.EqualTo(expected.LinkProperty?.Id));

            var actualEmbedded = actual.EmbeddedProperty;
            var expectedEmbedded = expected.EmbeddedProperty;

            if (actualEmbedded == null)
            {
                Assert.That(expectedEmbedded, Is.Null);
            }
            else
            {
                Assert.That(actualEmbedded.RegularProperty, Is.EqualTo(expectedEmbedded.RegularProperty));
                CollectionAssert.AreEqual(actualEmbedded.GuidList, expectedEmbedded.GuidList);
                CollectionAssert.AreEquivalent(actualEmbedded.GuidSet, expectedEmbedded.GuidSet);
                CollectionAssert.AreEquivalent(actualEmbedded.GuidDict, expectedEmbedded.GuidDict);

                Assert.That(actualEmbedded.OptionalProperty, Is.EqualTo(expectedEmbedded.OptionalProperty));
                CollectionAssert.AreEqual(actualEmbedded.OptionalList, expectedEmbedded.OptionalList);
                CollectionAssert.AreEquivalent(actualEmbedded.OptionalSet, expectedEmbedded.OptionalSet);
                CollectionAssert.AreEquivalent(actualEmbedded.OptionalDict, expectedEmbedded.OptionalDict);

                Assert.That(actualEmbedded.MixedProperty, Is.EqualTo(expectedEmbedded.MixedProperty));
                CollectionAssert.AreEqual(actualEmbedded.MixedList, expectedEmbedded.MixedList);
                CollectionAssert.AreEquivalent(actualEmbedded.MixedSet, expectedEmbedded.MixedSet);
                CollectionAssert.AreEquivalent(actualEmbedded.MixedDict, expectedEmbedded.MixedDict);

                Assert.That(actualEmbedded.LinkProperty?.Id, Is.EqualTo(expectedEmbedded.LinkProperty?.Id));
            }
        }

        private static IEnumerable<GuidType> GetGuidObjects()
        {
            var embedded1 = new EmbeddedGuidType
            {
                RegularProperty = Guid.Parse("64073484-5c31-4dcc-8276-282a448f760b"),
                GuidList =
                {
                    Guid.Parse("f3c7428c-6fcb-4a25-a4e0-9eef5d695b6d"),
                    Guid.Parse("78e97bb2-e5fa-4256-856f-e6e304e79f3a"),
                },
                GuidSet =
                {
                    Guid.Parse("5ba8da18-4e97-4aa3-aabc-314d34616f1f"),
                    Guid.Parse("76c5ff1c-2f75-40c3-af22-e9f35d901c36"),
                },
                GuidDict =
                {
                    { "a", Guid.Parse("0771ea3c-19ec-479c-b109-db258266ffc2") },
                    { "b", Guid.Parse("4ad92d5b-27a8-46d2-aed9-e0b14f25a19e") }
                },
                OptionalProperty = Guid.Parse("2d45c27e-440c-4713-9d2a-e5756487a293"),
                OptionalList =
                {
                    Guid.Parse("b9199da2-4d7e-4c67-9ab9-ecdc9a7e0380"),
                    null,
                },
                OptionalSet =
                {
                    Guid.Parse("6b544e76-537c-43ee-9664-a3bc4e842632"),
                    null
                },
                OptionalDict =
                {
                    { "c", Guid.Parse("a0a7eabc-8877-443d-98ce-a4f29a16379d") },
                    { "d", null }
                },
                MixedProperty = Guid.Parse("de5a5cbc-a41e-472a-9a73-a4f8d98ece82"),
                MixedSet =
                {
                    Guid.Parse("2bed1e3d-56c4-453b-acfd-013827caa8b0"),
                    Guid.Parse("5eb30478-7dde-41e2-bad9-10d4ba713ed0").ToString(),
                    1.23456
                },
                MixedList =
                {
                    Guid.Parse("6362a871-d61a-4882-bb44-dc79b0456794"),
                    "abc",
                    999
                },
                MixedDict =
                {
                    { "a", Guid.Parse("03ddf81b-6ff6-4b74-8172-421b3cf1c638") },
                    { "b", new DateTimeOffset(637798379974738110, offset: TimeSpan.Zero) }
                },
            };

            var first = new GuidType
            {
                Id = Guid.Parse("f8f37f1f-26c5-415e-b45c-12dbdc8478c8"),
                RegularProperty = Guid.Parse("527bf8dc-5452-493b-a091-f098d986f120"),
                GuidList =
                {
                    Guid.Parse("b897f2dc-e5aa-4328-8789-77a40e4b7bcf"),
                    Guid.Parse("d9d02d70-5d89-45cb-8ce8-04f54fb7d9fc"),
                },
                GuidSet =
                {
                    Guid.Parse("00000000-0000-0000-0000-000000000000"),
                    Guid.Parse("64073484-5c31-4dcc-8276-282a448f760b"),
                },
                GuidDict =
                {
                    { "a", Guid.Parse("f3c7428c-6fcb-4a25-a4e0-9eef5d695b6d") },
                    { "b", Guid.Parse("78e97bb2-e5fa-4256-856f-e6e304e79f3a") }
                },
                OptionalProperty = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                OptionalList =
                {
                    Guid.Parse("09367b97-4ecf-4391-ae4a-44d85f8dc27f"),
                    null,
                },
                OptionalSet =
                {
                    Guid.Parse("e89d691a-fed9-4a6f-9028-566c06dee2fc"),
                    null
                },
                OptionalDict =
                {
                    { "c", Guid.Parse("6700d95f-5e49-4331-bae0-03b9b0994549") },
                    { "d", null }
                },
                MixedProperty = Guid.Parse("5ba8da18-4e97-4aa3-aabc-314d34616f1f"),
                MixedSet =
                {
                    Guid.Parse("76c5ff1c-2f75-40c3-af22-e9f35d901c36"),
                    Guid.Parse("0771ea3c-19ec-479c-b109-db258266ffc2").ToString(),
                    1.23456
                },
                MixedList =
                {
                    Guid.Parse("4ad92d5b-27a8-46d2-aed9-e0b14f25a19e"),
                    "abc",
                    999
                },
                MixedDict =
                {
                    { "a", Guid.Parse("de5a5cbc-a41e-472a-9a73-a4f8d98ece82") },
                    { "b", new DateTimeOffset(637798379974738110, offset: TimeSpan.Zero) }
                },
                EmbeddedProperty = embedded1
            };

            yield return first;

            var embedded2 = new EmbeddedGuidType
            {
                RegularProperty = Guid.Parse("2bed1e3d-56c4-453b-acfd-013827caa8b0"),
                GuidList =
                {
                    Guid.Parse("5eb30478-7dde-41e2-bad9-10d4ba713ed0"),
                    Guid.Parse("6362a871-d61a-4882-bb44-dc79b0456794"),
                },
                GuidSet =
                {
                    Guid.Parse("03ddf81b-6ff6-4b74-8172-421b3cf1c638"),
                    Guid.Parse("f8f37f1f-26c5-415e-b45c-12dbdc8478c8"),
                },
                GuidDict =
                {
                    { "a", Guid.Parse("527bf8dc-5452-493b-a091-f098d986f120") },
                    { "b", Guid.Parse("b897f2dc-e5aa-4328-8789-77a40e4b7bcf") }
                },
                OptionalProperty = null,
                OptionalList =
                {
                    null,
                },
                OptionalSet =
                {
                    null
                },
                OptionalDict =
                {
                    { "d", null }
                },
                MixedProperty = Guid.Parse("162a5dd1-48b5-47b3-8b6a-73ebdb9cce69"),
                MixedSet =
                {
                    Guid.Parse("ee3ee889-78e3-40f3-89c7-02b43f571b42"),
                    Guid.Parse("6f043170-fb5c-4a5c-bd06-72b4477a58a0").ToString(),
                    1.23456
                },
                MixedList =
                {
                    Guid.Parse("54d27013-7717-4bbf-9f68-17e05a4cf4d5"),
                    "abc",
                    999
                },
                MixedDict =
                {
                    { "a", Guid.Parse("8a453631-dd37-4881-a292-5e83198b1bb5") },
                    { "b", new DateTimeOffset(2022, 2, 7, 13, 39, 57, offset: TimeSpan.Zero) }
                },
                LinkProperty = first,
            };

            yield return new GuidType
            {
                Id = Guid.Parse("60325508-b005-46ae-8223-b9fae925b9d3"),
                LinkProperty = first,
                EmbeddedProperty = embedded2,
                OptionalProperty = null,
                OptionalList =
                {
                    null,
                },
                OptionalSet =
                {
                    null
                },
                OptionalDict =
                {
                    { "d", null }
                },
            };
        }

        [Explicit]
        public class GuidType : RealmObject
        {
            [PrimaryKey, MapTo("_id")]
            public Guid Id { get; set; }

            public Guid RegularProperty { get; set; }

            public IList<Guid> GuidList { get; }

            public ISet<Guid> GuidSet { get; }

            public IDictionary<string, Guid> GuidDict { get; }

            public Guid? OptionalProperty { get; set; }

            public IList<Guid?> OptionalList { get; }

            public ISet<Guid?> OptionalSet { get; }

            public IDictionary<string, Guid?> OptionalDict { get; }

            public GuidType LinkProperty { get; set; }

            public RealmValue MixedProperty { get; set; }

            public IList<RealmValue> MixedList { get; }

            public ISet<RealmValue> MixedSet { get; }

            public IDictionary<string, RealmValue> MixedDict { get; }

            public EmbeddedGuidType EmbeddedProperty { get; set; }
        }

        [Explicit]
        public class EmbeddedGuidType : EmbeddedObject
        {
            public Guid RegularProperty { get; set; }

            public IList<Guid> GuidList { get; }

            public ISet<Guid> GuidSet { get; }

            public IDictionary<string, Guid> GuidDict { get; }

            public Guid? OptionalProperty { get; set; }

            public IList<Guid?> OptionalList { get; }

            public ISet<Guid?> OptionalSet { get; }

            public IDictionary<string, Guid?> OptionalDict { get; }

            public GuidType LinkProperty { get; set; }

            public RealmValue MixedProperty { get; set; }

            public IList<RealmValue> MixedList { get; }

            public ISet<RealmValue> MixedSet { get; }

            public IDictionary<string, RealmValue> MixedDict { get; }
        }
    }
}
