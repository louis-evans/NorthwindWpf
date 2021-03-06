﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace NorthwindWpf.Data.Models
{
    public class AddressFindResult
    {
        [JsonProperty("postcode")]
        public string PostCode { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("addresses")]
        public IEnumerable<Address> Addresses { get; set; }

        public class Address
        {
            [JsonProperty("formatted_address")]
            public IEnumerable<string> FormattedAddress { get; set; }

            [JsonProperty("thoroughfare")]
            public string Thoroughfare { get; set; }

            [JsonProperty("building_name")]
            public string BuildingName { get; set; }

            [JsonProperty("building_number")]
            public string BuildingNumber { get; set; }

            [JsonProperty("sub_building_name")]
            public string SubBuildingName { get; set; }

            [JsonProperty("sub_building_number")]
            public string SubBuildingNumber { get; set; }

            [JsonProperty("line_1")]
            public string Line1 { get; set; }

            [JsonProperty("line_2")]
            public string Line2 { get; set; }

            [JsonProperty("line_3")]
            public string Line3 { get; set; }

            [JsonProperty("line_4")]
            public string Line4 { get; set; }

            [JsonProperty("locality")]
            public string Locality { get; set; }

            [JsonProperty("town_or_city")]
            public string TownCity { get; set; }

            [JsonProperty("county")]
            public string County { get; set; }

            [JsonProperty("district")]
            public string District { get; set; }

            [JsonProperty("country")]
            public string Country { get; set; }

            public string PostCode { get; set; }
        }
    }

}
