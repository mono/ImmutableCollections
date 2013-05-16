//
// ImmutableQueueTests.cs
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
using System.Linq;

namespace UnitTests
{
    [TestFixture]
    public class ImmutableQueueTests
    {
        [Test]
        public void TestSimpleOperation()
        {
            var queue = ImmutableQueue.Create<int>();
            queue = queue.Enqueue(1);
            queue = queue.Enqueue(2);
            queue = queue.Enqueue(3);

            Assert.AreEqual(1, queue.Peek());
            queue = queue.Dequeue();
            Assert.IsFalse(queue.IsEmpty);

            Assert.AreEqual(2, queue.Peek());
            queue = queue.Dequeue();
            Assert.IsFalse(queue.IsEmpty);

            Assert.AreEqual(3, queue.Peek());
            queue = queue.Dequeue();
            Assert.IsTrue(queue.IsEmpty);
        }

        [Test]
        public void TestPeek()
        {
            var s = ImmutableQueue.Create<int>();
            s = s.Enqueue(1);

            Assert.AreEqual(1, s.Peek(), "#1");
        }

        [Test]
        [ExpectedException (typeof (InvalidOperationException))]
        public void TestPeekEx()
        {
            var s = ImmutableQueue.Create<int>();
            s.Peek();
        }

        [Test]
        [ExpectedException (typeof (InvalidOperationException))]
        public void TestPeekEx2()
        {
            var s = ImmutableQueue.Create<int>();
            s = s.Enqueue(1);
            s = s.Dequeue();
            s.Peek();
        }

        [Test]
        [ExpectedException (typeof (InvalidOperationException))]
        public void TestDequeueEx()
        {
            var s = ImmutableQueue.Create<int>();
            s.Dequeue();
        }

        [Test]
        [ExpectedException (typeof (InvalidOperationException))]
        public void TestDequeueEx2()
        {
            var s = ImmutableQueue.Create<int>();
            s = s.Enqueue(1);
            s = s.Dequeue();
            s.Dequeue();
        }

        [Test]
		public void TestEnumerator()
        {
            var s = ImmutableQueue.Create<int>();

            foreach (int x in s)
                Assert.Fail("#1" + x);

            s = s.Enqueue(1);

            int i = 0;

            foreach (int x in s)
            {
                Assert.AreEqual(0, i, "#2");
                Assert.AreEqual(1, x, "#3");
                i ++;
            }

            for (i = 2; i < 100; i ++)
                s = s.Enqueue(i);

            i = 1;
            Assert.AreEqual (99, s.Count ());
            foreach (int x in s)
            {
                Assert.AreEqual(i, x, "#4");
                i ++;
            }
        }
    }
}

