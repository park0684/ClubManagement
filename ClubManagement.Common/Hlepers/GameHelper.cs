using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClubManagement.Common.Hlepers
{
    public static class GameHelper
    {
        public readonly static Dictionary<int, string> MatchType = new Dictionary<int, string>()
        {
            {1,"정기전" },
            {2,"비정기전" },
            {3,"이벤트" }
        };
        public readonly static Dictionary<int, string> PlayerType = new Dictionary<int, string>()
        {
            {1, "회원" },
            {2, "게스트" }
        };
        public readonly static Dictionary<int, string> GameType = new Dictionary<int, string>()
        {
            {1, "개인전" },
            {2, "단체전" }
        };
        public static string GetMatchType(int keyCode)
        {
            return CodeHelper.GetStatusString(MatchType, keyCode);
        }
        public static string GetPlayerType(int keycode)
        {
            return CodeHelper.GetStatusString(PlayerType, keycode);
        }
        public static string GetGameType(int keycode)
        {
            return CodeHelper.GetStatusString(GameType, keycode);
        }

        public static int GetMatchTypeCode(string valueString)
        {
            return CodeHelper.GetStatusCode(MatchType, valueString);
        }

        public static int GetPlayerTypeCode(string valueString)
        {
            return CodeHelper.GetStatusCode(MatchType, valueString);
        }
        public static int GetGameTypeCode(string valueString)
        {
            return CodeHelper.GetStatusCode(GameType, valueString);
        }
    }
}
