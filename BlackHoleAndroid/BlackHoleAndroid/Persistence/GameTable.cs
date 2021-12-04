using System;

namespace BlackHoleAndroid.Persistence {

    /// Név: Neszlényi Kálmán Balázs
    /// Neptun kód: DPU51T
    /// Dátum: 2021.11.21.
    /// <summary>
    /// A Fekete Lyuk játéktábla típusa.
    /// </summary>
    public class GameTable
    {

        #region Fields

        /// <summary>
        /// A tábla mezőinek értékei.
        /// </summary>
        private readonly int[,] _mapValues;
        /// <summary>
        /// A tábla mérete.
        /// </summary>
        /// /// <summary>
        /// A jelenlegi játékos.
        /// </summary>
        private int _currentP;
        /// <summary>
        /// Az első játékos maradék nyeréshez szükséges hajójának száma.
        /// </summary>
        private int _remainigShipsP1;
        /// <summary>
        /// A második játékos maradék nyeréshez szükséges hajójának száma.
        /// </summary>
        private int _remainingShipsP2;

        #endregion

        #region Properties

        public int Size { get { return _mapValues.GetLength(0); } }
        /// <summary>
        /// A tábla x,y koordinátájú mezőjének lekérdezése.
        /// </summary>
        /// <param name="x">Vízszintes koordináta</param>
        /// <param name="y">Függőleges koordináta.</param>
        /// <returns>A mező értéke.</returns>
        public int this[int x, int y] { get { return GetValue(x, y); } }
        /// <summary>
        /// A jelenlegi játékos lekérdezése.
        /// </summary>
        public int CurrentP { get => _currentP; set => _currentP = value; }
        /// <summary>
        /// Az első játékos maradék nyeréshez szükséges hajójának számának lekérdezése.
        /// </summary>
        public int ShipsP1 { get => _remainigShipsP1; set => _remainigShipsP1 = value; }
        /// <summary>
        /// A második játékos maradék nyeréshez szükséges hajójának számának lekérdezése.
        /// </summary>
        public int ShipsP2 { get => _remainingShipsP2; set => _remainingShipsP2 = value; }

        #endregion

        #region Constructors

        /// <summary>
        /// A Fekete Lyuk játéktábla példányosítása.
        /// </summary>
        /// <param name="tableSize">A tábla mérete</param>
        public GameTable(int tableSize)
        {
            if (tableSize < 0)
                throw new ArgumentOutOfRangeException(nameof(tableSize), "The table size is less than 0.");
            _mapValues = new int[tableSize, tableSize];
            CurrentP = 0;
        }   
        /// <summary>
        /// A Fekete Lyuk játéktábla példányosítása
        /// alapértelmezett esetben 7*7-es méretben.
        /// </summary>
        public GameTable() : this(7) { }

        #endregion

        #region Public methods

        /// <summary>
        /// A tábla x,y koordinátájú mezőjének lekérdezéséhez tartozó lekérdező metódus.
        /// </summary>
        /// <param name="x">Vízszintes koordináta</param>
        /// <param name="y">Függőleges koordináta.</param>
        /// <returns>A mező értéke.</returns>
        public int GetValue(int x, int y)
        {
            if (x < 0 || x >= _mapValues.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _mapValues.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");

            return _mapValues[x, y];
        }

        /// <summary>
        /// A tábla x,y koordinátájú mezőjének értékének beállítása.
        /// </summary>
        /// <param name="x">Vízszintes koordináta</param>
        /// <param name="y">Függőleges koordináta.</param>
        /// <param name="value">A mező étékének új értéke.</param>
        public void SetValue(int x, int y, int value)
        {
            if(x < 0 || x >= _mapValues.GetLength(0))
                throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
            if (y < 0 || y >= _mapValues.GetLength(1))
                throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
            if (value < -1 || value > 2)
                throw new ArgumentOutOfRangeException(nameof(value), "The value is out of range.");

            _mapValues[x, y] = value;
        }
    }

    #endregion

}
