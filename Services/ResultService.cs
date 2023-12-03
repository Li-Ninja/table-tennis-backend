
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

    // public async Task AddResult(AddReqDto[] req)
    // {

    //     var result = req.Select(r => new Result { Id = r.Name });

    //     await _repository.CreateResult(result);
    //     return;
    // }

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
