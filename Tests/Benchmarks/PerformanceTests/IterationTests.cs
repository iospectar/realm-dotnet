﻿////////////////////////////////////////////////////////////////////////////
//
// Copyright 2022 Realm Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License")
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

using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Realms;

namespace PerformanceTests
{
    [MemoryDiagnoser]
    public class IterationTests : BenchmarkBase
    {
        [Params(100, 1000, 10_000)]
        public int ObjectCount { get; set; }

        protected override void SeedData()
        {
            base.SeedData();

            _realm.Write(() =>
            {
                for (var i = 0; i < ObjectCount; i++)
                {
                    _realm.Add(new ObjectWithList
                    {
                        Value = i,
                    });
                }
            });

            _realm.Dispose();
            _realm = Realm.GetInstance(_realm.Config.DatabasePath);
        }

        [Benchmark(Description = "Time to iterate a collection of %ObjectCount% objects")]
        public void SimpleIteration()
        {
            foreach (var value in _realm.All<ObjectWithList>())
            {
                _ = value;
            }
        }

        [Benchmark(Description = "Time to iterate a collection of %ObjectCount% objects and then dispose of the parent realm")]
        public void Disposal()
        {
            foreach (var value in _realm.All<ObjectWithList>())
            {
                _ = value;
            }

            _realm.Dispose();
            _realm = Realm.GetInstance(_realm.Config.DatabasePath);
        }

        private class ObjectWithList : RealmObject
        {
            public int Value { get; set; }

            public IList<string> Strings { get; }
        }
    }
}
