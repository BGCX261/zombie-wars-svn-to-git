using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ZombieWars.Core.Maps;

namespace MapEditor.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public MapObject SourceBase { get; private set; }

        [DisplayName("Название")]
        public string Caption
        {
            get { return (SourceBase != null) ? SourceBase.Caption : String.Empty; }
            set { if (SourceBase != null) { SourceBase.Caption = value; OnPropertyChanged("Caption"); } }
        }

        [DisplayName("Описание")]
        public string Description
        {
            get { return (SourceBase != null) ? SourceBase.Description : String.Empty; }
            set { if (SourceBase != null) { SourceBase.Description = value; OnPropertyChanged("Description"); } }
        }

        public BaseViewModel(MapObject sourceBase)
        {
            SourceBase = sourceBase;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged

        #region IDataErrorInfo

        public string Error
        {
            get
            {
                return String.Empty;
            }
        }

        public string this[string columnName]
        {
            get
            {
                if (errors.ContainsKey(columnName)) return errors[columnName];
                return String.Empty;
            }
        }

        private Dictionary<string, string> errors = new Dictionary<string,string>();

        protected void SetError(string columnName, string errorText)
        {
            errors[columnName] = errorText;
        }

        protected void ClearError(string columnName)
        {
            errors.Remove(columnName);
        }

        #endregion IDataErrorInfo;
    }
}
