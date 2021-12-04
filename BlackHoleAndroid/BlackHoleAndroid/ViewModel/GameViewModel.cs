using System;
using System.Collections.ObjectModel;
using BlackHoleAndroid.Model;

namespace BlackHoleAndroid.ViewModel
{
    /// Név: Neszlényi Kálmán Balázs
    /// Neptun kód: DPU51T
    /// Dátum: 2021.11.21.

    /// <summary>
    /// Black Hole nézetmodell típusa.
    /// </summary>
    public class GameViewModel : ViewModelBase
    {
        #region Fields

        private readonly GameModel _model; // modell

        #endregion

        #region Properties

        /// <summary>
        /// Az új játék kezdése parancs lekérdezése.
        /// </summary>
        public DelegateCommand NewGameCommand { get; private set; }

        /// <summary>
        /// A játék betöltése parancs lekérdezése.
        /// </summary>
        public DelegateCommand LoadGameCommand { get; private set; }

        /// <summary>
        /// A játék mentése parancs lekérdezése.
        /// </summary>
        public DelegateCommand SaveGameCommand { get; private set; }

        /// <summary>
        /// A kilépés parancsának lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// A játékmező gyűjtemény lekérdezése.
        /// </summary>
        public ObservableCollection<GameField> Fields { get; set; }

        /// <summary>
        /// A játékmező méretének lekérdezése.
        /// </summary>
        public int MapSizeView { get; set; }

        public string Selection
        {
            get
            {
                switch(_model.MapSize)
                {
                    case MapSize.Small:
                        return "Small";

                    case MapSize.Medium:
                        return "Medium";

                    case MapSize.Big:
                        return "Big";

                    default: return "Medium";
                }
            }
            set
            {

                switch(value)
                {
                    case "Small":
                        _model.MapSize = MapSize.Small;
                        break;
                    case "Medium":
                        _model.MapSize = MapSize.Medium;
                        break;
                    case "Big":
                        _model.MapSize = MapSize.Big;
                        break;

                }

                
                OnPropertyChanged(nameof(Selection));
            }
        }

       
        /// <summary>
        /// A jelenlegi játékos lekérdezése.
        /// </summary>
        public string ActualPlayer { get { return (_model.Table.CurrentP == 1 ? "Piros" : "Kék"); } }

        /// <summary>
        /// Az egyes játékos még hátralévő hajóinak lekérdezése.
        /// </summary>
        public int P1Left { get { return _model.Table.ShipsP1; } }

        /// <summary>
        /// A kettes játékos még hátralévő hajóinak lekérdezése.
        /// </summary>
        public int P2Left { get { return _model.Table.ShipsP2; } }

        #endregion

        #region Events

        /// <summary>
        /// Új játék kezdésének eseménye.
        /// </summary>
        public event EventHandler NewGame;

        /// <summary>
        /// A játék betöltésének eseménye.
        /// </summary>
        public event EventHandler LoadGame;

        /// <summary>
        /// A játék mentésének eseménye.
        /// </summary>
        public event EventHandler SaveGame;

        /// <summary>
        /// A játékból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitGame;

        

        #endregion

        #region Constructors

        /// <summary>
        /// Black Hole nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public GameViewModel(GameModel model)
        {
            // A játék csatlakoztatása a viewmodel-hez
            _model = model;
            _model.GameAdvanced += new EventHandler<GameEventArgs>(Model_GameAdvanced);
            _model.GameOver += new EventHandler<GameEventArgs>(Model_GameOver);
            _model.GameCreated += new EventHandler<GameEventArgs>(Model_GameCreated);

            // A parancsok kezelése beállítása.
            NewGameCommand = new DelegateCommand(param => OnNewGame());
            LoadGameCommand = new DelegateCommand(param => OnLoadGame());
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            // A játéktábla létrehozása.
            Fields = new ObservableCollection<GameField>();
            GenerateMap();
            RefreshTable();
        }


        #endregion

        #region Private methods
        /// <summary>
        /// A játéktér létrehozása.
        /// </summary>
        public void GenerateMap()
        {
            Fields.Clear();

            for (int i = 0; i < _model.Table.Size; i++)
            {
                for (int j = 0; j < _model.Table.Size; j++)
                {
                    Fields.Add(new GameField
                    {
                        Text = string.Empty,
                        X = i,
                        Y = j,
                        Number = i * _model.Table.Size + j,
                        MoveCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))

                    });
                }
            }
        }


        /// <summary>
        /// A játék léptetésének eseménykiváltása.
        /// </summary>
        /// <param name="index">A lépett mező indexe.</param>
        private void StepGame(int index)
        {
            GameField field = Fields[index];

            // A lépések megjelenítése a játék szabályai szerint.
            if (field.Text == "1" || field.Text == "2")
            {
                RefreshTable();
                if (field.Y < _model.Table.Size - 1 && _model.Table[field.X, field.Y] == _model.Table.CurrentP && _model.Table[field.X, field.Y + 1] < 1)
                {
                    if (_model.Table[field.X, field.Y + 1] == -1)
                    {
                        Fields[index + 1].OnBlackHole = true;
                    }

                    Fields[index + 1].Text = "jobbra";
                }
                if (field.X > 0 && _model.Table[field.X, field.Y] == _model.Table.CurrentP && _model.Table[field.X - 1, field.Y] < 1)
                {
                    if (_model.Table[field.X - 1, field.Y] == -1)
                    {
                        Fields[index - (1 * _model.Table.Size)].OnBlackHole = true;
                    }

                    Fields[index - (1 * _model.Table.Size)].Text = "fel";
                }
                if (field.Y > 0 && _model.Table[field.X, field.Y] == _model.Table.CurrentP && _model.Table[field.X, field.Y - 1] < 1)
                {
                    if (_model.Table[field.X, field.Y - 1] == -1)
                    {
                        Fields[index - 1].OnBlackHole = true;
                    }

                    Fields[index - 1].Text = "balra";
                }
                if (field.X < _model.Table.Size - 1 && _model.Table[field.X, field.Y] == _model.Table.CurrentP && _model.Table[field.X + 1, field.Y] < 1)
                {
                    if (_model.Table[field.X + 1, field.Y] == -1)
                    {
                        Fields[index + (1 * _model.Table.Size)].OnBlackHole = true;
                    }

                    Fields[index + (1 * _model.Table.Size)].Text = "le";
                }
            }
            if (field.Text == "balra")
            {
                _model.MoveShip(field.X, field.Y + 1, Direction.Left);
                Fields[(_model.Table.Size * _model.Table.Size - 1) / 2].OnBlackHole = false;
                RefreshTable();
                _model.EndTurn();
            }
            if (field.Text == "jobbra")
            {
                _model.MoveShip(field.X, field.Y - 1, Direction.Right);
                Fields[(_model.Table.Size * _model.Table.Size - 1) / 2].OnBlackHole = false;
                RefreshTable();
                _model.EndTurn();
            }
            if (field.Text == "fel")
            {
                _model.MoveShip(field.X + 1, field.Y, Direction.Up);
                Fields[(_model.Table.Size * _model.Table.Size - 1) / 2].OnBlackHole = false;
                RefreshTable();
                _model.EndTurn();
            }
            if (field.Text == "le")
            {
                _model.MoveShip(field.X - 1, field.Y, Direction.Down);
                Fields[(_model.Table.Size * _model.Table.Size - 1) / 2].OnBlackHole = false;
                RefreshTable();
                _model.EndTurn();
            }
        }
        /// <summary>
        /// A játéktábla frissítése.
        /// </summary>
        public void RefreshTable()
        {
            foreach (GameField field in Fields) // inicializálni kell a mezőket is
            {
                field.Text = _model.Table[field.X, field.Y] != 0 ? _model.Table[field.X, field.Y].ToString() : string.Empty;
            }
            OnPropertyChanged(nameof(ActualPlayer));

        }


        #endregion

        #region Game event handlers

        /// <summary>
        /// A játék végének eseménykezelője.
        /// </summary>
        private void Model_GameOver(object sender, GameEventArgs e)
        {
            NewGame?.Invoke(this, EventArgs.Empty);

        }

        /// <summary>
        /// A játék előrehaladásának eseménykezelője.
        /// </summary>
        private void Model_GameAdvanced(object sender, GameEventArgs e)
        {
            OnPropertyChanged(nameof(ActualPlayer));
            OnPropertyChanged(nameof(P1Left));
            OnPropertyChanged(nameof(P2Left));

        }

        /// <summary>
        /// A játék létrehozásának eseménykezelője.
        /// </summary>
        private void Model_GameCreated(object sender, GameEventArgs e)
        {
            switch (_model.MapSize)
            {
                case MapSize.Small:
                    MapSizeView = 5;
                    break;
                case MapSize.Medium:
                    MapSizeView = 7;
                    break;
                case MapSize.Big:
                    MapSizeView = 9;
                    break;

            }
            OnPropertyChanged(nameof(Selection));
            OnPropertyChanged(nameof(MapSizeView));
            OnPropertyChanged(nameof(ActualPlayer));
            OnPropertyChanged(nameof(P1Left));
            OnPropertyChanged(nameof(P2Left));
           
            GenerateMap();
            RefreshTable();
        }

        #endregion

        #region Event methods

        /// <summary>
        /// Új játék indításának eseménykiváltása.
        /// </summary>
        private void OnNewGame()
        {
            NewGame?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(ActualPlayer));
            OnPropertyChanged(nameof(P1Left));
            OnPropertyChanged(nameof(P2Left));
        }

        /// <summary>
        /// A játék betöltésének eseménykiváltása.
        /// </summary>
        private void OnLoadGame()
        {
            LoadGame?.Invoke(this, EventArgs.Empty);
            OnPropertyChanged(nameof(Selection));
            OnPropertyChanged(nameof(MapSizeView));
            OnPropertyChanged(nameof(ActualPlayer));
            OnPropertyChanged(nameof(P1Left));
            OnPropertyChanged(nameof(P2Left));
           

          
        }

        /// <summary>
        /// A játék mentésének eseménykiváltása.
        /// </summary>
        private void OnSaveGame()
        {
            SaveGame?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// A játékból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitGame()
        {
            ExitGame?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
