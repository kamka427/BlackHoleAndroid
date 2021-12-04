using System;
using System.Threading.Tasks;

namespace BlackHoleAndroid.Persistence
{
    /// Név: Neszlényi Kálmán Balázs
    /// Neptun kód: DPU51T
    /// Dátum: 2021.11.21.
    /// <summary>
    /// A Fekete Lyuk fájl kezelő interfésze.
    /// </summary>
    public interface IDataAccess
    {
        /// <summary>
        /// Mentés fájl betöltése.
        /// </summary>
        /// <param name="path">A fájl elérési útvonala.</param>
        /// <returns>A fájlból beolvasott játéktábla.</returns>
        Task<GameTable> LoadAsync(string path);

        /// <summary>
        /// Fájl mentése a jelenlegi tábla állapotával.
        /// </summary>
        /// <param name="path">Elérési útvonal.</param>
        /// <param name="table">A fájlba kiírandó játéktábla.</param>
        Task SaveAsync(string path, GameTable table);
    }
}
