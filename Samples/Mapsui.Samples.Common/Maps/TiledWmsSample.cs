﻿using System;
using System.Linq;
using BruTile;
using BruTile.Predefined;
using BruTile.Web;
using BruTile.Wmsc;
using Mapsui.Layers;
using Attribution = BruTile.Attribution;

namespace Mapsui.Samples.Common.Maps
{
    /// <summary>
    /// An ordinary WMS service called through a tiled schema (WMS-C) 
    /// </summary>
    public static class TiledWmsSample
    {
        public static Map CreateMap()
        {
            var map = new Map();
            map.Layers.Add(CreateLayer());
            return map;
        }

        public static ILayer CreateLayer()
        {
            return new TileLayer(new GeodanWorldWmsTileSource()) {Name = "WMS called as WMS-C"};
        }
    }

    public class GeodanWorldWmsTileSource : ITileSource
    {
        public GeodanWorldWmsTileSource()
        {
            var schema = new WkstNederlandSchema {Srs = "EPSG:28992"};
            schema.Format = "image/png";
            Provider = new HttpTileProvider(CreateWmsRequest(schema));
            Schema = schema;
        }

        public byte[] GetTile(TileInfo tileInfo)
        {
            return Provider.GetTile(tileInfo);
        }

        private static WmscRequest CreateWmsRequest(ITileSchema schema)
        {
            const string url = "http://geodata.nationaalgeoregister.nl/omgevingswarmte/wms?SERVICE=WMS&VERSION=1.1.1";
            return new WmscRequest(new Uri(url), schema, new[] { "koudegeslotenwkobuurt" }.ToList(), new string[0].ToList());
        }

        public ITileProvider Provider { get; }
        public ITileSchema Schema { get; }

        public string Name => "Potentiele Koude Gesloten WKO Buurt (PDOK)";
        public Attribution Attribution { get; } = new Attribution();
    }
}