using Sentinel.ML.Models;

namespace Sentinel.ML.Classifers;

public interface INewsBinaryClassifier
{
    News Predict(string prompt);
    void Retrain(News news);
    void Train();
    ClassifierMetrics Metrics {get;}
}