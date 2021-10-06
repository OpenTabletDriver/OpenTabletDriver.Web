using System.Collections.Generic;

namespace OpenTabletDriver.Web.Models
{
    public class EnumerableSearchViewModel<T>
    {
        public IEnumerable<T> Items { set; get; }
        public string Search { set; get; }
    }
}