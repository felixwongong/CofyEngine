using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

namespace CM.Network.LobbyUtil
{
    public class LobbyQueryFactory
    {
        private readonly List<QueryFilter> _filters;
        private readonly QueryLobbiesOptions _options;
        private readonly List<QueryOrder> _orders;

        public LobbyQueryFactory(int resultCnt)
        {
            _options = new QueryLobbiesOptions();
            _filters = new List<QueryFilter>();
            _orders = new List<QueryOrder>();
            _options.Count = resultCnt;
        }

        public LobbyQueryFactory Compare(QueryFilter.FieldOptions target, QueryFilter.OpOptions op, string val)
        {
            _filters.Add(
                new QueryFilter(
                    target,
                    op: op,
                    value: val
                )
            );
            return this;
        }

        public LobbyQueryFactory Greater<T>(QueryFilter.FieldOptions target, T val, bool inclusive = true)
        {
            var op = inclusive ? QueryFilter.OpOptions.GE : QueryFilter.OpOptions.GT;
            return Compare(target, op, val.ToString());
        }

        public LobbyQueryFactory LessThan<T>(QueryFilter.FieldOptions target, T val, bool inclusive = true)
        {
            var op = inclusive ? QueryFilter.OpOptions.LE : QueryFilter.OpOptions.LT;
            return Compare(target, op, val.ToString());
        }

        public LobbyQueryFactory InRange<T>(QueryFilter.FieldOptions target, T lower, T upper, bool inclusive = true)
        {
            return Greater(target, lower, inclusive).LessThan(target, upper, inclusive);
        }

        public LobbyQueryFactory Equal<T>(QueryFilter.FieldOptions target, T val)
        {
            return Compare(target, QueryFilter.OpOptions.EQ, val.ToString());
        }

        public LobbyQueryFactory NotEqual<T>(QueryFilter.FieldOptions target, T val)
        {
            return Compare(target, QueryFilter.OpOptions.NE, val.ToString());
        }


        public QueryLobbiesOptions GetQuery()
        {
            _options.Filters = _filters;
            _options.Order = _orders;
            return _options;
        }
    }
}