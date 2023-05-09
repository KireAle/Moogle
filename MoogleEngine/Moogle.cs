
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.RegularExpressions;

namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        query= query.ToLower();

        //Esta expresion regular coincide con cualquier caracter que no sea una letra o un numero
        string pattern = @"[^a-z0-9óéíüúáñ]+";
        Regex rgx = new Regex(pattern, RegexOptions.Compiled)
            ;
        //En esta linea reemplazo todos los caracteres que coinciden con la expresion regular
        //una o mas veces con un espacio en blanco
        query = rgx.Replace(query, " ");

        //Despues aplico el metodo Split() para separar la cadena por los espacios en blanco y llevarla a una lista
        List<string> WordsQuery = query.Split(' ',StringSplitOptions.RemoveEmptyEntries).ToList();
        string rute = @"C:\Users\Ronal\Desktop\1\moogle-main\moogle-main\Content";

        //Con este metodo voy accediendo a los txt que estan en una ruta determinada ylos agrego a la lista
        List<string> files = Directory.GetFiles(rute, "*.txt").ToList();

        //En esta lista van a estar los titulos de cada documento
        List<string> titles = new List<string>();

        //En esta lista van a estar los contenidos de cada documento tal como aparecen
        List<string> FilesInformation1 = new List<string>();

        //En esta lista van a estar los contenidos de cada documento aplicandoles el mismo metodo Replace()
        List<string> FilesInformation = new List<string>();

        //Voy añadiendo los elementos a cada lista segun correspondan 
        for (int i = 0; i < files.Count; i++)
        {
           string content = File.ReadAllText(files[i]);
            FilesInformation1.Add(content);
           content=content.ToLower();
            content= rgx.Replace(content, " ");
            FilesInformation.Add(content);
        }
        //En este bucle recorro el contenido de cada documento y si esta vacio se lo quito a cada lista
        for (int i = 0; i < FilesInformation.Count; i++)
        {
            if (string.IsNullOrEmpty(FilesInformation[i]))
            {

                FilesInformation.RemoveAt(i);
                files.RemoveAt(i);
                FilesInformation1.RemoveAt(i);
            }
        }

        //Aqui voy agregando los titulos de los txt sin la extension
        for (int i = 0; i < files.Count; i++)
        {
            titles.Add(Path.GetFileNameWithoutExtension(files[i]));
        }
        List<string> AllWords = new List<string>();

       //Con este ciclo separo cada contenido para sacar todas las palabras de los documentos
        for (int i = 0; i < FilesInformation.Count; i++)
        {
            
            string[] words = FilesInformation[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
           
            AllWords.AddRange(words);
          
        }

       //Aqui elimino las palabras que sean iguales
        AllWords = AllWords.Distinct().ToList();

        //Formo la matriz con los TF de cada palabra en cada documento y despues calculo el TF_IDF
        double[,] TF = TF_IDF.TF(AllWords, FilesInformation);
        double[,] Matrix = TF_IDF.TFIDF(TF,FilesInformation,AllWords);

        //Creo un diccionario de todas las palabras con este metodo 
        Dictionary<string, int> Vocabulary = Dict.ConvertToDict(AllWords);

        //Despues lo modifico
        Dict.WordsInVocabulary(WordsQuery, Vocabulary);

        //Hago una lista con los valores del diccionario
        List<int> QueryValues = Vocabulary.Values.ToList();
        List<double> Scores = new List<double>();

        //En este bucle voy cogiendo cada columna de la matriz con los TF_IDF,aplico el metodo Similitud
        //con cada columna y la lista de los valores del diccionario y voy agregando cada resultado a la lista de Scores
        for (int i = 0; i < Matrix.GetLength(1); i++)
        {
            List<double> Columnas = new List<double>();
            for (int j = 0; j < Matrix.GetLength(0); j++)
            {
                Columnas.Add(Matrix[j, i]);
            }
            Scores.Add(Coseno.Similitud(QueryValues, Columnas));

        }
        //Verifico si todos los valores de Scores son 0,y en ese caso modifico la lista aplicando el metodo Similitud
        //entre las columnas de la matriz TF y la misma lista anterior
        if (Scores.All(x=>x==0))
        {
            Scores.Clear();
            for (int i = 0; i < TF.GetLength(1); i++)
            {
                List<double> Columnas = new List<double>();
                for (int j = 0; j < TF.GetLength(0); j++)
                {
                    Columnas.Add(TF[j, i]);
                }
                Scores.Add(Coseno.Similitud(QueryValues, Columnas));
            }
        }

        //Estos 2 diccionarios tienen como clave los titulos y el numero de cada documento
        //que tienen un Score diferente de 0 y como valor los respectivos Scores
        Dictionary<string, double> ScoreTitle = new Dictionary<string, double>();
        Dictionary<int, double> ScoreDocs = new Dictionary<int, double>();
        
        for (int i = 0; i < Scores.Count; i++)
        {
            if (Scores[i] != 0)
            {
                ScoreTitle.Add(titles[i], Scores[i]);
            }
        }
        for (int i = 0; i < Scores.Count; i++)
        {
            if (Scores[i] != 0)
            {
                ScoreDocs.Add(i, Scores[i]);
            }

        }

        //Aplico este metodo pa ordenar los elemtos de un diccionario por orden ascendente segun los valores
        ScoreTitle=Dict.SortDictionary(ScoreTitle);
        ScoreDocs = Dict.SortDictionary(ScoreDocs);

        //Modifico la lista de Scores y la de titulos y la invierto para tener el orden descendente
        Scores = ScoreTitle.Values.ToList();
        Scores.Reverse();
        titles = ScoreTitle.Keys.ToList();
        titles.Reverse();

        //Creo una lista con los numeros de los documentos y los ordeno
        List<int> DocNumbers = ScoreDocs.Keys.ToList();
        DocNumbers.Reverse();
        List<string> Snippet = new List<string>();

        //En este ciclo selecciono la palabra com mayor TF_IDF de cada documento y calculo el snippet de cada documento
        for (int i = 0; i < Scores.Count; i++)
        { 
            List<string> WordsInDoc = Coseno.WordsInDoc(WordsQuery, FilesInformation[DocNumbers[i]]);
            List<int> WordsPositions = Coseno.Positions(WordsInDoc, AllWords);
            List<double> doubles = Coseno.TF_IDFWordsQueryInDoc(Matrix, DocNumbers[i], WordsPositions);
            int maxposition = (doubles.IndexOf(doubles.Max()));
            string word = WordsInDoc[maxposition];
            string snippets = Coseno.Snippet(word, FilesInformation1[DocNumbers[i]]);
            Snippet.Add(snippets);
        }

        //Creo el Array de SearchItem y voy agregandole cada parametro de cada lista
        SearchItem[] item = new SearchItem[Scores.Count];
        for (int i = 0; i < item.Length; i++)
        {
            item[i] = new SearchItem(titles[i], Snippet[i], Scores[i]);

        }

        //Crreo un array del query y un array tester de igual longitud para recorrer el documento
        string[] arrayquery = WordsQuery.ToArray();
        string[] tester = new string[arrayquery.Length];
        string suggestion = "";
        int apoyo = 5;
        for (int i = 0; i < files.Count; i++)
        {
            string[] converted = Suggestion.ConvertDocArray(FilesInformation[i]);
            string[] Texto = new string[converted.Length];

            for (int j = 0; j < (converted.Length - tester.Length) + 1; j++)
            {
                Array.Copy(converted, j, tester, 0, arrayquery.Length);
                string texte = string.Join(" ", tester);

                int similitud = Suggestion.LevenshteinDistance(query, texte);
                //Con el tester convertido a string texte voy a compararlo con el query por el metodo LevenstheinDistance

                if (similitud <= apoyo)
                {
                    apoyo = similitud;
                    suggestion = texte;
                    //me quedo con el string mas similar al query
                    //Si no encuentro una similitud <= 5 , no va a devolver ninguna sugerencia
                }
            }
        }
        return new SearchResult(item,suggestion);
      
    }
}
