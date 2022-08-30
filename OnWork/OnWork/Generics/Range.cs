using System;
using System.Collections.Generic;
using System.Text;

namespace OnWork.Generics
{
    public class Range<T> where T : IComparable<T>
    {
        public Range(T minimum, T maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public T Minimum { get; set; }
        public T Maximum { get; set; }
        public override string ToString() => string.Format("{0}-{1}", this.Minimum, this.Maximum);
        public bool IsValid() => this.Minimum.CompareTo(this.Maximum) <= 0;

        public bool ContainsValue(T value) => (this.Minimum.CompareTo(value) <= 0) && (value.CompareTo(this.Maximum) <= 0);

        public bool IsInsideRange(Range<T> range) => this.IsValid() && range.IsValid() && range.ContainsValue(this.Minimum) && range.ContainsValue(this.Maximum);

        public bool ContainsRange(Range<T> range) => this.IsValid() && range.IsValid() && this.ContainsValue(range.Minimum) && this.ContainsValue(range.Maximum);
    }
}
