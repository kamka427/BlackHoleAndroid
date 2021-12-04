using System;
using System.IO;
using System.Threading.Tasks;
using BlackHoleAndroid.Droid.Persistence;
using BlackHoleAndroid.Persistence;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDataAccess))]
namespace BlackHoleAndroid.Droid.Persistence
{
    /// <summary>
    /// Tic-Tac-Toe adatel�r�s megval�s�t�sa Android platformra.
    /// </summary>
    public class AndroidDataAccess : IDataAccess
    {
        /// <summary>
        /// F�jl bet�lt�se.
        /// </summary>
        /// <param name="path">El�r�si �tvonal.</param>
        /// <returns>A beolvasott mez��rt�kek.</returns>
        public async Task<GameTable> LoadAsync(String path)
        {
            // a bet�lt�s a szem�lyen k�nyvt�rb�l t�rt�nik
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // a f�jlm�veletet taszk seg�ts�g�vel v�gezz�k (aszinkron m�don)
            String[] values = (await Task.Run(() => File.ReadAllText(filePath))).Split(' ');
            
            int tableSize = int.Parse(values[0]);
            GameTable table = new GameTable(tableSize)
            {
                CurrentP = int.Parse(values[1]),
                ShipsP1 = int.Parse(values[2]),
                ShipsP2 = int.Parse(values[3])
            };


            Int32 valueIndex = 4;
            for (Int32 rowIndex = 0; rowIndex < tableSize; rowIndex++)
            {              
                for (Int32 columnIndex = 0; columnIndex < tableSize; columnIndex++)
                {
                    table.SetValue(rowIndex, columnIndex, Int32.Parse(values[valueIndex])); // �rt�kek bet�lt�se
                    valueIndex++;
                }
            }

            return table;
        }

        /// <summary>
        /// F�jl ment�se.
        /// </summary>
        /// <param name="path">El�r�si �tvonal.</param>
        /// <param name="table">A f�jlba ki�rand� j�t�kt�bla.</param>
        public async Task SaveAsync(String path, GameTable table)
        {
            String text = "" + table.Size + " " + table.CurrentP + " " + table.ShipsP1 + " " + table.ShipsP2 + " ";

            for (Int32 i = 0; i < table.Size; i++)
            {
                for (Int32 j = 0; j < table.Size; j++)
                {
                    text += table[i, j] + " "; // mez��rt�kek
                }
            }

            // f�jl l�trehoz�sa
            String filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            // ki�r�s (aszinkron m�don)
            await Task.Run(() => File.WriteAllText(filePath, text));
        }
    }
}