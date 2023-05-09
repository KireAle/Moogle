using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine;

 public class Coseno
{

    //Calculo la similitud entre dos vectores
    public static double Similitud(List<int> vector1, List<double> vector2)
    {
        double productoEscalar = 0;
        double magnitud1 = 0;
        double magnitud2 = 0;

        for (int i = 0; i < vector1.Count; i++)
        {
            productoEscalar += vector1[i] * vector2[i];
            magnitud1 += Math.Pow(vector1[i], 2);
            magnitud2 += Math.Pow(vector2[i], 2);
        }
        magnitud1 = Math.Sqrt(magnitud1);
        magnitud2 = Math.Sqrt(magnitud2);
        if(magnitud1==0 || magnitud2 == 0)
        {
            return 0;
        }
        return productoEscalar / (magnitud1 * magnitud2);
    }
    //Creo una lista con las palabras del query que aparezcan en un documento
    public static List<string> WordsInDoc(List<string> query, string ContentDoc)
    {
        List<string> strings = ContentDoc.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        List<string> result = new List<string>();
        foreach (string str in query)
        {
            if (strings.Contains(str))
            {
                result.Add(str);
            }

        }
        return result;
    }
    //Busco las posiciones de esas palabras en la lista de todas las palabras
    public static List<int> Positions(List<string> query, List<string> AllWords)
    {
        List<int> result = new List<int>();
        Dictionary<string, int> a = new Dictionary<string, int>();
        for (int i = 0; i < AllWords.Count; i++)
        {
            a.Add(AllWords[i], i);
        }

        for (int i = 0; i < query.Count; i++)
        {
            result.Add(a[query[i]]);
        }
        return result;
    }
    //Busco el TF_IDF de cada una de esas palabras
    public static List<double> TF_IDFWordsQueryInDoc(double[,] matrix, int docnumber, List<int> positions)
    {
        List<double> result = new List<double>();
        for (int i = 0; i <positions.Count; i++)
        {
            result.Add(matrix[positions[i], docnumber]);

        }
        return result;

    }
    //Busco la palabra con mayor TF_IDF y calculo la similitud con cada porcion del documento original
    //despues de separar los string por los espacios
    //Busco la porcion con mayor similitud y devuelvo una cantidad a la izquiera y a la derecha

    public static string Snippet( string txt,string doc_content1)
    {
        List<string> result = new List<string>();
        List<int> distances = new List<int>();
       
        List<string> strings1=doc_content1.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        for(int i=0; i < strings1.Count; i++)
        {
            int distance = Suggestion.LevenshteinDistance(strings1[i], txt);
            distances.Add(distance);
        }

        int indice = distances.IndexOf(distances.Min());
       
       for (int i = indice; i >= 0 && i >= indice - 50; i--)
            {
                result.Add(strings1[i]);
            }
            result.Reverse();
        for(int i = indice +1;i<strings1.Count && i<=indice+50; i++)
        {
            result.Add(strings1[i]);
        }

        string snippet = string.Join(" ", result);
        return snippet;
       
    }
}


