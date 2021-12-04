using BlackHoleAndroid.Persistence;
using System;
using System.Threading.Tasks;

namespace BlackHoleAndroid.Model
{

    /// Név: Neszlényi Kálmán Balázs
    /// Neptun kód: DPU51T
    /// Dátum: 2021.11.21.
    #region Enumeration types

    /// <summary>
    /// A táblaméret felsorolási típusa.
    /// </summary>
    public enum MapSize { Small, Medium, Big }
    /// <summary>
    /// Az irányok felsorolási típusa.
    /// </summary>
    public enum Direction { Up, Down, Left, Right }

    #endregion

    /// <summary>
    /// A Fekete Lyuk játék típusa.
    /// </summary>
    public class GameModel
    {
        #region Fields

        /// <summary>
        /// Maga a játéktábla.
        /// </summary>
        private GameTable _gameTable;

        /// <summary>
        /// Az adatelérés.
        /// </summary>
        private readonly IDataAccess _dataAccess;
        /// <summary>
        /// A táblaméret.
        /// </summary>
        private MapSize _mapSize;

        #endregion

        #region MapSize Constants

        /// <summary>
        /// A kis táblaméret konstans szám értéke.
        /// </summary>
        private const int GenerateSmallMap = 5;
        /// <summary>
        /// A közepes táblaméret konstans szám értéke.
        /// </summary>
        private const int GenerateMediumMap = 7;
        /// <summary>
        /// A nagy táblaméret konstans szám értéke.
        /// </summary>
        private const int GenerateBigMap = 9;

        #endregion

        #region Properties

        /// <summary>
        /// A táblaméret lekérdezése.
        /// </summary>
        public MapSize MapSize { get => _mapSize; set => _mapSize = value; }
        /// <summary>
        /// Játék végének lekérdezése.
        /// </summary>
        public bool IsGameOver { get { return _gameTable.ShipsP1 == 0 || _gameTable.ShipsP2 == 0; } }
        /// <summary>
        /// A játéktábla lekérdezése.
        /// </summary>
        public GameTable Table { get { return _gameTable; } }

        #endregion

        #region Events

        /// <summary>
        /// A játék körváltásának eseménye.
        /// </summary>
        public event EventHandler<GameEventArgs> GameAdvanced;
        /// <summary>
        /// A játék végének eseménye.
        /// </summary>
        public event EventHandler<GameEventArgs> GameOver;
        /// <summary>
	    /// A játék létrehozásának eseménye.
	    /// </summary>
	    public event EventHandler<GameEventArgs> GameCreated;
        #endregion

        #region Constructor

        /// <summary>
        /// A játékmodel típus példányosítása.
        /// </summary>
        /// <param name="dataAccess"></param>
        public GameModel(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _gameTable = new GameTable();
            _mapSize = MapSize.Medium;

        }

        #endregion

        #region Private Event Methods

        /// <summary>
        /// A játék körváltásának eseményének kiváltása.
        /// </summary>
        private void OnGameAdvanced() => GameAdvanced?.Invoke(this, new GameEventArgs(false, _gameTable.CurrentP, _gameTable.ShipsP1, _gameTable.ShipsP2));
        /// <summary>
        /// A játék végének eseményének kiváltása.
        /// </summary>
        /// <param name="isWon"></param>
        private void OnGameOver(bool isWon) => GameOver?.Invoke(this, new GameEventArgs(isWon, _gameTable.CurrentP, _gameTable.ShipsP1, _gameTable.ShipsP2));
        /// <summary>
	    /// A játék létrehozás eseményének kiváltása.
	    /// </summary>
	    private void OnGameCreated() => GameCreated?.Invoke(this, new GameEventArgs(false, _gameTable.CurrentP, _gameTable.ShipsP1, _gameTable.ShipsP2));
        

        #endregion

        #region Private Game Methods

        /// <summary>
        /// Kezdőjátékos véletlenszerő kiválasztása.
        /// </summary>
        private void GenerateRandomStarter()
        {
            var rand = new Random();
            _gameTable.CurrentP = rand.Next(1, 3);
        }
        /// <summary>
        /// A játéktábla felépítése a modelben.
        /// </summary>
        /// <param name="n"></param>
        private void GenerateMap(int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == n / 2 && j == n / 2)
                    {
                        _gameTable.SetValue(i, j, -1);
                    }
                    else if ((i == j || i == n - 1 - j) && (i != (n - 1) / 2 && j != (n - 1) / 2))
                    {
                        if ((i == j && i < n / 2) || (i == n - 1 - j && i < n / 2))
                            _gameTable.SetValue(i, j, 1);
                        if ((i == n - 1 - j && i > n / 2) || (i == j && i > n / 2))
                            _gameTable.SetValue(i, j, 2);
                    }
                }
            }
            _gameTable.ShipsP1 = (_gameTable.Size - 1) / 2;
            _gameTable.ShipsP2 = (_gameTable.Size - 1) / 2;
            GenerateRandomStarter();
        }
        /// <summary>
        /// A fekete lyukba jutáskor a pontszámolásához csökkenti a bejuttatandó hajók számát a nyeréshez.
        /// </summary>
        /// <param name="játékosHajója">A játékos aki bejuttatta a hajóját a fekete lyukba.</param>
        public void RemoveShips(int játékosHajója) => _ = játékosHajója == 1 ? _gameTable.ShipsP1-- : _gameTable.ShipsP2--;

        #endregion

        #region Public Game Methods

        /// <summary>
        /// A körök léptetése.
        /// </summary>
        ///
        public void EndTurn()
        {
            _gameTable.CurrentP = _gameTable.CurrentP == 1 ? 2 : 1;

            if (IsGameOver)
                OnGameOver(true);
            else
                OnGameAdvanced();
        }
      

        /// <summary>
        /// Új játék indítása.
        /// </summary>
        public void NewGame()
        {
            switch (MapSize)
            {
                case MapSize.Small:
                    _gameTable = new GameTable(GenerateSmallMap);
                    GenerateMap(GenerateSmallMap);
                    break;
                case MapSize.Medium:
                    _gameTable = new GameTable(GenerateMediumMap);
                    GenerateMap(GenerateMediumMap);
                    break;
                case MapSize.Big:
                    _gameTable = new GameTable(GenerateBigMap);
                    GenerateMap(GenerateBigMap);
                    break;
            }
            OnGameCreated();
        }
       
        /// <summary>
        /// A hajó mozgatása.
        /// </summary>
        /// <param name="x">Visszintes koordináta.</param>
        /// <param name="y">Függőleges koordináta.</param>
        /// <param name="_d">Mozgatás iránya.</param>
        public void MoveShip(int x, int y, Direction _d)
        {
            if (_gameTable.CurrentP == _gameTable[x, y])
                switch (_d)
                {
                    case Direction.Up:
                        int currentPlayersShip = _gameTable.GetValue(x, y);
                        bool stepped = false;
                        for (int i = x - 1; i >= 0 && !stepped; i--)
                        {
                            if (_gameTable.GetValue(i, y) == -1)
                            {
                                RemoveShips(currentPlayersShip);
                                _gameTable.SetValue(x, y, 0);
                                stepped = true;
                            }
                            else if (_gameTable.GetValue(i, y) == 1 || _gameTable.GetValue(i, y) == 2)
                            {
                                _gameTable.SetValue(x, y, 0);
                                _gameTable.SetValue(i + 1, y, currentPlayersShip);
                                stepped = true;
                            }
                            else if (i == 0)
                            {
                                _gameTable.SetValue(x, y, 0);
                                _gameTable.SetValue(0, y, currentPlayersShip);
                                stepped = true;
                            }
                        }
                        break;
                    case Direction.Down:
                        currentPlayersShip = _gameTable.GetValue(x, y);
                        stepped = false;
                        {
                            for (int i = x + 1; i < Table.Size && !stepped; i++)
                            {
                                if (_gameTable.GetValue(i, y) == -1)
                                {
                                    RemoveShips(currentPlayersShip);
                                    _gameTable.SetValue(x, y, 0);
                                    stepped = true;
                                }
                                else if (_gameTable.GetValue(i, y) == 1 || _gameTable.GetValue(i, y) == 2)
                                {
                                    _gameTable.SetValue(x, y, 0);
                                    _gameTable.SetValue(i - 1, y, currentPlayersShip);
                                    stepped = true;
                                }
                                else if (i == Table.Size - 1)
                                {
                                    _gameTable.SetValue(x, y, 0);
                                    _gameTable.SetValue(Table.Size - 1, y, currentPlayersShip);
                                    stepped = true;
                                }
                            }
                        }
                        break;
                    case Direction.Left:
                        currentPlayersShip = _gameTable.GetValue(x, y);
                        stepped = false;
                        for (int i = y - 1; i >= 0 && !stepped; i--)
                        {
                            if (_gameTable.GetValue(x, i) == -1)
                            {
                                RemoveShips(currentPlayersShip);
                                _gameTable.SetValue(x, y, 0);
                                stepped = true;
                            }
                            else if (_gameTable.GetValue(x, i) == 1 || _gameTable.GetValue(x, i) == 2)
                            {
                                _gameTable.SetValue(x, y, 0);
                                _gameTable.SetValue(x, i + 1, currentPlayersShip);
                                stepped = true;
                            }
                            else if (i == 0)
                            {
                                _gameTable.SetValue(x, y, 0);
                                _gameTable.SetValue(x, 0, currentPlayersShip);
                                stepped = true;
                            }
                        }
                        break;
                    case Direction.Right:
                        currentPlayersShip = _gameTable.GetValue(x, y);
                        stepped = false;
                        for (int i = y + 1; i < Table.Size && !stepped; i++)
                        {
                            if (_gameTable.GetValue(x, i) == -1)
                            {
                                RemoveShips(currentPlayersShip);
                                _gameTable.SetValue(x, y, 0);
                                stepped = true;
                            }
                            else if (_gameTable.GetValue(x, i) == 1 || _gameTable.GetValue(x, i) == 2)
                            {
                                _gameTable.SetValue(x, y, 0);
                                _gameTable.SetValue(x, i - 1, currentPlayersShip);
                                stepped = true;
                            }
                            else if (i == Table.Size - 1)
                            {
                                _gameTable.SetValue(x, y, 0);
                                _gameTable.SetValue(x, Table.Size - 1, currentPlayersShip);
                                stepped = true;
                            }
                        }
                        break;
                }
        }
        /// <summary>
        /// A játékbetöltés aszinkron feladata.
        /// </summary>
        /// <param name="path">A fájl elérési útvonala.</param>
        /// <returns>A játéktábla betöltött adatokkal.</returns>
        /// <exception cref="InvalidOperationException">Helytelen művelet kivétel.</exception>
        public async Task LoadGameAsync(string path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");
            //pályaméret és maradó hajók
            _gameTable = await _dataAccess.LoadAsync(path);
            switch (_gameTable.Size)
            {
                case 5:
                    MapSize = MapSize.Small;
                    break;
                case 7:
                    MapSize = MapSize.Medium;
                    break;
                case 9:
                    MapSize = MapSize.Big;
                    break;
            }
            OnGameCreated();
        }
        /// <summary>
        /// A játékmentés aszinkron feladata.
        /// </summary>
        /// <param name="path">A fájl mentés helyének útvonala.</param>
        /// <exception cref="InvalidOperationException">Helytelen művelet kivétel.</exception>
        public async Task SaveGameAsync(string path)
        {
            if (_dataAccess == null)
                throw new InvalidOperationException("No data access is provided.");

            await _dataAccess.SaveAsync(path, _gameTable);
        }

        #endregion
    }
}
