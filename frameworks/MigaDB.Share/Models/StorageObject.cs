using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiteDB;

namespace Acorisoft.FutureGL.MigaDB.Models
{
    public abstract class StorageObject
    {
        [BsonId]
        public string Id { get; init; }
    }

    public abstract class StorageUIObject : StorageObject, INotifyPropertyChanged
    {
        private PropertyChangedEventHandler PropertyChangedHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected internal void RaiseUpdated([CallerMemberName] string propertyName = null)
        {
            if (string.IsNullOrEmpty(propertyName)) return;
            PropertyChangedHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected void SetValue<T>(ref T source, T value, [CallerMemberName] string name = "")
        {
            if (string.IsNullOrEmpty(name)) return;

            source = value;
            RaiseUpdated(name);
        }
        
        public event PropertyChangedEventHandler PropertyChanged
        {
            add => PropertyChangedHandler += value;
            remove => PropertyChangedHandler -= value;
        }
    }
}