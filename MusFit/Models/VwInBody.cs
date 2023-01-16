using System;
using System.Collections.Generic;

#nullable disable

namespace MusFit.Models
{
    public partial class VwInBody
    {
        public string SName { get; set; }
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
        public short InBodyScore { get; set; }
    }
}
