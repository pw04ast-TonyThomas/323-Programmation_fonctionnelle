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

        public DataSeries<TResult> Transform<TResult>(Func<T, TResult> mapper)
        {
            return DataSeries<TResult>.From(_data.Select(x => mapper(x)));
        }
    }
}
