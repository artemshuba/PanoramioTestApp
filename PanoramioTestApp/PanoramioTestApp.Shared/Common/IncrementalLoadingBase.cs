using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;

namespace PanoramioTestApp.Common
{
    // This class can used as a jumpstart for implementing ISupportIncrementalLoading. 
    // Implementing the ISupportIncrementalLoading interfaces allows you to create a list that loads
    //  more data automatically when the user scrolls to the end of of a GridView or ListView.
    public abstract class IncrementalLoadingBase<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        //public bool ReverseOrder { get; set; }

        protected IncrementalLoadingBase()
        {
            
        }

        protected IncrementalLoadingBase(IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                this.Add(list[i]);
            }
        }

        #region ISupportIncrementalLoading

        public bool HasMoreItems
        {
            get { return HasMoreItemsOverride(); }
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            if (_busy)
            {
                throw new InvalidOperationException("Only one operation in flight at a time");
            }

            _busy = true;

            return AsyncInfo.Run((c) => LoadMoreItemsAsync(c, count));
        }

        #endregion

        #region INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion

        #region Private methods

        async Task<LoadMoreItemsResult> LoadMoreItemsAsync(CancellationToken token, uint count)
        {
            uint c = 0;
            try
            {
                var items = await LoadMoreItemsOverrideAsync(token, count);
                //var baseIndex = _storage.Count;

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        //if (!ReverseOrder)
                        Add(item);
                        //else
                        //    Insert(0, item);
                    }
                    //_storage.AddRange(items);

                    // Now notify of the new items
                    //NotifyOfInsertedItems(baseIndex, items.Count);

                    c = (uint)items.Count;
                }
            }
            finally
            {
                _busy = false;
            }

            return new LoadMoreItemsResult() { Count = c };
        }

        #endregion

        #region Overridable methods

        protected abstract Task<IList<T>> LoadMoreItemsOverrideAsync(CancellationToken c, uint count);
        protected abstract bool HasMoreItemsOverride();

        #endregion

        #region State

        //List<T> _storage = new List<T>();
        bool _busy = false;

        #endregion
    }
}
