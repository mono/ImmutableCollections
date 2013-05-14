//
// Test.cs
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
	public class ImmutableStackTests
    {
        [Test]
        public void TestSimpleOperation()
        {
            var stack = ImmutableStack.Create<int>();
            stack = stack.Push(1);
            stack = stack.Push(2);
            stack = stack.Push(3);

            Assert.AreEqual(3, stack.Peek());
            stack = stack.Pop();
            Assert.IsFalse(stack.IsEmpty);

            Assert.AreEqual(2, stack.Peek());
            stack = stack.Pop();
            Assert.IsFalse(stack.IsEmpty);

            Assert.AreEqual(1, stack.Peek());
            stack = stack.Pop();
            Assert.IsTrue(stack.IsEmpty);
        }

        [Test]
        public void TestPeek()
        {
            var s = ImmutableStack.Create<int>();
            s = s.Push(1);

            Assert.AreEqual(1, s.Peek(), "#1");

            var enumerator = s.GetEnumerator();
            s.Peek();
            enumerator.Reset();
        }

        [Test]
        [ExpectedException (typeof (InvalidOperationException))]
        public void TestPeekEx()
        {
            var s = ImmutableStack.Create<int>();
            s.Peek();
        }

        [Test]
        [ExpectedException (typeof (InvalidOperationException))]
        public void TestPeekEx2()
        {
            var s = ImmutableStack.Create<int>();
            s = s.Push(1);
            s = s.Pop();
            s.Peek();
        }

        [Test]
        [ExpectedException (typeof (InvalidOperationException))]
        public void TestPopEx()
        {
            var s = ImmutableStack.Create<int>();
            s.Pop();
        }

        [Test]
        [ExpectedException (typeof (InvalidOperationException))]
        public void TestPopEx2()
        {
            var s = ImmutableStack.Create<int>();
            s = s.Push(1);
            s = s.Pop();
            s.Pop();
        }

        [Test]
        public void TestEnumerator()
        {
            var s = ImmutableStack.Create<int>();

            foreach (int x in s)
                Assert.Fail("#1:" + x);

            s = s.Push(1);

            int i = 0;

            foreach (int x in s)
            {
                Assert.AreEqual(0, i, "#2");
                Assert.AreEqual(1, x, "#3");
                i ++;
            }

            i = 0;

            s = s.Push(2);
            s = s.Push(3);

            foreach (int x in s)
            {
                Assert.AreEqual(3 - i, x, "#4");
                Assert.IsTrue(i < 3, "#5");
                i ++;
            }
        }
    }
}

