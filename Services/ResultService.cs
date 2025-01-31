using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Result;
using ResultItemDto = table_tennis_backend.Dtos.ResultItem.GetResDto;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;
using table_tennis_backend.Const;


public class ResultService : IResultService
{
    private readonly IResultRepository _repository;
    private readonly IResultItemRepository _repository_result_item;
    private readonly IPlayerRepository _repository_player;
    private readonly IPlayerScoreHistoryRepository _repository_playerScoreHistory;
    private readonly IDoublePlayerRepository _repository_doublePlayer;
    public ResultService(
        IResultRepository repository,
        IResultItemRepository repository_result_item,
        IPlayerRepository repository_player,
        IDoublePlayerRepository repository_doublePlayer,
        IPlayerScoreHistoryRepository repository_playerScoreHistory)
    {
        _repository = repository;
        _repository_result_item = repository_result_item;
        _repository_player = repository_player;
        _repository_playerScoreHistory = repository_playerScoreHistory;
        _repository_doublePlayer = repository_doublePlayer;
    }

    public async Task AddResult(AddReqDto[] req)
    {

        var result = new List<Result>();

        foreach (var item in req)
        {
            int currentRound = item.Round;
            int matchId = 1; // 比赛 ID 从 1 开始

            while (currentRound >= 2) // 至少需要两队才能进行比赛
            {
                int matchesInThisRound = currentRound / 2;
                for (int i = 0; i < matchesInThisRound; i++)
                {
                    result.Add(new Result
                    {
                        RoundIndex = matchId++, // 分配比赛 ID 并递增
                        Round = currentRound,
                        Event_Id = item.Event_Id
                    });
                }
                currentRound /= 2; // 减少到下一轮
                matchId = 1; // 重置比赛 ID
            }
        }

        await _repository.CreateResult(result);
        return;
    }

    public async Task AddResultRanking(AddRankingReqDto[] req)
    {
        var sortedReq = req.OrderBy(item => item.ResultDateTime).ToArray();

        var playerList = await _repository_player.ReadAllPlayer();
        var doublePlayerList = await _repository_doublePlayer.ReadAllDoublePlayer();

        var result = new List<Result>();
        var resultItem = new List<ResultItem>();
        var playerScoreHistory = new List<PlayerScoreHistory>();
        var scoreManager = new ScoreManager();


        // TODO: 這個做法會有高並發先後順序問題，需要改正
        int maxResultId = await _repository.FindMaxResultId();


        foreach (var item in sortedReq)
        {
            int winsA = 0;
            int winsB = 0;

            maxResultId++;

            # region 玩家局數分數記錄
            foreach (var subitem in item.ResultItemList)
            {
                resultItem.Add(new ResultItem
                {
                    Result_Id = maxResultId,
                    MatchIndex = subitem.MatchIndex,
                    ScoreA = subitem.ScoreA,
                    ScoreB = subitem.ScoreB
                });

                if (subitem.ScoreA > subitem.ScoreB)
                {
                    winsA++;
                }
                else if (subitem.ScoreB > subitem.ScoreA)
                {
                    winsB++;
                }
            }
            # endregion

            # region 計算玩家積分
            int playerAScore = item.SubEventType == SubEventTypeEnum.Single
            ? playerList.FirstOrDefault(player => player.Id == item.Player_Id_A_1)?.Score ?? 0
            : doublePlayerList.FirstOrDefault(doublePlayer => doublePlayer.Id == item.DoublePlayer_Id_A)?.Score ?? 0;
            int playerBScore = item.SubEventType == SubEventTypeEnum.Single
            ? playerList.FirstOrDefault(player => player.Id == item.Player_Id_B_1)?.Score ?? 0
            : doublePlayerList.FirstOrDefault(doublePlayer => doublePlayer.Id == item.DoublePlayer_Id_B)?.Score ?? 0;

            playerScoreHistory.Add(new PlayerScoreHistory
            {
                Result_Id = maxResultId,
                Player_Id_A = item.Player_Id_A_1,
                Player_Id_B = item.Player_Id_B_1,
                InitialScore_A = playerAScore,
                InitialScore_B = playerBScore,
            });

            (int scoreA, int scoreB) = scoreManager.UpdateScores(playerAScore, playerBScore, winsA > winsB ? 'A' : 'B');
            # endregion

            result.Add(new Result
            {
                Id = maxResultId,
                Event_Id = item.Event_Id,
                Round = 0,
                RoundIndex = 0,
                Player_Id_A_1 = item.Player_Id_A_1,
                Player_Id_A_2 = item.Player_Id_A_2,
                Player_Id_B_1 = item.Player_Id_B_1,
                Player_Id_B_2 = item.Player_Id_B_2,
                DoublePlayer_Id_A = item.DoublePlayer_Id_A,
                DoublePlayer_Id_B = item.DoublePlayer_Id_B,
                SubEventType = item.SubEventType,
                ResultDateTime = item.ResultDateTime,
                ScoreA = winsA,
                ScoreB = winsB,
                PlayerScoreA = scoreA,
                PlayerScoreB = scoreB
            });

            if (item.SubEventType == SubEventTypeEnum.Single)
            {
                var playerAToModify = playerList.FirstOrDefault(player => player.Id == item.Player_Id_A_1);
                var playerBToModify = playerList.FirstOrDefault(player => player.Id == item.Player_Id_B_1);

                if (playerAToModify != null)
                {
                    playerAToModify.Score = scoreA;
                    playerAToModify.UpdateDateTime = DateTimeOffset.UtcNow;
                    playerAToModify.LatestResultDateTime = item.ResultDateTime;
                }

                if (playerBToModify != null)
                {
                    playerBToModify.Score = scoreB;
                    playerBToModify.UpdateDateTime = DateTimeOffset.UtcNow;
                    playerBToModify.LatestResultDateTime = item.ResultDateTime;
                }
            }
            else
            {
                var doublePlayerAToModify = doublePlayerList.FirstOrDefault(doublePlayer => doublePlayer.Id == item.DoublePlayer_Id_A);
                var doublePlayerBToModify = doublePlayerList.FirstOrDefault(doublePlayer => doublePlayer.Id == item.DoublePlayer_Id_B);

                if (doublePlayerAToModify != null)
                {
                    doublePlayerAToModify.Score = scoreA;
                    doublePlayerAToModify.UpdateDateTime = DateTimeOffset.UtcNow;
                    doublePlayerAToModify.LatestResultDateTime = item.ResultDateTime;
                }

                if (doublePlayerBToModify != null)
                {
                    doublePlayerBToModify.Score = scoreB;
                    doublePlayerBToModify.UpdateDateTime = DateTimeOffset.UtcNow;
                    doublePlayerBToModify.LatestResultDateTime = item.ResultDateTime;
                }
            }
        }
        await _repository.CreateResult(result);
        await _repository_result_item.CreateResultItem(resultItem);
        await _repository_playerScoreHistory.CreatePlayerScore(playerScoreHistory);
        await _repository_player.UpdatePlayers(playerList);
        await _repository_doublePlayer.UpdateDoublePlayers(doublePlayerList);
        return;
    }

    public async Task<List<GetResDto>> GetAllResult(GetAllReqDto req)
    {

        var results = await _repository.ReadAllResult(req.Event_Id, req.Event_Type, req.SubEventType, req.StartDate, req.EndDate, req.Player_Id_A_1, req.Player_Id_B_1);
        return results
        .OrderBy(r => r.Id)
        .Select(r => new GetResDto
        {
            Id = r.Id,
            Event_Id = r.Event_Id,
            Event_Name = r.Event.Name,
            Round = r.Round,
            Player_NameA1 = r.PlayerA1?.Name,
            Player_NameA2 = r.PlayerA2?.Name,
            Player_NameB1 = r.PlayerB1?.Name,
            Player_NameB2 = r.PlayerB2?.Name,
            Player_Id_A_1 = r.Player_Id_A_1,
            Player_Id_A_2 = r.Player_Id_A_2,
            Player_Id_B_1 = r.Player_Id_B_1,
            Player_Id_B_2 = r.Player_Id_B_2,
            ScoreA = r.ScoreA,
            ScoreB = r.ScoreB,
            ResultDateTime = r.ResultDateTime,
        }).ToList();
    }

    public async Task<GetResDto?> GetResult(GetReqDto getReqDto)
    {

        var result = await _repository.FindResultById(getReqDto.Id);

        if (result == null)
        {
            return null;
        }
        return new GetResDto
        {
            Id = result.Id,
            Event_Id = result.Event_Id,
            Event_Name = result.Event.Name,
            Round = result.Round,
            RoundIndex = result.RoundIndex,
            Player_NameA1 = result.PlayerA1?.Name,
            Player_NameA2 = result.PlayerA2?.Name,
            Player_NameB1 = result.PlayerB1?.Name,
            Player_NameB2 = result.PlayerB1?.Name,
            Player_Id_A_1 = result.Player_Id_A_1,
            Player_Id_A_2 = result.Player_Id_A_2,
            Player_Id_B_1 = result.Player_Id_B_1,
            Player_Id_B_2 = result.Player_Id_B_2,
            ScoreA = result.ScoreA,
            ScoreB = result.ScoreB,
        };
    }

    public async Task<List<GetRankingResDto>> GetResultRanking(GetAllReqDto req)
    {
        var results = await _repository.ReadAllResult(req.Event_Id, req.Event_Type, req.SubEventType, req.StartDate, req.EndDate, req.Player_Id_A_1, req.Player_Id_B_1);

        var allResultItemList = await _repository_result_item.ReadAllResultItem();

        var rankings = results.Select(r =>
        {
            bool shouldSwap = false;

            // 如果搜尋條件中有指定 Player_Id_A，且該 ID 在資料庫中是 B 方，則需要交換
            if ((req.Player_Id_A_1.HasValue && r.Player_Id_B_1 == req.Player_Id_A_1.Value) ||
                (req.Player_Id_A_1.HasValue && r.Player_Id_B_2 == req.Player_Id_A_1.Value) ||
                (req.Player_Id_A_2.HasValue && r.Player_Id_B_1 == req.Player_Id_A_2.Value) ||
                (req.Player_Id_A_2.HasValue && r.Player_Id_B_2 == req.Player_Id_A_2.Value)
                )
            {
                shouldSwap = true;
            }
            // 如果搜尋條件中有指定 Player_Id_B，且該 ID 在資料庫中是 A 方，則需要交換
            else if ((req.Player_Id_B_1.HasValue && r.Player_Id_A_1 == req.Player_Id_B_1.Value) ||
                     (req.Player_Id_B_1.HasValue && r.Player_Id_A_2 == req.Player_Id_B_1.Value) ||
                     (req.Player_Id_B_2.HasValue && r.Player_Id_A_1 == req.Player_Id_B_2.Value) ||
                     (req.Player_Id_B_2.HasValue && r.Player_Id_A_2 == req.Player_Id_B_2.Value))
            {
                shouldSwap = true;
            }

            var resultItems = allResultItemList
                .Where(ri => ri.Result_Id == r.Id)
                .Select(ri => new ResultItemDto
                {
                    Result_Id = ri.Result_Id,
                    MatchIndex = ri.MatchIndex,
                    ScoreA = shouldSwap ? ri.ScoreB : ri.ScoreA,
                    ScoreB = shouldSwap ? ri.ScoreA : ri.ScoreB
                })
                .ToList();

            return new GetRankingResDto
            {
                Id = r.Id,
                Event_Id = r.Event_Id,
                Event_Name = r.Event.Name,
                Player_NameA1 = shouldSwap ? r.PlayerB1?.Name : r.PlayerA1?.Name,
                Player_NameB1 = shouldSwap ? r.PlayerA1?.Name : r.PlayerB1?.Name,
                Player_Id_A_1 = shouldSwap ? r.Player_Id_B_1 : r.Player_Id_A_1,
                Player_Id_A_2 = shouldSwap ? r.Player_Id_B_2 : r.Player_Id_A_2,
                Player_Id_B_1 = shouldSwap ? r.Player_Id_A_1 : r.Player_Id_B_1,
                Player_Id_B_2 = shouldSwap ? r.Player_Id_A_2 : r.Player_Id_B_2,
                DoublePlayer_Id_A = r.DoublePlayer_Id_A,
                DoublePlayer_Id_B = r.DoublePlayer_Id_B,
                ScoreA = shouldSwap ? r.ScoreB : r.ScoreA,
                ScoreB = shouldSwap ? r.ScoreA : r.ScoreB,
                ResultDateTime = r.ResultDateTime,
                ResultItemList = resultItems,
                DoublePlayer_Name_A = r.DoublePlayerA?.TeamName,
                DoublePlayer_Name_B = r.DoublePlayerB?.TeamName
            };
        })
        .OrderBy(r => r.ResultDateTime)
        .ToList();

        return rankings;
    }

    public async Task<GetResDto> GetResultByOtherId(int event_id, int round, int roundIndex)
    {

        var result = await _repository.FindResultByOtherId(event_id, round, roundIndex);

        if (result == null)
        {
            return null;
        }
        return new GetResDto
        {
            Id = result.Id,
            Event_Id = result.Event_Id,
            Event_Name = result.Event.Name,
            Round = result.Round,
            RoundIndex = result.RoundIndex,
            Player_NameA1 = result.PlayerA1?.Name,
            Player_NameA2 = result.PlayerA2?.Name,
            Player_NameB1 = result.PlayerB1?.Name,
            Player_NameB2 = result.PlayerB1?.Name,
            Player_Id_A_1 = result.Player_Id_A_1,
            Player_Id_A_2 = result.Player_Id_A_2,
            Player_Id_B_1 = result.Player_Id_B_1,
            Player_Id_B_2 = result.Player_Id_B_2,
            ScoreA = result.ScoreA,
            ScoreB = result.ScoreB,
        };
    }

    public async Task UpdateResult(UpdateReqDto updateReqDto)
    {
        var Result = await _repository.FindResultById(updateReqDto.Id);

        if (Result == null)
        {
            throw new KeyNotFoundException("UpdateResult not found.");
        }

        await _repository.UpdateResult(new Result
        {
            Id = Result.Id,
            Player_Id_A_1 = updateReqDto.Player_Id_A_1,
            Player_Id_A_2 = updateReqDto.Player_Id_A_2,
            Player_Id_B_1 = updateReqDto.Player_Id_B_1,
            Player_Id_B_2 = updateReqDto.Player_Id_B_2,
            ScoreA = updateReqDto.ScoreA,
            ScoreB = updateReqDto.ScoreB,
        });
    }

    public async Task DeleteResult(int event_id)
    {
        // 先刪除 ResultItem
        var result = await _repository.FindResultListByEventId(event_id);
        var idList = result.Select(r => r.Id).ToList();
        await _repository_result_item.DeleteResultItem(idList);


        // 在刪除 Result
        await _repository.DeleteResult(event_id);
    }

    public class ScoreManager
    {
        private List<(int Min, int Max, int HighWinPoints, int LowWinPoints)> scoreRules = new List<(int Min, int Max, int HighWinPoints, int LowWinPoints)>
        {
            // 假設的範圍和分數，您需要用實際的值來替換這些
            (0, 30, 9, 9),    // 範圍0-30，高分獲勝+9，低分獲勝+9
            (31, 61, 8, 11),    // 範圍31-61，高分獲勝+8，低分獲勝+11
            (62, 92, 7, 15),
            (93, 123, 6, 21),
            (124, 154, 5, 29),
            (155, 185, 4, 39),
            (186, 216, 3, 51),
            (217, 247, 2, 65),
            (247, int.MaxValue, 1, 81), // 範圍247以上，高分獲勝+1，低分獲勝+81
        };

        public (int, int) UpdateScores(int scoreA, int scoreB, char winner)
        {
            int scoreAChange = 0;
            int scoreBChange = 0;
            int diff = Math.Abs(scoreA - scoreB);

            var scoreRule = scoreRules.FirstOrDefault(rule => diff >= rule.Min && diff <= rule.Max);

            if (winner == 'A')
            {
                scoreAChange += scoreA > scoreB ? scoreRule.HighWinPoints : scoreRule.LowWinPoints;
                scoreBChange -= scoreA > scoreB ? scoreRule.HighWinPoints : scoreRule.LowWinPoints;
            }
            else if (winner == 'B')
            {
                scoreAChange -= scoreB > scoreA ? scoreRule.HighWinPoints : scoreRule.LowWinPoints;
                scoreBChange += scoreB > scoreA ? scoreRule.HighWinPoints : scoreRule.LowWinPoints;
            }

            // 更新分數
            scoreA += scoreAChange;
            scoreB += scoreBChange;

            return (scoreA, scoreB);
        }
    }
}
