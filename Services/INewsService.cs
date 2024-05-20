using Sentinel.ML.Classifers;

namespace Sentinel.Frontend.Services;

public interface INewsService {
    INewsBinaryClassifier[] Classifiers {get;}
    INewsBinaryClassifier GetClassifierByName(string name);
}