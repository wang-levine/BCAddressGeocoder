using System;
using System.Collections.Generic;

namespace BCAddressGeocoder.Models
{
    public class Properties
    {
        public string FullAddress { get; set; }
        public int Score { get; set; }
        public string MatchPrecision { get; set; }
        public int PrecisionPoints { get; set; }
        public List<Fault> Faults { get; set; }
        public string SiteName { get; set; }
        public string UnitDesignator { get; set; }
        public string UnitNumber { get; set; }
        public string UnitNumberSuffix { get; set; }
        public string CivicNumber { get; set; }
        public string CivicNumberSuffix { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public bool IsStreetTypePrefix { get; set; }
        public string StreetDirection { get; set; }
        public bool IsStreetDirectionPrefix { get; set; }
        public string StreetQualifier { get; set; }
        public string LocalityName { get; set; }
        public string LocalityType { get; set; }
        public string ElectoralArea { get; set; }
        public string ProvinceCode { get; set; }
        public string LocationPositionalAccuracy { get; set; }
        public string LocationDescriptor { get; set; }
        public string SiteID { get; set; }
        public int BlockID { get; set; }
        public string FullSiteDescriptor { get; set; }
        public string AccessNotes { get; set; }
        public string SiteStatus { get; set; }
        public DateTime SiteRetireDate { get; set; }
        public DateTime ChangeDate { get; set; }
        public bool IsOfficial { get; set; }
    }
}