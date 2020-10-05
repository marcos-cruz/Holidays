using Bigai.Holidays.Core.Domain.Models.States;
using Bigai.Holidays.Shared.Domain.Enums.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bigai.Holidays.Core.Domain.Mappers.States
{
    internal static class StateMapper
    {
        /// <summary>
        /// This method maps the content to a list of states lists.
        /// </summary>
        /// <param name="content">The content read from a CSV file.</param>
        /// <returns>Returns a list of states lists.</returns>
        internal static List<List<State>> ToListOfStateList(this string[,] content)
        {
            List<List<State>> list = new List<List<State>>();
            if (content != null)
            {
                List<State> states;
                int items = content.GetLength(0);
                if (items > 0)
                {
                    const int itemsPerList = 64;
                    int tasks = 1;
                    int start;
                    int end = items;

                    if (items > itemsPerList)
                    {
                        tasks = (items / itemsPerList) + 1;
                        end = itemsPerList;
                    }

                    for (start = 0; tasks > 0; tasks--)
                    {
                        states = GetStatesFromCsv(content, start, end);
                        list.Add(states);

                        start = end;
                        end = (end + itemsPerList) > items ? items : (end += itemsPerList);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// This method maps the content to a list of states lists.
        /// </summary>
        /// <param name="content">The content read from a CSV file.</param>
        /// <returns>Returns a list of states lists.</returns>
        internal static async Task<List<List<State>>> ToListOfStatesListAsync(this string[,] content)
        {
            List<List<State>> list = new List<List<State>>();
            if (content != null)
            {
                List<State> states;
                int items = content.GetLength(0);
                if (items > 0)
                {
                    const int itemsPerList = 64;
                    int tasks = 1;
                    int start;
                    int end = items;

                    if (items > itemsPerList)
                    {
                        tasks = (items / itemsPerList) + 1;
                        end = itemsPerList;
                    }

                    for (start = 0; tasks > 0; tasks--)
                    {
                        states = await Task.Run(() => GetStatesFromCsv(content, start, end));
                        list.Add(states);

                        start = end;
                        end = (end + itemsPerList) > items ? items : (end += itemsPerList);
                    }
                }
            }

            return list;
        }

        private static List<State> GetStatesFromCsv(string[,] statesCsv, int start, int end)
        {
            List<State> states = null;

            if (statesCsv != null && statesCsv.GetLength(0) > 0)
            {
                states = new List<State>();

                try
                {
                    Guid userId = Guid.Parse("3332c0c3-c506-4ec2-beea-e7dd5c942038");

                    for (int i = start, j = end; i < j; i++)
                    {
                        Guid countryId = Guid.Empty;
                        var countryIsoCode = statesCsv[i, 1];
                        var stateIsoCode = statesCsv[i, 2];
                        var name = statesCsv[i, 3];

                        var state = State.CreateState(null, EntityStatus.Active, TypeProcess.Register, userId, countryId, countryIsoCode, stateIsoCode, name);
                        states.Add(state);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return states;
        }
    }
}
