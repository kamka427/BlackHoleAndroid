using System;

namespace BlackHoleAndroid.ViewModel
{
    /// Név: Neszlényi Kálmán Balázs
    /// Neptun kód: DPU51T
    /// Dátum: 2021.11.21.

    /// <summary>
    /// Black Hole játékmező típusa.
    /// </summary>
    public class GameField : ViewModelBase
    {
        private string _text;
        private bool _onBlackHole;


        /// <summary>
        /// A mezőfelirat lekérdezése, vagy beállítása.
        /// </summary>
        public string Text
        {
            get => _text;
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// A vízszintes koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// A függőleges koordináta lekérdezése, vagy beállítása.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// A mezősorszám lekérdezése.
        /// </summary>
        public int Number { get; set; }

        public bool OnBlackHole
        {
            get => _onBlackHole;
            set
            {
                if (_onBlackHole != value)
                {
                    _onBlackHole = value;
                    OnPropertyChanged();
                }
            }
        }
        /// <summary>
        /// A mozgás parancs lekérdezése, vagy beállítása.
        /// </summary>
        public DelegateCommand MoveCommand { get; set; }
    }
}
