
using table_tennis_backend.Services;
using DtoResult = table_tennis_backend.Dtos.Result;
using table_tennis_backend.Dtos.ResultItem;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;

public class ResultItemService : IResultItemService
{
    private readonly IResultItemRepository _repository;
    private readonly IResultService _resultService;


    public ResultItemService(IResultItemRepository repository, IResultService resultService)
    {
        _repository = repository;
        _resultService = resultService;

    }

    public async Task<string> AddResultItem(AddReqDto[] req)
    {

        var result = req.Select(r => new ResultItem
        {
            Result_Id = r.Result_Id,
            MatchIndex = r.MatchIndex,
            ScoreA = r.ScoreA,
            ScoreB = r.ScoreB
        });

        await _repository.CreateResultItem(result);

        int winsA = 0;
        int winsB = 0;
        foreach (var item in result)
        {
            if (item.ScoreA > item.ScoreB)
            {
                winsA++;
            }
            else if (item.ScoreB > item.ScoreA)
            {
                winsB++;
            }
        }

        var winnerId = winsA > winsB ? 'A' : 'B';

        if (req == null || req.Length == 0)
        {
            return "req array is empty or null";
        }

        var id = req[0].Result_Id;
        var getReqDto = new DtoResult.GetReqDto { Id = id };
        var result2 = await _resultService.GetResult(getReqDto);

        if (result2 == null)
        {
            return "result2 not found";
        }

        var updateReqDto = new DtoResult.UpdateReqDto
        {
            Id = result2.Id,
            Player_Id_A_1 = result2.Player_Id_A_1,
            Player_Id_A_2 = result2.Player_Id_A_2,
            Player_Id_B_1 = result2.Player_Id_B_1,
            Player_Id_B_2 = result2.Player_Id_B_2,
            ScoreA = winsA,
            ScoreB = winsB,
        };

        // update score
        await _resultService.UpdateResult(updateReqDto);

        // update next round player id
        if (result2.Round != 2)
        {

            var nextRound = result2.Round / 2;
            var nextRoundIndexPosition = result2.RoundIndex % 2 != 0 ? 'A' : 'B';
            // TODO 優化算式
            var nextRoundIndex = result2.RoundIndex / 2.0 <= 1 ? 1 : (result2.RoundIndex / 2.0 <= 2 ? 2 : (result2.RoundIndex / 2.0 <= 3 ? 3 : (result2.RoundIndex / 2.0 <= 4 ? 4 : (result2.RoundIndex / 2.0 <= 5 ? 5 : 6))));


            var result3 = await _resultService.GetResultByOtherId(result2.Event_Id, nextRound, nextRoundIndex);

            var winnerPlayerId1 = winnerId == 'A' ? result2.Player_Id_A_1 : result2.Player_Id_B_1;
            var winnerPlayerId2 = winnerId == 'A' ? result2.Player_Id_A_2 : result2.Player_Id_B_2;

            // var nextPlayerId1 = nextRoundIndexPosition == 'A'

            if (nextRoundIndexPosition == 'A')
            {

                var updateReqDto2 = new DtoResult.UpdateReqDto
                {
                    Id = result3.Id,
                    Player_Id_A_1 = winnerPlayerId1,
                    Player_Id_A_2 = winnerPlayerId2,
                    Player_Id_B_1 = result3.Player_Id_B_1,
                    Player_Id_B_2 = result3?.Player_Id_B_2,
                };

                await _resultService.UpdateResult(updateReqDto2);

            }
            else
            {


                var updateReqDto2 = new DtoResult.UpdateReqDto
                {
                    Id = result3.Id,

                    Player_Id_A_1 = result3?.Player_Id_A_1,
                    Player_Id_A_2 = result3?.Player_Id_A_2,
                    Player_Id_B_1 = winnerPlayerId1,
                    Player_Id_B_2 = winnerPlayerId2,
                };

                await _resultService.UpdateResult(updateReqDto2);

            }
        }

        return "success";
    }


    public async Task<List<GetResDto>> GetAllResultItemList()
    {
        var result = await _repository.ReadAllResultItem();
        return result.Select(r => new GetResDto
        {
            Result_Id = r.Result_Id,
            MatchIndex = r.MatchIndex,
            ScoreA = r.ScoreA,
            ScoreB = r.ScoreB,
        }).ToList();
    }

    public async Task<List<GetResDto>> GetResultItemList(int id)
    {
        var result = await _repository.FindResultItemById(id);
        return result.Select(r => new GetResDto
        {
            Result_Id = r.Result_Id,
            MatchIndex = r.MatchIndex,
            ScoreA = r.ScoreA,
            ScoreB = r.ScoreB,
        }).ToList();
    }

    public async Task DeleteResultItem(int result_id)
    {
        // 先刪除 ResultItem
        await _repository.DeleteResultItemById(result_id);
    }
}
