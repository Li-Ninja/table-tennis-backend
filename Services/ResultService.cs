
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
            Event_id = r.Event_Id,
            Event_name = r.Event.Name,
            Round = r.Round,
            Player_Id_A_1 = r.Player_Id_A_1,
            Player_Id_A_2 = r.Player_Id_A_2,
            Player_Id_B_1 = r.Player_Id_B_1,
            Player_Id_B_2 = r.Player_Id_B_2
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
