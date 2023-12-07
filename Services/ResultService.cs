
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


    public async Task<List<GetResDto>> GetAllResult()
    {

        var results = await _repository.ReadAllResult();
        return results.Select(r => new GetResDto
        {
            Id = r.Id,
            Event_Id = r.Event_Id,
            Event_Name = r.Event.Name,
            Round = r.Round,
            RoundIndex = r.RoundIndex,
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
}
