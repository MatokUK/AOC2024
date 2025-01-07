using System.Text.RegularExpressions;

namespace AOC2024
{
    internal class Day22: IAdventSolution
    {
        public (long, long) Execute(string[] lines)
        {            
            List<long> secrets = ReadSecrets(lines);
            Dictionary<long, List<long>> secretsDict = secrets.ToDictionary(key => key, value => new List<long>() { value });


            for (int i = 0; i < 2000; i++)
            {
                secretsDict = secretsDict.ToDictionary(
                    x => x.Key,
                    x => x.Value.Append(PreusoRandom(x.Value.Last())).ToList()
                );
            }

            // get prices - last digit
            var prices = secretsDict.ToDictionary(
                x => x.Key,
                x => x.Value.Select(x => x % 10).ToList()
            );

            // prices - compute diff
            var pricesWithDiff = prices.ToDictionary(
                x => x.Key,
                x => x.Value.Zip(x.Value.Skip(1), (long a, long b) => (b, b - a)).ToList()
            );

            Console.WriteLine("Calculation of sequences");

            List<(long, long, long, long)> allPriceChanges = new();
            foreach (var (key, value) in pricesWithDiff)
            {
                var diff = value.Select(x => x.Item2).ToList();

                var priceChanges = new HashSet<(long, long, long, long)>();

                for (int i = 0; i <= diff.Count - 4; i++)
                {
                    var tuple = (diff[i], diff[i + 1], diff[i + 2], diff[i + 3]);
                    priceChanges.Add(tuple); 
                }
                allPriceChanges.AddRange(priceChanges);
            }

            long mostBananas = 0;

            foreach (var x in allPriceChanges)
            {
                long totalPrice = 0;
                foreach (var aaaa in pricesWithDiff)
                {
                    var price = FindPriceAfterSequence(aaaa.Value, x);
                    totalPrice += price;
                }

                if (mostBananas < totalPrice)
                {
                    mostBananas = totalPrice;
                    Console.WriteLine("top so far " + mostBananas);
                }
            }

            // 1460 - too low
            // 1475 - too low
            return (secretsDict.Select(kvp => kvp.Value.Last()).Sum(), mostBananas);
        }

        private long FindPriceAfterSequence(List<(long, long)> values, (long d1, long d2, long d3, long d4) seq)
        {
            for (int i = 0; i < values.Count-3; i++)
            {
                if (
                    values.ElementAt(i).Item2 == seq.d1
                    &&
                    values.ElementAt(i + 1).Item2 == seq.d2
                    &&
                    values.ElementAt(i + 2).Item2 == seq.d3
                    &&
                    values.ElementAt(i + 3).Item2 == seq.d4
                    )
                {
                    return values.ElementAt(i + 3).Item1;
                }
            }

            return 0;
        }

        private long PreusoRandom(long secret)
        {
            long result = secret * 64;
            secret = MixPrune(secret, result);

            result = secret / 32;
            secret = MixPrune(secret, result);

            result = secret * 2048;
            secret = MixPrune(secret, result);

            return secret;
        }

        private long MixPrune(long secret, long n)
        {
            return (secret ^ n) % 16777216;
        }

        private List<long> ReadSecrets(string[] lines)
        {
            List<long> numbers = new();

            foreach (string line in lines)
            {
                numbers.Add(long.Parse(line));
            }

            return numbers;
        }
    }
}
