//
// ImmutableListTests.cs
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
using System.Collections.Generic;

namespace UnitTests
{
	[TestFixture]
	public class ImmutableListTests
	{
		[Test]
		public void TestSimpleOperation ()
		{
			var list = ImmutableList.Create<int> ();
			list = list.Add (1);
			list = list.Add (2);
			list = list.Add (3);

			Assert.AreEqual (1, list[0]);
			Assert.AreEqual (2, list[1]);
			Assert.AreEqual (3, list[2]);
			Assert.AreEqual (3, list.Count);

			list = list.RemoveAt (1);
			Assert.AreEqual (2, list.Count);

			Assert.AreEqual (1, list[0]);
			Assert.AreEqual (3, list[1]);
		}

		[Test]
		public void TestFork ()
		{
			var list = ImmutableList.Create<int> ();
			list = list.Add (1);
			var list2 = list.Add (2);
			var list3 = list.Add (3);

			Assert.AreEqual (1, list [0]);
			Assert.AreEqual (1, list2 [0]);
			Assert.AreEqual (1, list3 [0]);
			Assert.AreEqual (2, list2 [1]);
			Assert.AreEqual (3, list3 [1]);
		}

		[Test]
		public void TestInsert ()
		{
			var list = ImmutableList.Create<int> ();
			list = list.Add (1);
			list = list.Add (3);
			list = list.Insert (1, 2);

			Assert.AreEqual (1, list[0]);
			Assert.AreEqual (2, list[1]);
			Assert.AreEqual (3, list[2]);
		}

		[Test]
		public void TestInsertCollection ()
		{
			var list = ImmutableList.Create<int> ();
			list = list.Add (1);
			list = list.Add (3);
			list = list.InsertRange (1, new [] {4, 5, 6});

			Assert.AreEqual (1, list[0]);
			Assert.AreEqual (4, list[1]);
			Assert.AreEqual (5, list[2]);
			Assert.AreEqual (6, list[3]);
			Assert.AreEqual (3, list[4]);
		}


		[Test]
		public void TestAddRange()
		{
			var toAdd = new List<int>() { 1, 2, 3, 4 };
			var immutable1 = ImmutableList.Create<int> ();
			var immutable2 = immutable1.AddRange(toAdd);
			Assert.AreNotEqual(immutable1, immutable2);
			Assert.AreEqual(4, immutable2.Count);
			for(int i = 0; i < toAdd.Count; i++)
			{
				Assert.AreEqual(toAdd[i], immutable2[i]);
			}
		}


		[Test]
		public void TestAddRangeToListCreatedWithAddRange()
		{
			var toAdd = new List<int>() { 1, 2, 3, 4 };
			var immutable1 = ImmutableList.Create<int> ();
			var immutable2 = immutable1.AddRange(toAdd);
			Assert.AreNotEqual(immutable1, immutable2);
			Assert.AreEqual(4, immutable2.Count);
			for(int i = 0; i < toAdd.Count; i++)
			{
				Assert.AreEqual(toAdd[i], immutable2[i]);
			}
			var toAdd2 = new List<int> { 5, 6, 7 };
			var immutable3 = immutable2.AddRange(toAdd2);
			Assert.AreNotEqual(immutable2, immutable3);
			Assert.AreEqual(7, immutable3.Count);
			for(int i = 0; i < toAdd2.Count; i++)
			{
				Assert.AreEqual(toAdd2[i], immutable3[4+i]);
			}
		}

		[Test]
		public void TestFork2()
		{
			var immutableBase = ImmutableList.Create<int>(1);

			var fork1 = immutableBase.Add(2);
			var fork2 = immutableBase.Add(3);

			Assert.AreEqual(immutableBase.Count, 1);
			Assert.AreEqual(immutableBase[0], 1);

			Assert.AreEqual(fork1.Count, 2);
			Assert.AreEqual(fork1[0], 1);
			Assert.AreEqual(fork1[1], 2);

			Assert.AreEqual(fork2.Count, 2);
			Assert.AreEqual(fork2[0], 1);
			Assert.AreEqual(fork2[1], 3);
		}

		[Test]
		public void TestLargeRandomInsert ()
		{
			const int N = 10000;
			var list = ImmutableList.Create<int> ();
			var reflist = new List<int> ();
			var rand = new Random (123);
			int i;
			for (i = 0; i < N; ++i) {
				var r = rand.Next (0, i + 1);
				list = list.Insert (r, i);
				reflist.Insert (r, i);
			}

			Assert.AreEqual (N, list.Count);

			// traverse by indexed access
			for (i = 0; i < N; ++i)
				Assert.AreEqual (reflist [i], list [i]);

			// traverse by enumerator
			i = 0;
			foreach (var k in list) {
				Assert.AreEqual (reflist [i], k);
				++i;
			}
		}

		[Test]
		public void TestEnumerator ()
		{
			var s = ImmutableList.Create<int>();

			foreach (int x in s)
				Assert.Fail ("#1" + x);

			s = s.Add (1);

			int i = 0;

			foreach (int x in s) {
				Assert.AreEqual  (0, i, "#2");
				Assert.AreEqual  (1, x, "#3");
				i ++;
			}

			for (i = 2; i < 100; i ++)
				s = s.Add (i);

			i = 1;

			foreach (int x in s) {
				Assert.AreEqual (i, x, "#4");
				i ++;
			}
		}

		[Test]
		public void TestCopyTo ()
		{
			var list = ImmutableList.Create<int> ();

			for (int i = 0 ; i <10;i++)
				list = list.Add (i);

			int[] array = new int[10];

			list.CopyTo (3, array, 5, 5);

			for (int i = 0 ; i < 5;i++)
				Assert.AreEqual (list[i + 3], array[i + 5]);
		}


		[Test]
		public void TestForeach ()
		{
			var list = ImmutableList.Create<int> ();

			for (int i = 1 ; i <= 10;i++)
				list = list.Add (i);

			int sum = 0;
			list.ForEach (i => sum += i);
			Assert.AreEqual ((1 + 10) * 10 / 2, sum);
		}

		[Test]
		public void TestBuilderLargeRandomInsert ()
		{
			const int N = 10000;
			var list = ImmutableList.Create<int> ();
			var reflist = new List<int> ();
			var rand = new Random (123);
			int i;
			for (i = 0; i < N / 2; ++i) {
				var r = rand.Next (0, i + 1);
				list = list.Insert (r, i);
				reflist.Insert (r, i);
			}
			var builder = list.ToBuilder ();
			for (i = N / 2; i < N; ++i) {
				var r = rand.Next (0, i + 1);
				builder.Insert (r, i);
				reflist.Insert (r, i);
			}
			list = builder.ToImmutable ();

			Assert.AreEqual (N, list.Count);

			// traverse by indexed access
			for (i = 0; i < N; ++i)
				Assert.AreEqual (reflist [i], list [i]);

			// traverse by enumerator
			i = 0;
			foreach (var k in list) {
				Assert.AreEqual (reflist [i], k);
				++i;
			}
		}

		
		[Test]
		public void TestFindAll ()
		{
			var list = ImmutableList.Create<int> ();

			for (int i = 0 ; i < 10;i++)
				list = list.Add (i);

			var all = list.FindAll (x => x % 2 == 0);
			Assert.AreEqual (10 / 2, all.Count);
			for (int i = 0 ; i < 10;i += 2)
				Assert.AreEqual (i, all[i / 2]);
		}
	}
}
