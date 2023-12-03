
using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Result;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;


public class ResultService : IResultService
{
    private readonly IResultRepository _repository;

    public ResultService(IResultRepository repository)
    {
        _repository = repository;
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
                        Id = matchId++, // 分配比赛 ID 并递增
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


    public async Task<List<GetAllResDto>> GetAllResult()
    {

        var results = await _repository.ReadAllResult();
        return results.Select(r => new GetAllResDto
        {
            Id = r.Id,
            Event_Id = r.Event_Id,
            Event_Name = r.Event.Name,
            Round = r.Round,
            Player_NameA1 = r.PlayerA1?.Name,
            Player_NameA2 = r.PlayerA2?.Name,
            Player_NameB1 = r.PlayerB1?.Name,
            Player_NameB2 = r.PlayerB2?.Name
        }).ToList();
    }

    public async Task UpdateResult(UpdateReqDto updateReqDto)
    {
        var Result = await _repository.FindResultById(updateReqDto.Id);

        if (Result == null)
        {
            throw new KeyNotFoundException("Result not found.");
        }

        // Result.Name = updateReqDto.Name;
        await _repository.UpdateResult(Result);
    }
}
