using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class InBody
    {
        public int InBodyId { get; set; }
        public int SId { get; set; }
        public DateTime Date { get; set; }
        public short Age { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double WeightMinRange { get; set; }
        public double WeightMaxRange { get; set; }
        public double Smm { get; set; }
        public double SmmminRange { get; set; }
        public double SmmmaxRange { get; set; }
        public double TotalBodyWater { get; set; }
        public double TotalBodyWaterMinRange { get; set; }
        public double TotalBodyWaterMaxRange { get; set; }
        public double BodyFatMass { get; set; }
        public double BodyFatMassMinRange { get; set; }
        public double BodyFatMassMaxRange { get; set; }
        public double Bmi { get; set; }
        public double BmiminRange { get; set; }
        public double BmimaxRange { get; set; }
        public double Protein { get; set; }
        public double ProteinMinRange { get; set; }
        public double ProteinMaxRange { get; set; }
        public double Ffm { get; set; }
        public double FfmminRange { get; set; }
        public double FfmmaxRange { get; set; }
        public double Mineral { get; set; }
        public double MineralMinRange { get; set; }
        public double MineralMaxRange { get; set; }
        public double Pbf { get; set; }
        public double PbfminRange { get; set; }
        public double PbfmaxRange { get; set; }
        public double Whr { get; set; }
        public double WhrminRange { get; set; }
        public double WhrmaxRange { get; set; }
        public double Bmr { get; set; }
        public double BmrminRange { get; set; }
        public double BmrmaxRange { get; set; }
        public byte ProteinLevel { get; set; }
        public byte BoneMineralLevel { get; set; }
        public byte BodyFatMassLevel { get; set; }
        public byte WeightLevel { get; set; }
        public byte Smmlevel { get; set; }
        public byte Bmilevel { get; set; }
        public byte Pbflevel { get; set; }
        public byte Whrlevel { get; set; }
        public string MuscleControl { get; set; }
        public string FatControl { get; set; }
        public double UpperRightMuscle { get; set; }
        public double UpperLeftMuscle { get; set; }
        public double CenterMuscle { get; set; }
        public double BottomRightMuscle { get; set; }
        public double BottomLeftMuscle { get; set; }
        public byte UpperRightMuscleLevel { get; set; }
        public byte UpperLeftMuscleLevel { get; set; }
        public byte CenterMuscleLevel { get; set; }
        public byte BottomRightMuscleLevel { get; set; }
        public byte BottomLeftMuscleLevel { get; set; }
        public double UpperRightFatPercentage { get; set; }
        public double UpperRightFat { get; set; }
        public double UpperLeftFatPercentage { get; set; }
        public double UpperLeftFat { get; set; }
        public double CenterFatPercentage { get; set; }
        public double CenterFat { get; set; }
        public double BottomLeftFatPercentage { get; set; }
        public double BottomLeftFat { get; set; }
        public double BottomRightFatPercentage { get; set; }
        public double BottomRightFat { get; set; }
        public byte UpperRightFatLevel { get; set; }
        public byte UpperLeftFatLevel { get; set; }
        public byte CenterFatLevel { get; set; }
        public byte BottomRightFatLevel { get; set; }
        public byte BottomLeftFatLevel { get; set; }
        public double EI20kHzRightArm { get; set; }
        public double EI20kHzLefttArm { get; set; }
        public double EI20kHzBody { get; set; }
        public double EI20kHzRightLeg { get; set; }
        public double EI20kHzLefttLeg { get; set; }
        public double EI100kHzRightArm { get; set; }
        public double EI100kHzLeftArm { get; set; }
        public double EI100kHzBody { get; set; }
        public double EI100kHzRightLeg { get; set; }
        public double EI100kHzLeftLeg { get; set; }
        public short InBodyScore { get; set; }

        public virtual Student SIdNavigation { get; set; }
    }
}
