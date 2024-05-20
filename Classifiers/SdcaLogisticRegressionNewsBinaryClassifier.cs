using Microsoft.ML;
using Microsoft.ML.Data;
using Sentinel.ML.Misc;
using Sentinel.ML.Models;

namespace Sentinel.ML.Classifers;

public class SdcaLogisticRegressionNewsBinaryClassifier : INewsBinaryClassifier
{
    private ITransformer Model { get; set; }
    public ClassifierMetrics Metrics { get; private set; }

    public SdcaLogisticRegressionNewsBinaryClassifier()
    {
        try
        {
            string classifierName = GetType().Name.Split("NewsBinaryClassifier")[0];
            Model = Utils.Context.Model.Load(Path.Combine(Environment.CurrentDirectory, $"{classifierName}-pretrained-model.zip"), out _);
            Metrics = Utils.LoadMetrics(classifierName);
        }
        catch
        {
            Console.WriteLine("Failed to load pretrained model, training...");
            Train();
        }
        if (Model == null || Metrics == null)
            throw new Exception("Failed to train model");
    }

    public News Predict(string prompt) => Utils.Predict(Utils.Context, Model, prompt);

    public void Train()
    {
        var dataView = Utils.Context.Data.LoadFromTextFile<ModelInput>(Path.Combine(Environment.CurrentDirectory, "train.csv"), separatorChar: ',', hasHeader: true);
        var estimator = Utils.Context.Transforms.Text.FeaturizeText(outputColumnName: "Features", inputColumnName: nameof(ModelInput.Text))
            .Append(Utils.Context.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));
        var trainTestSplit = Utils.Context.Data.TrainTestSplit(dataView, testFraction: 0.2);
        var model = estimator.Fit(trainTestSplit.TrainSet);
        IDataView predictions = model.Transform(trainTestSplit.TestSet);
        BinaryClassificationMetrics metrics = Utils.Context.BinaryClassification.Evaluate(predictions);
        Console.WriteLine();
        Console.WriteLine("Model quality metrics evaluation");
        Console.WriteLine("--------------------------------");
        Console.WriteLine($"Accuracy: {metrics.Accuracy:P2}");
        Console.WriteLine($"Auc: {metrics.AreaUnderRocCurve:P2}");
        Console.WriteLine($"F1Score: {metrics.F1Score:P2}");
        Console.WriteLine(metrics.ConfusionMatrix.GetFormattedConfusionTable());
        Console.WriteLine("=============== End of model evaluation ===============");
        Utils.Context.Model.Save(model, dataView.Schema, Path.Combine(Environment.CurrentDirectory, $"{GetType().Name.Split("NewsBinaryClassifier")[0]}-pretrained-model.zip"));
        Model = model;
        Metrics = new ClassifierMetrics(
            GetType().Name.Split("NewsBinaryClassifier")[0],
            metrics.Accuracy,
            metrics.AreaUnderRocCurve,
            metrics.F1Score,
            (int)metrics.ConfusionMatrix.Counts[0][0],
            (int)metrics.ConfusionMatrix.Counts[0][1],
            (int)metrics.ConfusionMatrix.Counts[1][0],
            (int)metrics.ConfusionMatrix.Counts[1][1],
            metrics.PositiveRecall,
            metrics.PositivePrecision,
            metrics.NegativeRecall,
            metrics.NegativePrecision
        );
        Utils.SaveMetrics(Metrics);
    }

    public void Retrain(News news)
    {
        Utils.AppendToCsv([new() { Text = news.Text, Label = news.Truth }]);
        Train();
    }
}
