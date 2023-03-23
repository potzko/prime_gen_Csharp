using System.Collections;
using System.Collections.Generic;

using System.Diagnostics;
void main(){
    void test(int num)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        int ret = Primes.getNthPrime(num);
        sw.Stop();
        Console.WriteLine("got {0} as the {1}'th prime", ret, num);
        Console.WriteLine("took {0} seconds to calc", sw.Elapsed.TotalSeconds);
    }
    test(100_000_000);
}
main();

public static class Primes
{
    static List<int> primes = new List<int>(new int[] { 2, 3, 5, 7 });
    static int segment = 1;
    public static void addNextSegment()
    {
        // use segmented sieve to add numbers to the prime list.
        
        //the next primes for whom we will check the squared segment
        int start = Primes.primes[Primes.segment];
        int end = Primes.primes[Primes.segment + 1];

        //create the bounds of the segment
        int segStart = start * start;
        int segEnd = end * end;
        //create the segment, false -> prime, true -> composite
        bool[] seg = new bool[segEnd - segStart];
        
        //the max number is segEnd which is end**2 -> only need to check primes under and including end.
        //no need to check 2, as we will jump by 2 when adding the numbers later
        for (int i = 1; i < segment + 1; i++) {
            int primeNum = Primes.primes[i];
            //start at start of segment and mark all numbers devisible by the prime number
            int ind = segStart + (primeNum - (segStart%primeNum))%primeNum;
            for (; ind - segStart < seg.Length; ind += primeNum)
            {
                seg[ind - segStart] = true;
            }
        }
        
        //no need to check 2, as we precoded first iteration, -> start != 2 -> segStart is odd
        for (int ind = segStart; ind < segEnd; ind+=2)
        {
            if (!seg[ind - segStart])
            {
                Primes.primes.Add(ind);
            }
        }
        Primes.segment++;
        seg = null;
        GC.Collect();
    }
    public static int getNthPrime(int ind){
        //returns the nth prime
        Primes.primes.EnsureCapacity(ind);
        while (Primes.primes.Count <= ind){
            Primes.addNextSegment();
        }
        return Primes.primes[ind];
    }
}


