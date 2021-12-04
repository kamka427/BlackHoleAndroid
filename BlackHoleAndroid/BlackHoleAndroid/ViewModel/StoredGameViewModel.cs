using System;

namespace BlackHoleAndroid.ViewModel
{
    /// <summary>
    /// Tárolt játék nézetmodell típusa.
    /// </summary>
    public class StoredGameViewModel : ViewModelBase
    {
        private String _name;
        private DateTime _modified;

        /// <summary>
        /// Név lekérdezése.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Módosítás idejének lekérdezése.
        /// </summary>
        public DateTime Modified
        {
            get { return _modified; }
            set
            {
                if (_modified != value)
                {
                    _modified = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Betöltés parancsa.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; set; }

        /// <summary>
        /// Mentés parancsa.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; set; }
    }
}
