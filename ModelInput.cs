using CsvHelper.Configuration.Attributes;
using Microsoft.ML.Data;

namespace Sentinel.ML;
public class ModelInput
{
    [LoadColumn(0)]
    [BooleanTrueValues("1")]
    [BooleanFalseValues("0")]
    public bool Label {get; set;}
    
    [LoadColumn(1)]
    public string? Text {get; set;}

}