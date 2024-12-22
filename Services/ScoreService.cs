using table_tennis_backend.Services;
using table_tennis_backend.Const;
using table_tennis_backend.Dtos.Player;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using table_tennis_backend.Dtos.Result;

public class ScoreService : IScoreService
{
    private readonly IResultService _resultService;

    public ScoreService(IResultService resultService)
    {
        _resultService = resultService;
    }

    public async Task<PlayerStats> GetPlayerStats(int idA, int idB, PlayerComparisonTypeEnum type)
    {
        var param = new GetAllReqDto { Player_Id_A_1 = idA, Player_Id_B_1 = idB };
        var list = await _resultService.GetResultRanking(param);

        if (type == PlayerComparisonTypeEnum.Recent)
        {
            // 取最前面 10 筆
            list = list.Take(10).ToList();
        }


        return new PlayerStats
        {
            WinRate = Math.Round(CalculateWinRate(list), 2),
            GameWinRate = Math.Round(CalculateGameWinRate(list), 2),
            AverageScore = Math.Round(CalculateAverageScore(list), 2),
            CriticalGameWinRate = Math.Round(CalculateCriticalGameWinRate(list), 2),
            LongestWinStreak = CalculateLongestWinStreak(list), // 因為是整數，無需四捨五入
            CriticalPointRate = Math.Round(CalculateCriticalPointRate(list), 2),
        };
    }

    // 計算勝率
    private double CalculateWinRate(List<GetRankingResDto> results)
    {
        int totalMatches = results.Count;
        int totalWins = results.Count(result => result.ScoreA > result.ScoreB);

        return totalMatches == 0 ? 0 : (double)totalWins / totalMatches;
    }

    // 計算局數勝率
    private double CalculateGameWinRate(List<GetRankingResDto> results)
    {
        int totalGames = results.Sum(result => result.ResultItemList.Count);
        int gamesWon = results.Sum(result => result.ResultItemList.Count(item => item.ScoreA > item.ScoreB));

        return totalGames == 0 ? 0 : (double)gamesWon / totalGames;
    }

    // 計算平均得分
    private double CalculateAverageScore(List<GetRankingResDto> results)
    {
        int totalGames = results.Sum(result => result.ResultItemList.Count);
        int totalScoreA = results.Sum(result => result.ResultItemList.Sum(item => item.ScoreA));

        return totalGames == 0 ? 0 : (double)totalScoreA / totalGames;
    }

    // 計算關鍵局勝率
    private double CalculateCriticalGameWinRate(List<GetRankingResDto> results)
    {
        int criticalGames = results.Count;
        int criticalGameWins = results.Count(result => result.ResultItemList.Last().ScoreA > result.ResultItemList.Last().ScoreB);

        return criticalGames == 0 ? 0 : (double)criticalGameWins / criticalGames;
    }

    // 計算連勝次數
    private int CalculateLongestWinStreak(List<GetRankingResDto> results)
    {
        int maxStreak = 0;
        int currentStreak = 0;

        foreach (var result in results)
        {
            foreach (var item in result.ResultItemList)
            {
                if (item.ScoreA > item.ScoreB)
                {
                    currentStreak++;
                    maxStreak = Math.Max(maxStreak, currentStreak);
                }
                else
                {
                    currentStreak = 0;
                }
            }
        }

        return maxStreak;
    }

    // 計算關鍵分表現
    private double CalculateCriticalPointRate(List<GetRankingResDto> results)
    {
        int criticalGames = 0; // 符合條件的局數總數
        int criticalWins = 0;  // 玩家 A 獲勝的局數

        foreach (var result in results)
        {
            foreach (var item in result.ResultItemList)
            {
                // 只記錄比分達到 10 分以上的局數
                if (item.ScoreA >= 10 && item.ScoreB >= 10)
                {
                    criticalGames++; // 累計符合條件的局數

                    // 判斷玩家 A 是否獲勝
                    if (item.ScoreA > item.ScoreB)
                    {
                        criticalWins++;
                    }
                }
            }
        }

        // 計算並返回關鍵分表現
        return criticalGames == 0 ? 0 : (double)criticalWins / criticalGames;
    }

}
