﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2PAO.API.Data;
using GW2PAO.API.Data.Entities;

namespace GW2PAO.API.Services.Interfaces
{
    public interface IZoneService
    {
        /// <summary>
        /// Initializes the zone service
        /// </summary>
        void Initialize();

        /// <summary>
        /// Retrieves the continent information for the given continent ID
        /// </summary>
        /// <param name="mapId">The ID of a continent</param>
        /// <returns>The continent data</returns>
        Data.Entities.Continent GetContinent(int continentId);

        /// <summary>
        /// Retrieves the continent information for the given map ID
        /// </summary>
        /// <param name="mapId">The ID of a zone</param>
        /// <returns>The continent data</returns>
        Data.Entities.Continent GetContinentByMap(int mapId);

        /// <summary>
        /// Retrieves continent information for all continents
        /// </summary>
        /// <returns>a collection of continents</returns>
        IEnumerable<Data.Entities.Continent> GetContinents();

        /// <summary>
        /// Retrieves the map information for the given map ID
        /// </summary>
        /// <param name="mapId">The ID of a zone</param>
        /// <returns>The map data</returns>
        Data.Entities.Map GetMap(int mapId);

        /// <summary>
        /// Retrieves map information using the provided continent coordinates,
        /// or null if the coordinates do not fall into any current zone
        /// </summary>
        /// <param name="continentId">ID of the continent that the coordinates are for</param>
        /// <param name="continentCoordinates">Continent coordinates to use</param>
        /// <returns>The map data, or null if not found</returns>
        Data.Entities.Map GetMap(int continentId, Point continentCoordinates);

        /// <summary>
        /// Retrieves a collection of ZoneItems located in the zone with the given mapID
        /// </summary>
        /// <param name="mapId">The mapID of the zone to retrieve zone items for</param>
        /// <returns>a collection of ZoneItems located in the zone with the given mapID</returns>
        IEnumerable<ZoneItem> GetZoneItems(int mapId);

        /// <summary>
        /// Retrieves a collection of zone items located in the given continent
        /// </summary>
        /// <param name="continentId">ID of the continent</param>
        /// <returns></returns>
        IEnumerable<ZoneItem> GetZoneItemsByContinent(int continentId);

        /// <summary>
        /// Retrieves the name of the zone using the given mapID
        /// </summary>
        /// <param name="mapId">The mapID of the zone to retrieve the name for</param>
        /// <returns>the name of the zone</returns>
        string GetZoneName(int mapId);
    }
}
