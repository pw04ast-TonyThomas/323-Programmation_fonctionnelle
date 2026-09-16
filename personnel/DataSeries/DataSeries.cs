using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSeries
{
    public class DataSeries<T>
    {
        private readonly IEnumerable<T> _data;

        private DataSeries(IEnumerable<T> data) => _data = data;

        public static DataSeries<T> From(IEnumerable<T> source) => new DataSeries<T>(source);

        public IEnumerable<T> Values => _data;

        public int Count { get => _data.Count(); }


        public static DataSeries<T> FromCsv(string path, Func<string[], T> Parser)
        {
            IEnumerable<string> lines = File.ReadAllLines(path).Skip(1);
            return new DataSeries<T>(lines.Select(line => Parser(line.Split(','))));
        }

        /// <summary>
        /// Returns the defined outliers without modifying the original DataSerie
        /// </summary>
        /// <param name="predicate">Predicate used to find the outliers</param>
        /// <returns></returns>
        public DataSeries<T> Outliers(Predicate<T> predicate) => DataSeries<T>.From(_data.Where(predicate.Invoke));

        /// <summary>
        /// returns a sanitized version of the DataSerie
        /// </summary>
        /// <param name="predicate">Predicate used to sanitize</param>
        /// <returns>sanitized DataSerie</returns>
        public DataSeries<T> Sanitize(Predicate<T> predicate) => DataSeries<T>.From(_data.Where(x => !predicate(x)));

        /// <summary>
        /// Takes any DataSerie and transforms it into whatever the mapper given does
        /// </summary>
        /// <typeparam name="TResult">The result, of any type</typeparam>
        /// <param name="mapper">the mapper that knows what transformation to do</param>
        /// <returns></returns>
        public DataSeries<TResult> Transform<TResult>(Func<T, TResult> mapper)
        {
            return DataSeries<TResult>.From(_data.Select(x => mapper(x)));
        }

        public DataSeries<TResult> Normalize<TResult>(Func<T, T, T, TResult> evaluator)
        {
            var min = _data.Min();
            var max = _data.Max();

            return DataSeries<TResult>.From(_data.Select(x => evaluator(x, min, max)));
        }
    }
}
