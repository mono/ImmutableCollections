//
// ImmutableDictionaryTests.cs
//
// Author:
//       Mike Krüger <mkrueger@xamarin.com>
//
// Copyright (c) 2013 Xamarin Inc. (http://xamarin.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using NUnit.Framework;
using System.Collections.Immutable;

namespace UnitTests
{
	[TestFixture]
	public class ImmutableDictionaryTests
	{
		[Test]
		public void TestSimpleOperation ()
		{
			var  dict = ImmutableDictionary.Create<string, string> ();
			Assert.AreEqual (0, dict.Count);

			dict = dict.Add ("Hello", "World");
			Assert.AreEqual (1, dict.Count);

			dict = dict.Add ("Xamarin", "Rocks");
			Assert.AreEqual (2, dict.Count);

			Assert.AreEqual ("World", dict["Hello"]);
			Assert.AreEqual ("Rocks", dict["Xamarin"]);

			dict = dict.SetItem ("Hello", "Immutability");
			Assert.AreEqual (2, dict.Count);
			Assert.AreSame ("Immutability", dict["Hello"]);

			dict = dict.SetItem ("Mutation", "Sucks");
			Assert.AreEqual (3, dict.Count);
			Assert.AreSame ("Sucks", dict["Mutation"]);

			var items = new string[] { "Hello", "Mutation", "Xamarin" };
			int i = 0;
			foreach (var kvp in dict) {
				Assert.AreEqual (items [i], kvp.Key);
				++i;
			}
		}

		[Test]
		public void TestLargeRandomInsert ()
		{
			const int N = 10000;
			var indexes = new int [N];
			for (int i = 0; i < N; ++i)
				indexes [i] = i;

			var dict = ImmutableDictionary.Create<int, int> ();

			var rand = new Random (123);

			for (int i = N; i > 0; --i) {
				var ri = rand.Next (0, i);
				var rv = indexes [ri];
				indexes [ri] = indexes [i - 1];
				dict = dict.Add (rv, -rv);
			}

			int j = 0;
			foreach (var kvp in dict) {
				Assert.AreEqual (j, kvp.Key);
				Assert.AreEqual (-j, kvp.Value);
				++j;
			}
		}

		[Test]
		public void TestBuilderLargeRandomInsert ()
		{
			const int N = 10000;
			var indexes = new int [N];
			for (int i = 0; i < N; ++i)
				indexes [i] = i;
			
			var dict = ImmutableDictionary.Create<int, int> ();
			
			var rand = new Random (123);
			
			for (int i = N; i > N/2; --i) {
				var ri = rand.Next (0, i);
				var rv = indexes [ri];
				indexes [ri] = indexes [i - 1];
				dict = dict.Add (rv, -rv);
			}

			var builder = dict.ToBuilder ();
			for (int i = N/2; i > 0; --i) {
				var ri = rand.Next (0, i);
				var rv = indexes [ri];
				indexes [ri] = indexes [i - 1];
				builder.Add (rv, -rv);
			}
			dict = builder.ToImmutable ();

			int j = 0;
			foreach (var kvp in dict) {
				Assert.AreEqual (j, kvp.Key);
				Assert.AreEqual (-j, kvp.Value);
				++j;
			}
		}
	}
}
