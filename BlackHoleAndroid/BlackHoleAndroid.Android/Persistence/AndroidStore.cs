using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlackHoleAndroid.Droid.Persistence;
using BlackHoleAndroid.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidStore))]
namespace BlackHoleAndroid.Droid.Persistence
{
    /// <summary>
    /// Játék tároló megvalósítása Android platformra.
    /// </summary>
    public class AndroidStore : IStore
    {
        /// <summary>
        /// Fájlok lekérdezése.
        /// </summary>
        /// <returns>A fájlok listája.</returns>
        public async Task<IEnumerable<String>> GetFiles()
        {
            return await Task.Run(() => Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.Personal)).Select(file => Path.GetFileName(file)));
        }

        /// <summary>
        /// Módosítás idejének lekrédezése.
        /// </summary>
        /// <param name="name">A fájl neve.</param>
        /// <returns>Az utolsó módosítás ideje.</returns>
        public async Task<DateTime> GetModifiedTime(String name)
        {
            FileInfo info = new FileInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name));

            return await Task.Run(() => info.LastWriteTime);
        }
    }
}