using System;
using System.Collections.Generic;
using System.Linq;

namespace GreplinChallenge
{
    class Program
    {
        const string Gettysburg = "FourscoreandsevenyearsagoourfaathersbroughtforthonthiscontainentanewnationconceivedinzLibertyanddedicatedtothepropositionthatallmenarecreatedequalNowweareengagedinagreahtcivilwartestingwhetherthatnaptionoranynartionsoconceivedandsodedicatedcanlongendureWeareqmetonagreatbattlefiemldoftzhatwarWehavecometodedicpateaportionofthatfieldasafinalrestingplaceforthosewhoheregavetheirlivesthatthatnationmightliveItisaltogetherfangandproperthatweshoulddothisButinalargersensewecannotdedicatewecannotconsecratewecannothallowthisgroundThebravelmenlivinganddeadwhostruggledherehaveconsecrateditfaraboveourpoorponwertoaddordetractTgheworldadswfilllittlenotlenorlongrememberwhatwesayherebutitcanneverforgetwhattheydidhereItisforusthelivingrathertobededicatedheretotheulnfinishedworkwhichtheywhofoughtherehavethusfarsonoblyadvancedItisratherforustobeherededicatedtothegreattdafskremainingbeforeusthatfromthesehonoreddeadwetakeincreaseddevotiontothatcauseforwhichtheygavethelastpfullmeasureofdevotionthatweherehighlyresolvethatthesedeadshallnothavediedinvainthatthisnationunsderGodshallhaveanewbirthoffreedomandthatgovernmentofthepeoplebythepeopleforthepeopleshallnotperishfromtheearth";
        const int MinFibonacci = 227000;
        static readonly int[] Numbers = new[] { 3, 4, 9, 14, 15, 19, 28, 37, 47, 50, 54, 56, 59, 61, 70, 73, 78, 81, 92, 95, 97, 99 };

        static void Main(string[] args) {
            Console.WriteLine("Longest reversible substring: {0}", FindLongestReversibleSubstring(Gettysburg));

            int smallest = FindSmallestPrimeFibonacciNumber(MinFibonacci);
            Console.WriteLine("Smallest prime fibonacci number: {0}", smallest);
            int sumOfDivisors = CalcSumOfPrimeDivisors(smallest + 1);
            Console.WriteLine("Sum of prime divisors: {0}", sumOfDivisors);

            Console.WriteLine("Number of subsets: {0}", CalcNumberOfSubsets(Numbers));

            Console.ReadLine();
        }

        static IEnumerable<string> GetAllSubstrings(string s) {
            for (int i = 0; i < s.Length; i++) {
                for (int j = i + 1; j < s.Length; j++) {
                    yield return s.Substring(i, j - i);
                }
            }
        }

        static string FindLongestReversibleSubstring(string s) {
            string longest = "";

            foreach (var substring in GetAllSubstrings(s)) {
                if (substring.Length > longest.Length && substring == new string(substring.Reverse().ToArray())) {
                    longest = substring;
                }
            }

            return longest;
        }

        // from http://www.dotnetperls.com/fibonacci
        static int Fibonacci(int n) {
            int a = 0;
            int b = 1;
            
            for (int i = 0; i < n; i++) {
                int temp = a;
                a = b;
                b = temp + b;
            }

            return a;
        }

        // from http://www.dotnetperls.com/prime
        public static bool IsPrime(int candidate) {
            if ((candidate & 1) == 0)
                return candidate == 2;
            
            for (int i = 3; (i * i) <= candidate; i += 2)
                if ((candidate % i) == 0)
                    return false;

            return candidate != 1;
        }

        static int FindSmallestPrimeFibonacciNumber(int min) {
            int smallest = 0;

            for (int i = 0; i < int.MaxValue; i++) {
                int temp = Fibonacci(i);

                if (temp > min && IsPrime(temp)) {
                    smallest = temp;
                    break;
                }
            }

            return smallest;
        }

        static IEnumerable<int> GetDivisors(int n) {
            return from a in Enumerable.Range(2, n / 2)
                   where n % a == 0
                   select a;
        }

        static int CalcSumOfPrimeDivisors(int n) {
            return GetDivisors(n).Where(IsPrime).Sum();
        }

        // from http://stackoverflow.com/a/999182/913491
        public static IEnumerable<IEnumerable<T>> SubSetsOf<T>(IEnumerable<T> source) {
            if (!source.Any())
                return Enumerable.Repeat(Enumerable.Empty<T>(), 1);

            var element = source.Take(1);

            var haveNots = SubSetsOf(source.Skip(1));
            var haves = haveNots.Select(set => element.Concat(set));

            return haves.Concat(haveNots);
        }

        private static int CalcNumberOfSubsets(int[] numbers) {
            return SubSetsOf(numbers).Select(subset => subset.ToArray())
                .Where(a => a.Length >= 3)
                .Where(a => {
                           int max = a.Max();
                           return a.Where(i => i != max).Sum() == max;
                       })
                .Count();
        }
    }
}
