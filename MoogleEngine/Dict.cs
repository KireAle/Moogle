using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoogleEngine;

public class Dict
{
    //Devuelvo un Diccionario con cada palabra como clave y cada valor 0
    public static Dictionary<string, int> ConvertToDict(List<string> words)
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();
        for (int i = 0; i < words.Count; i++)
        {
            dict.Add(words[i], 0);
        }
        return dict;

    }
    
    public static Dictionary<string, double> SortDictionary(Dictionary<string, double> dictionary)
    {
        List<KeyValuePair<string, double>> list = dictionary.ToList();
        list.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        Dictionary<string, double> sortedDictionary = new Dictionary<string, double>();
        foreach (KeyValuePair<string, double> pair in list)
        {
            sortedDictionary.Add(pair.Key, pair.Value);
        }
        return sortedDictionary;
    }
    public static Dictionary<int, double> SortDictionary(Dictionary<int, double> dictionary)
    {
        List<KeyValuePair<int, double>> list = dictionary.ToList();
        list.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        Dictionary<int, double> sortedDictionary = new Dictionary<int, double>();
        foreach (KeyValuePair<int, double> pair in list)
        {
            sortedDictionary.Add(pair.Key, pair.Value);
        }
        return sortedDictionary;
    }
    //Si el diccionario contiene una palabra del query, cambio el valor de esa palbra a 1
    public static void WordsInVocabulary(List<string> query, Dictionary<string, int> Vocabulary)
    {


        for (int i = 0; i < query.Count; i++)
        {
            if (Vocabulary.ContainsKey(query[i]))
            {
                Vocabulary[query[i]] = 1;

            }

        }

    }
}

