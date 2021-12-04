using System;
using System.Threading.Tasks;
using BlackHoleAndroid.Model;
using BlackHoleAndroid.Persistence;
using BlackHoleAndroid.View;
using BlackHoleAndroid.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BlackHoleAndroid
{
    public partial class App : Application
    {
        #region Fields

        private IDataAccess _dataAccess;
        private GameModel _gameModel;
        private GameViewModel _gameViewModel;
        private GamePage _gamePage;
        private SettingsPage _settingsPage;

        private IStore _store;
        private StoredGameBrowserModel _storedGameBrowserModel;
        private StoredGameBrowserViewModel _storedGameBrowserViewModel;
        private LoadGamePage _loadGamePage;
        private SaveGamePage _saveGamePage;

        private NavigationPage _mainPage;

        #endregion

        #region Application methods

        public App()
        {
            // játék összeállítása
            _dataAccess = DependencyService.Get<IDataAccess>(); // az interfész megvalósítását automatikusan megkeresi a rendszer

            _gameModel = new GameModel(_dataAccess);
            _gameModel.GameOver += new EventHandler<GameEventArgs>(GameModel_GameOver);

            _gameViewModel = new GameViewModel(_gameModel);
            _gameViewModel.NewGame += new EventHandler(GameViewModel_NewGame);
            _gameViewModel.LoadGame += new EventHandler(GameViewModel_LoadGame);
            _gameViewModel.SaveGame += new EventHandler(GameViewModel_SaveGame);
            _gameViewModel.ExitGame += new EventHandler(GameViewModel_ExitGame);

            _gamePage = new GamePage();
            _gamePage.BindingContext = _gameViewModel;

            _settingsPage = new SettingsPage();
            _settingsPage.BindingContext = _gameViewModel;

            // a játékmentések kezelésének összeállítása
            _store = DependencyService.Get<IStore>(); // a perzisztencia betöltése az adott platformon
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameLoading);
            _storedGameBrowserViewModel.GameSaving += new EventHandler<StoredGameEventArgs>(StoredGameBrowserViewModel_GameSaving);

            _loadGamePage = new LoadGamePage();
            _loadGamePage.BindingContext = _storedGameBrowserViewModel;

            _saveGamePage = new SaveGamePage();
            _saveGamePage.BindingContext = _storedGameBrowserViewModel;

            // nézet beállítása
            _mainPage = new NavigationPage(_gamePage); // egy navigációs lapot használunk fel a három nézet kezelésére

            MainPage = _mainPage;
        }

        protected override void OnStart()
        {
            _gameModel.NewGame();
            _gameViewModel.RefreshTable();
        }

        protected override void OnSleep()
        {

            // elmentjük a jelenleg folyó játékot
            try
            {
                Task.Run(async () => await _gameModel.SaveGameAsync("SuspendedGame"));
            }
            catch { }
        }

        protected override void OnResume()
        {
            // betöltjük a felfüggesztett játékot, amennyiben van
            try
            {
                Task.Run(async () =>
                {
                    await _gameModel.LoadGameAsync("SuspendedGame");
                    _gameViewModel.GenerateMap();
                    _gameViewModel.RefreshTable();


                });
            }
            catch { }

        }

        #endregion

        #region ViewModel event handlers

        /// <summary>
        /// Új játék indításának eseménykezelője.
        /// </summary>
        private void GameViewModel_NewGame(object sender, EventArgs e)
        {
            _gameModel.NewGame();


        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private async void GameViewModel_LoadGame(object sender, System.EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await _mainPage.PushAsync(_loadGamePage); // átnavigálunk a lapra
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void GameViewModel_SaveGame(object sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await _mainPage.PushAsync(_saveGamePage); // átnavigálunk a lapra
        }

        private async void GameViewModel_ExitGame(object sender, EventArgs e)
        {
            await _mainPage.PushAsync(_settingsPage); // átnavigálunk a beállítások lapra
        }


        /// <summary>
        /// Betöltés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameLoading(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync(); // visszanavigálunk

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                await _gameModel.LoadGameAsync(e.Name);
                _gameViewModel.RefreshTable();


            }
            catch
            {
                await MainPage.DisplayAlert("Sudoku játék", "Sikertelen betöltés.", "OK");
            }
        }

        /// <summary>
        /// Mentés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameSaving(object sender, StoredGameEventArgs e)
        {
            await _mainPage.PopAsync(); // visszanavigálunk

            try
            {
                // elmentjük a játékot
                await _gameModel.SaveGameAsync(e.Name);
            }
            catch { }

            await MainPage.DisplayAlert("Sudoku játék", "Sikeres mentés.", "OK");
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Játék végének eseménykezelője.
        /// </summary>
        private async void GameModel_GameOver(object sender, GameEventArgs e)
        {

            if (e.IsEnded) // győzelemtől függő üzenet megjelenítése
            {
                await MainPage.DisplayAlert("Sudoku játék", "Gratulálok, győztél!" + Environment.NewLine +
                                            "Összesen " +  " lépést tettél meg és " + " ideig játszottál.",
                                            "OK");
            }
            else
            {
                await MainPage.DisplayAlert("Sudoku játék", "Sajnálom, vesztettél, lejárt az idő!", "OK");
            }
        }

        #endregion
    }
}
