using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VitalCapacityCoreV2.GameWindow;
using VitalCapacityV2.Summer.GameSystem.FreeSqlHelper;
using VitalCapacityV2.Summer.GameSystem.GameModel;

namespace VitalCapacityCoreV2.GameWindowSys
{
    public class SystemSettingWindowSys
    {
        private readonly IFreeSql freeSql = FreeSqlHelper.Sqlite;
        private SystemSettingWindow SystemSettingWindow = null;

        /// <summary>
        ///
        /// </summary>
        public void ShowSystemSettingWindow()
        {
            SystemSettingWindow = new SystemSettingWindow();
            SystemSettingWindow.ShowDialog();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public SportProjectInfos LoadingInitData()
        {
            return freeSql.Select<SportProjectInfos>().ToOne();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="roundCount"></param>
        /// <param name="bestMethod"></param>
        /// <param name="testMethod"></param>
        /// <param name="floatType"></param>
        /// <param name="sportProjectInfos"></param>
        /// <returns></returns>
        public bool SaveSportProjectsSetting(string projectName, int roundCount, int bestMethod, int testMethod, int floatType, SportProjectInfos sportProjectInfos)
        {
            try
            {
                freeSql.Delete<SportProjectInfos>(sportProjectInfos).ExecuteAffrows();
                SportProjectInfos sport = new SportProjectInfos()
                {
                    SortId = sportProjectInfos.SortId,
                    BestScoreMode = bestMethod,
                    CreateTime = sportProjectInfos.CreateTime,
                    FloatType = floatType,
                    Id = sportProjectInfos.Id,
                    IsRemoved = sportProjectInfos.IsRemoved,
                    Name = projectName,
                    RoundCount = roundCount,
                    TestMethod = testMethod,
                    Type = floatType,
                    TurnsNumber0 = sportProjectInfos.TurnsNumber0,
                    TurnsNumber1 = sportProjectInfos.TurnsNumber1,
                };
                var res = freeSql.InsertOrUpdate<SportProjectInfos>().SetSource(sport).IfExistsDoNothing().ExecuteAffrows();
                if (res > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}