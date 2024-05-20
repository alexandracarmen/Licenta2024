using System.Globalization;
using System.Xml.Serialization;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.ML;
using Sentinel.ML.Models;

namespace Sentinel.ML.Misc;

public class Utils
{
    public static void AppendToCsv(IEnumerable<ModelInput> additionalData)
    {
        var datasetFile = Path.Combine(Environment.CurrentDirectory, "train.csv");
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };
        using var stream = File.Open(datasetFile, FileMode.Append);
        using var writer = new StreamWriter(stream);
        using var csv = new CsvWriter(writer, config);
        csv.WriteRecords(additionalData);
    }

    public static void SaveMetrics(ClassifierMetrics metrics)
    {
        XmlSerializer serializer = new(typeof(ClassifierMetrics));
        TextWriter writer = new StreamWriter($"{metrics.Name}-metrics.xml");
        serializer.Serialize(writer, metrics);
        writer.Close();
    }

    public static ClassifierMetrics LoadMetrics(string classifierName)
    {
        XmlSerializer serializer = new(typeof(ClassifierMetrics));
        FileStream fs = new($"{classifierName}-metrics.xml", FileMode.Open);
        return (ClassifierMetrics)serializer.Deserialize(fs)!;
    }

    public static News Predict(MLContext context, ITransformer model, string prompt)
    {
        PredictionEngine<ModelInput, ModelOutput> predictionFunction = context.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);
        var prediction = predictionFunction.Predict(new()
        {
            Text = prompt
        });
        return new News(prompt, prediction.Prediction, prediction.Probability, prediction.Score);

    }

    public static MLContext Context { get; } = new MLContext();
}