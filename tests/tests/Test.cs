using System;
using System.Collections.Generic;
using System.Text;

namespace tests
{
    class A
    {
        internal void DoSomething()
        {
            Console.WriteLine("Hi Do Something in A");
        }
    }    class B
    {
        A a = new A(); 
        
        public void DoSomethingWithA()
        {
            a.DoSomething();
        }
    }    class Program
    {
        public static void Main()
        {
            B b = new tests.B();

            b.DoSomethingWithA();
            
        }
    }}