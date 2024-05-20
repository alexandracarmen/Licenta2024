using Microsoft.ML.Data;

namespace Sentinel.ML;
public class ModelOutput : ModelInput
{

    [ColumnName("PredictedLabel")]
    public bool Prediction { get; set; }
    public float Score { get; set; }
    public float Probability {get; set;}
}