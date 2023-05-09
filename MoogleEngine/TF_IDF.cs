namespace MoogleEngine;

public class TF_IDF
{

    //Devuelvo una matriz con cada TF_IDF
    public static double[,] TFIDF(double[,] matrix, List<string> content,List<string> AllWords)
    {
        List<double> IDF = new List<double>();
        double[,] result = new double[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < AllWords.Count; i++)
        {
            int count = 0;
            for (int j = 0; j < content.Count; j++)
            {
                
                if (content[j].Contains(AllWords[i]))
                {
                    count++;
                }
            }
            IDF.Add(Math.Log10((double)content.Count / count));
            for (int j = 0; j < content.Count; j++)
            {
                result[i, j] = IDF[i] * result[i, j];
            }
        }
        return result;
    }

    //Devuelvo una matriz con cada TF
    public static double[,] TF(List<string> AllWords, List<string> content)
    {
        double[,] result = new double[AllWords.Count, content.Count];
        for (int i = 0; i < AllWords.Count; i++)
        {
            for (int j = 0; j < content.Count; j++)
            {
                string[] words = content[j].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                int cant = words.Count(w => w == AllWords[i]);
                double TF = (double)cant / words.Length;
                result[i, j] = TF;
            }
        }
        return result;
    }
}

