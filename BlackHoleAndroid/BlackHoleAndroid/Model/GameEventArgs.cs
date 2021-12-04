using System;

namespace BlackHoleAndroid.Model
{
    /// Név: Neszlényi Kálmán Balázs
    /// Neptun kód: DPU51T
    /// Dátum: 2021.11.21.
    /// <summary>
    /// Fekete Lyuk eseményargumentum típusa.
    /// </summary>
    public class GameEventArgs : EventArgs
    {
        /// <summary>
        /// Vége van-e már a játéknak.
        /// </summary>
        private readonly bool _isEnded;
        /// <summary>
        /// Jelenleg soron lévő játékos.
        /// </summary>
        private int _currentP;
        /// <summary>
        /// Az 1. játékos fekete lyukba juttatandó hajóinak száma.
        /// </summary>
        private int _shipsLeftP1;
        // <summary>
        /// Az 2. játékos fekete lyukba juttatandó hajóinak száma.
        /// </summary>
        private int _shipsLeftP2;
        /// <summary>
        /// Jelenleg soron lévő játékos lekérdezése.
        /// </summary>
        public int CurrentP { get => _currentP; set => _currentP = value; }
        /// <summary>
        /// Az 1. játékos fekete lyukba juttatandó hajóinak számámank lekérdezése.
        /// </summary>
        public int ShipsLeftP1 { get => _shipsLeftP1; set => _shipsLeftP1 = value; }
        /// <summary>
        /// Az 2. játékos fekete lyukba juttatandó hajóinak számámank lekérdezése.
        /// </summary>
        public int ShipsLeftP2 { get => _shipsLeftP2; set => _shipsLeftP2 = value; }
        /// <summary>
        /// Lekérdezi, hogy tart-e még a játék.
        /// </summary>
        public bool IsEnded { get { return _isEnded; } }
        /// <summary>
        /// A Fekete Lyuk eseményargumentum példányosítása.
        /// </summary>
        /// <param name="isEnded">Véget ért-e a játék.</param>
        /// <param name="currentP">Jelenleg soron lévő játékos.</param>
        /// <param name="shipLeftP1">Az 1. játékos fekete lyukba juttatandó hajóinak száma.</param>
        /// <param name="shipLeftP2">Az 2. játékos fekete lyukba juttatandó hajóinak száma.</param>
        public GameEventArgs(bool isEnded, int currentP, int shipLeftP1, int shipLeftP2)
        {
            _isEnded = isEnded;
            _currentP = currentP;
            _shipsLeftP1 = shipLeftP1;
            _shipsLeftP2 = shipLeftP2;
        }
    }
}
