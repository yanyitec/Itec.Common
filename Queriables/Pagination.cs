using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Itec.Queriables
{
    public class Pagination<T> : IPagination<T>
    {
        public Pagination(IQuery<T> query) {
            this.Query = query;
            //this.PageIndex = query.
        }

        public Pagination(Pagination<T> other) {
            this.FilterExpression = other.FilterExpression;
            this._PageSize = other.PageSize;
            this._PageIndex = other.PageIndex;
            this.AscendingExpression = other.AscendingExpression;
            this.DescendingExpression = other.DescendingExpression;
        }
        List<T> Items {
            get;set;
        }

        public IQuery<T> Query { get; private set; }

        public int Count {
            get {
                return Items == null ? -1 : Items.Count;
            }
        }

        public IList<T> ToList() { return this.Items; }

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        long? _RecordCount;
        public long TotalCount
        {
            get { return _RecordCount == null ? 0 : _RecordCount.Value; }
            set {
                if (value < 0) value = 0;
                if (_PageSize > 0) {
                    this.PageCount = (int)value / _PageSize;
                    if (value % _PageSize > 0) this.PageCount++;
                    if (_PageIndex > this.PageCount)
                    {
                        _PageIndex = this.PageCount;
                        
                    }
                }
                _RecordCount = value;
            }
        }
        int _PageSize;
        public int PageSize {
            get {
                return _PageSize;
            }
            set {
                if (value < 0) value = 0;
                if (value == 0) this.PageCount = -1;
                else {
                    if (_RecordCount != null)
                    {
                        this.PageCount = (int)value / _PageSize;
                        if (value % _PageSize > 0) this.PageCount++;
                        if (_PageIndex > this.PageCount)
                        {
                            _PageIndex = this.PageCount;

                        }
                    }
                    if (_PageIndex > 0) {
                        this.SkipCount = (_PageIndex - 1) * value;
                    }
                    this.TakeCount = _PageSize = value;
                }
            }
        }


        public int PageCount { get; private set; }

        int _PageIndex;

        public int PageIndex {
            get {
                return _PageIndex;
            }
            set {
                if (value < 0) value = 0;
                if (value > PageCount) value = PageCount;
                this.SkipCount = (value - 1) * _PageSize;
                _PageIndex = value;
            }
        }

        public Expression<Func<T, object>> AscendingExpression { get; set; }
        public Expression<Func<T, object>> DescendingExpression { get; set; }
        public Expression<Func<T, bool>> FilterExpression { get; set; }

        

        public int SkipCount { get; set; }
        public int TakeCount { get; set; }

        
        public T this[int index] {
            get {
                return this.Items == null ? default(T) : this.Items[index];
            }
        }
    }
}
