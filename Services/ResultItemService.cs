
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
            ScoreA = result2.ScoreA,
            ScoreB = result2.ScoreB,
        };

        // update score
        await _resultService.UpdateResult(updateReqDto);

        // update next player id

        return "success";
    }


    public async Task<List<GetResDto>> GetResultItemList(GetReqDto getReqDto)
    {
        var result = await _repository.FindResultItemById(getReqDto.Result_Id);
        return result.Select(r => new GetResDto
        {
            Result_Id = r.Result_Id,
            MatchIndex = r.MatchIndex,
            ScoreA = r.ScoreA,
            ScoreB = r.ScoreB,
        }).ToList();
    }
}
