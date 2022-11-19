﻿using dijkstra_api.Data;
using dijkstra_api.Services.Interfaces;

namespace dijkstra_api.Services
{
    public class FileMapLoader : IMapLoader
    {
        private List<List<int>> dangerLevels= new List<List<int>>();
        private List<List<ETileType>> tileTypes = new List<List<ETileType>>();


        public FileMapLoader(IConfiguration config) {
            var mapBase = config.GetSection("MapInputs");
            var dangerLevelFile = mapBase.GetValue<string>("DangerLevel");
            var tileTypeFile = mapBase.GetValue<string>("TileType");
            if(dangerLevelFile == null || tileTypeFile == null)
            {
                throw new ApplicationException("MapInputs.DangerLevel or MapInputs.TileType not set.");
            }
            dangerLevels = new List<List<int>>();

            foreach(var line in File.ReadAllLines(dangerLevelFile))
            {
                var tmp = new List<int>();
                foreach(var ch in line)
                {
                    if (!Char.IsAsciiDigit(ch))
                    {
                        throw new ApplicationException("MapInputs.DangerLevel contains non numeric chars");
                    }
                    tmp.Add(ch - '0');
                }
                dangerLevels.Add(tmp);
            }


            tileTypes = new List<List<ETileType>>();

            foreach (var line in File.ReadAllLines(tileTypeFile))
            {
                var tmp = new List<ETileType>();
                foreach (var ch in line)
                {
                    if (ch == 'L')
                    {
                        tmp.Add(ETileType.LAND);
                    } else if (ch == 'W')
                    {
                        tmp.Add(ETileType.WATER);
                    } else
                    {
                        throw new ApplicationException("MapInputs.TileTypes contains char that is not 'L' or 'W'");
                    }
                }
                tileTypes.Add(tmp);
            }
        }

        public int? getDangerLevel(Point point)
        {
            if (point.y < dangerLevels.Count && point.x < dangerLevels[point.y].Count)
            {
                return dangerLevels[point.y][point.x];
            }
            else
            {
                return null;
            }
        }

        public ETileType? getTileType(Point point)
        {
            if(point.y < tileTypes.Count && point.x < tileTypes[point.y].Count)
            {
                return tileTypes[point.y][point.x];
            } else
            {
                return null;
            }
        }
    }
}
