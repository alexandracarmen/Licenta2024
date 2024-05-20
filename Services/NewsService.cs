using Sentinel.ML.Classifers;

namespace Sentinel.Frontend.Services;

public class NewsService() : INewsService
{
    public INewsBinaryClassifier[] Classifiers { get; init; } = [
        new AveragedPerceptronNewsBinaryClassifier(),
        new FastForestNewsBinaryClassifier(),
        new FastTreeNewsBinaryClassifier(),
        new FieldAwareFactorizationMachineNewsBinaryClassifier(),
        new LbfgsLogisticRegressionNewsBinaryClassifier(),
        new LightGbmNewsBinaryClassifier(),
        new LinearSvmNewsBinaryClassifier(),
        new SdcaLogisticRegressionNewsBinaryClassifier(),
        new SdcaNonCalibratedNewsBinaryClassifier()
    ];

    public INewsBinaryClassifier GetClassifierByName(string name) => Classifiers.First(c => c.GetType().Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
}