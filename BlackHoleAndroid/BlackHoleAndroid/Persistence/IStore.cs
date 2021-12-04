using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlackHoleAndroid.Persistence
{
    /// <summary>
    /// Játék tároló interfésze.
    /// </summary>
    public interface IStore
    {
        /// <summary>
        /// Fájlok lekérdezése.
        /// </summary>
        /// <returns>A fájlok listája.</returns>
        Task<IEnumerable<String>> GetFiles();

        /// <summary>
        /// Módosítás idejének lekrédezése.
        /// </summary>
        /// <param name="name">A fájl neve.</param>
        /// <returns>Az utolsó módosítás ideje.</returns>
        Task<DateTime> GetModifiedTime(String name);
    }
}
