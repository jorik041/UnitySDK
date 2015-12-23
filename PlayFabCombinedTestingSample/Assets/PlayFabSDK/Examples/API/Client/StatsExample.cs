using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

namespace PlayFab.Examples.Client
{
    public static class StatsExample
    {
        #region Controller Event Handling
        static StatsExample()
        {
            PfSharedControllerEx.RegisterEventMessage(PfSharedControllerEx.EventType.OnUserLogin, OnUserLogin);
            PfSharedControllerEx.RegisterEventMessage(PfSharedControllerEx.EventType.OnUserCharactersLoaded, OnUserCharactersLoaded);
        }

        public static void SetUp()
        {
            // The static constructor is called as a by-product of this call
        }

        private static void OnUserLogin(string playFabId, string characterId, PfSharedControllerEx.Api eventSourceApi, bool requiresFullRefresh)
        {
            GetUserStatistics();
        }

        private static void OnUserCharactersLoaded(string playFabId, string characterId, PfSharedControllerEx.Api eventSourceApi, bool requiresFullRefresh)
        {
            if (eventSourceApi != PfSharedControllerEx.Api.Client)
                return;

            // Cannot get character statistics on the client right now
        }
        #endregion Controller Event Handling

        #region User/Character stats API
        public static void GetUserStatistics()
        {
            var request = new GetUserStatisticsRequest();
            PlayFabClientAPI.GetUserStatistics(request, GetUserStatisticsCallback, PfSharedControllerEx.FailCallback("GetUserStatistics"));
        }
        private static void GetUserStatisticsCallback(GetUserStatisticsResult result)
        {
            PfSharedModelEx.globalClientUser.userStatistics = result.UserStatistics;
        }

        public static Action UpdateUserStatistics(string key, int value)
        {
            Action output = () =>
            {
                var request = new UpdateUserStatisticsRequest();
                request.UserStatistics = new Dictionary<string, int>();
                request.UserStatistics[key] = value;
                PlayFabClientAPI.UpdateUserStatistics(request, UpdateUserStatisticsCallback, PfSharedControllerEx.FailCallback("UpdateUserStatistics"));
            };
            return output;
        }

        private static void UpdateUserStatisticsCallback(UpdateUserStatisticsResult result)
        {
            var updatedStats = ((UpdateUserStatisticsRequest) result.Request).UserStatistics;

            foreach (var statPair in updatedStats)
                PfSharedModelEx.globalClientUser.userStatistics[statPair.Key] = statPair.Value;
        }
        #endregion User/Character stats API
    }
}
