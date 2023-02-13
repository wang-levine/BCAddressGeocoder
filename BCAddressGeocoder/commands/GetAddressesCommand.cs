using System.Collections.Generic;
using System.Management.Automation;
using BCAddressGeocoder.Models;
using BCAddressGeocoder.Service;

namespace BCAddressGeocoder.Commands
{
    /// <summary>
    /// <para type="synopsis">Geocode an address</para>
    /// <para type="description">Represents the set of geocoded and standardized sites and intersections whose address best matches a given query address.</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Addresses")]
    [OutputType(typeof(Address))]
    public class GetAddressesCommand : PSCmdlet
    {
        /// <summary>
        /// <para type="description">Civic or intersection address as a single string.</para>
        /// </summary>
        [Parameter(Position = 0)]
        public string? AddressString { get; set; }

        /// <summary>
        /// <para type="description">Describes the nature of the address location.</para>
        /// </summary>
        [Parameter(Position = 1)]
        [ValidateSet("any", "accessPoint", "frontDoorPoint", "parcelPoint", "rooftopPoint", "routingPoint")]
        public string LocationDescriptor { get; set; } = "any";

        /// <summary>
        /// <para type="description">The maximum number of search results to return.</para>
        /// </summary>
        [Parameter(Position = 2)]
        public int MaxResults { get; set; } = 1;

        /// <summary>
        /// <para type="description">accessPoint interpolation method.</para>
        /// </summary>
        [Parameter(Position = 3)]
        [ValidateSet("adaptive", "linear", "none")]
        public string Interpolation { get; set; } = "adaptive";

        /// <summary>
        /// <para type="description">If true, include unmatched address details such as site name in results.</para>
        /// </summary>
        [Parameter(Position = 4)]
        public bool Echo { get; set; } = true;

        /// <summary>
        /// <para type="description">If true, include only basic match and address details in results. Not supported for shp, csv, and gml formats.</para>
        /// </summary>
        [Parameter(Position = 5)]
        public bool Brief { get; set; } = false;

        /// <summary>
        /// <para type="description">If true, addressString is expected to contain a partial address that requires completion. Not supported for shp, csv, gml formats.</para>
        /// </summary>
        [Parameter(Position = 6)]
        public bool AutoComplete { get; set; } = false;

        /// <summary>
        /// <para type="description">The distance to move the accessPoint away from the curb and towards the inside of the parcel (in metres). Ignored if locationDescriptor not set to accessPoint.</para>
        /// </summary>
        [Parameter(Position = 7)]
        public int SetBack { get; set; } = 0;

        /// <summary>
        /// <para type="description">The EPSG code of the spatial reference system (SRS) to use for output geometries.</para>
        /// </summary>
        [Parameter(Position = 8)]
        [ValidateSet("4326", "4269", "3005", "26907", "26908", "26909", "26910", "26911")]
        public int OutputSRS { get; set; } = 4326;

        /// <summary>
        /// <para type="description">The minimum score required for a match to be returned.</para>
        /// </summary>
        [Parameter(Position = 9)]
        public int MinScore { get; set; } = 1;

        /// <summary>
        /// <para type="description">Example: street,locality.  A comma separated list of individual match precision levels to include in results.</para>
        /// </summary>
        [Parameter(Position = 10)]
        public string? MatchPrecision { get; set; }

        /// <summary>
        /// <para type="description">Example: street,locality.  A comma separated list of individual match precision levels to exclude from results.</para>
        /// </summary>
        [Parameter(Position = 11)]
        public string? MatchPrecisionNot { get; set; }

        /// <summary>
        /// <para type="description">A string containing the name of the building, facility, or institution (e.g., Duck Building, Casa Del Mar, Crystal Garden, Bluebird House).</para>
        /// </summary>
        [Parameter(Position = 12)]
        public string? SiteName { get; set; }

        /// <summary>
        /// <para type="description">The type of unit within a house or building.</para>
        /// </summary>
        [Parameter(Position = 13)]
        [ValidateSet("APT", "BLDG", "BSMT", "FLR", "LOBBY", "LWR", "PAD", "PH", "REAR", "RM", "SIDE", "SITE", "SUITE", "TH", "UNIT", "UPPR")]
        public string? UnitDesignator { get; set; }

        /// <summary>
        /// <para type="description">The number of the unit, suite, or apartment within a house or building.</para>
        /// </summary>
        [Parameter(Position = 14)]
        public string? UnitNumber { get; set; }

        /// <summary>
        /// <para type="description">A letter that follows the unit number as in Unit 1A or Suite 302B.</para>
        /// </summary>
        [Parameter(Position = 15)]
        public string? UnitNumberSuffix { get; set; }

        /// <summary>
        /// <para type="description">The official number assigned to a site by an address authority.</para>
        /// </summary>
        [Parameter(Position = 16)]
        public string? CivicNumber { get; set; }

        /// <summary>
        /// <para type="description">A letter or fraction that follows the civic number (e.g., the A in 1039A Bledsoe St).</para>
        /// </summary>
        [Parameter(Position = 17)]
        public string? CivicNumberSuffix { get; set; }

        /// <summary>
        /// <para type="description">The official name of the street as assigned by an address authority (e.g., the Douglas in 1175 Douglas Street).</para>
        /// </summary>
        [Parameter(Position = 18)]
        public string? StreetName { get; set; }

        /// <summary>
        /// <para type="description">The type of street as assigned by a municipality (e.g., the ST in 1175 DOUGLAS St).</para>
        /// </summary>
        [Parameter(Position = 19)]
        public string? StreetType { get; set; }

        /// <summary>
        /// <para type="description">The abbreviated compass direction as defined by Canada Post and B.C. civic addressing authorities.</para>
        /// </summary>
        [Parameter(Position = 20)]
        [ValidateSet("N", "S", "E", "W", "O", "NE", "NO", "NW", "SE", "SO", "SW")]
        public string? StreetDirection { get; set; }

        /// <summary>
        /// <para type="description">Example: the Bridge in Johnson St Bridge. The qualifier of a street name.</para>
        /// </summary>
        [Parameter(Position = 21)]
        public string? StreetQualifier { get; set; }

        /// <summary>
        /// <para type="description">The name of the locality assigned to a given site by an address authority.</para>
        /// </summary>
        [Parameter(Position = 22)]
        public string? LocalityName { get; set; }

        /// <summary>
        /// <para type="description">The ISO 3166-2 Sub-Country Code. The code for British Columbia is BC.</para>
        /// </summary>
        [Parameter(Position = 23)]
        public string ProvinceCode { get; set; } = "BC";

        /// <summary>
        /// <para type="description">A comma separated list of locality names that matched addresses must belong to. For example, setting localities to Nanaimo only returns addresses in Nanaimo</para>
        /// </summary>
        [Parameter(Position = 24)]
        public string? Localities { get; set; }

        /// <summary>
        /// <para type="description">A comma-separated list of localities to exclude from the search.</para>
        /// </summary>
        [Parameter(Position = 25)]
        public string? NotLocalities { get; set; }

        /// <summary>
        /// <para type="description">Example: -126.07929,49.7628,-126.0163,49.7907.  A bounding box (xmin,ymin,xmax,ymax) that limits the search area.</para>
        /// </summary>
        [Parameter(Position = 26)]
        public string? Bbox { get; set; }

        /// <summary>
        /// <para type="description">Example: -124.0165926,49.2296251 .  The coordinates of a centre point (x,y) used to define a bounding circle that will limit the search area. This parameter must be specified together with 'maxDistance'.</para>
        /// </summary>
        [Parameter(Position = 27)]
        public string? Centre { get; set; }

        /// <summary>
        /// <para type="description">The maximum distance (in metres) to search from the given point.  If not specified, the search distance is unlimited.</para>
        /// </summary>
        [Parameter(Position = 28)]
        public int? MaxDistance { get; set; }

        /// <summary>
        /// <para type="description">If true, uses supplied parcelPoint to derive an appropriate accessPoint.</para>
        /// </summary>
        [Parameter(Position = 29)]
        public bool? Extrapolate { get; set; }

        /// <summary>
        /// <para type="description">The coordinates of a point (x,y) known to be inside the parcel containing a given address.</para>
        /// </summary>
        [Parameter(Position = 30)]
        public string? ParcelPoint { get; set; }

        protected override void ProcessRecord()
        {
            var queryParams = new Dictionary<string, object>
            {
                { "addressString", AddressString },
                { "locationDescriptor", LocationDescriptor },
                { "maxResults", MaxResults },
                { "interpolation", Interpolation },
                { "echo", Echo },
                { "brief", Brief },
                { "autoComplete", AutoComplete },
                { "setBack", SetBack },
                { "outputSRS", OutputSRS },
                { "minScore", MinScore },
                { "matchPrecision", MatchPrecision },
                { "matchPrecisionNot", MatchPrecisionNot },
                { "siteName", SiteName },
                { "unitDesignator", UnitDesignator },
                { "unitNumber", UnitNumber },
                { "unitNumberSuffix", UnitNumberSuffix },
                { "civicNumber", CivicNumber },
                { "civicNumberSuffix", CivicNumberSuffix },
                { "streetName", StreetName },
                { "streetType", StreetType },
                { "streetDirection", StreetDirection },
                { "streetQualifier", StreetQualifier },
                { "localityName", LocalityName },
                { "provinceCode", ProvinceCode },
                { "localities", Localities },
                { "notLocalities", NotLocalities },
                { "bbox", Bbox },
                { "centre", Centre },
                { "maxDistance", MaxDistance },
                { "extrapolate", Extrapolate },
                { "parcelPoint", ParcelPoint },
            };

            var response = AddressService.GetAddresses(queryParams).GetAwaiter().GetResult();
            foreach (var address in response.Addresses)
            {
                WriteObject(new
                {
                    address.Type,
                    GeometryType = address.Geometry.Type,
                    CRS = $"{address.Geometry.Crs.Type}:{address.Geometry.Crs.Properties.Code}",
                    Coordinates = $"{address.Geometry.Coordinates[1]}, {address.Geometry.Coordinates[0]}",
                    address.Properties.FullAddress,
                    address.Properties.Score,
                    address.Properties.MatchPrecision,
                    address.Properties.PrecisionPoints,
                    address.Properties.SiteName,
                    address.Properties.UnitDesignator,
                    address.Properties.UnitNumber,
                    address.Properties.UnitNumberSuffix,
                    address.Properties.CivicNumber,
                    address.Properties.CivicNumberSuffix,
                    address.Properties.StreetName,
                    address.Properties.StreetType,
                    address.Properties.IsStreetTypePrefix,
                    address.Properties.StreetDirection,
                    address.Properties.IsStreetDirectionPrefix,
                    address.Properties.StreetQualifier,
                    address.Properties.LocalityName,
                    address.Properties.LocalityType,
                    address.Properties.ElectoralArea,
                    address.Properties.ProvinceCode,
                    address.Properties.LocationPositionalAccuracy,
                    address.Properties.LocationDescriptor,
                    address.Properties.SiteID,
                    address.Properties.BlockID,
                    address.Properties.FullSiteDescriptor,
                    address.Properties.AccessNotes,
                    address.Properties.SiteStatus,
                    SiteRetireDate = address.Properties.SiteRetireDate?.ToString("yyyy-MM-dd"),
                    ChangeDate = address.Properties.ChangeDate?.ToString("yyyy-MM-dd"),
                    address.Properties.IsOfficial,
                });
            }
        }
    }
}