namespace Sentinel.ML.Models;

public class ClassifierMetrics
{
    public string Name { get; set; } = "";
    public double Accuracy { get; set; }
    public double AreaUnderRocCurve { get; set; }
    public double F1Score { get; set; }

    public double PositiveRecall { get; set; }

    public double PositivePrecision { get; set; }

    public double NegativeRecall { get; set; }

    public double NegativePrecision { get; set; }

    public int TruePositive { get; set; }
    public int FalsePositive { get; set; }
    public int FalseNegative { get; set; }
    public int TrueNegative { get; set; }

    public ClassifierMetrics() { }

    public ClassifierMetrics(string Name, double Accuracy, double AreaUnderRocCurve, double F1Score, int TP, int FN, int FP, int TN, double PR, double PP, double NR, double NP)
    {
        this.Name = Name;
        this.Accuracy = Accuracy;
        this.AreaUnderRocCurve = AreaUnderRocCurve;
        this.F1Score = F1Score;
        TruePositive = TP;
        FalsePositive = FP;
        FalseNegative = FN;
        TrueNegative = TN;
        PositiveRecall = PR;
        PositivePrecision = PP;
        NegativeRecall = NR;
        NegativePrecision = NP;
    }
}