using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine
{
    internal class Suggestion
    {
        //Halla la semejanza entre dos strings y devuelve un numero de menor tamaño cuanto más se parezcan
        public static int LevenshteinDistance(string str1, string str2)
        {
            int[,] matrix = new int[str1.Length + 1, str2.Length + 1];

            for (int i = 0; i <= str1.Length; i++)
            {
                matrix[i, 0] = i;
            }
            for (int j = 0; j <= str2.Length; j++)
            {
                matrix[0, j] = j;
            }
            for (int i = 1; i <= str1.Length; i++)
            {
                for (int j = 1; j <= str2.Length; j++)
                {
                    int cost = (str1[i - 1] == str2[j - 1]) ? 0 : 1;

                    matrix[i, j] = Math.Min(Math.Min(matrix[i - 1, j] + 1, matrix[i, j - 1] + 1), matrix[i - 1, j - 1] + cost);
                }
            }
            return matrix[str1.Length, str2.Length];
        }
        //Convierte un string en array de string almacennado cada palabra en una posicion del array
        public static string[] ConvertDocArray(string documento)
        {
            string[] palabras = documento.Split(' ',StringSplitOptions.RemoveEmptyEntries);
            return palabras;
        }

    }

}
