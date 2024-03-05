
using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Player;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;
using ResultGetAllReqDto = table_tennis_backend.Dtos.Result.GetAllReqDto;



public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _repository;
    private readonly IResultService _resultService;


    public PlayerService(IPlayerRepository repository, IResultService resultService)
    {
        _repository = repository;
        _resultService = resultService;
    }

    public async Task AddPlayer(AddReqDto[] req)
    {

        var result = req.Select(r => new Player { Name = r.Name });

        await _repository.CreatePlayer(result);
        return;
    }

    public async Task<List<GetAllResDto>> GetAllPlayer()
    {
        var playerList = await _repository.ReadAllPlayer();
        var result = await _resultService.GetAllResult(new ResultGetAllReqDto
        {
            // TODO create enum
            Event_Type = 3
        });

        return playerList.Select(r => new GetAllResDto
        {
            Id = r.Id,
            Name = r.Name,
            Score = r.Score,
            ResultCount = result.Select(r => new
            {
                IdA = r.Player_Id_A_1,
                IdB = r.Player_Id_B_1
            })
                .Where(ri => ri.IdA == r.Id || ri.IdB == r.Id).Count()
        }).ToList();
    }

    public async Task UpdatePlayer(UpdateReqDto updateReqDto)
    {
        var player = await _repository.FindPlayerById(updateReqDto.Id);

        if (player == null)
        {
            throw new KeyNotFoundException("Player not found.");
        }

        player.Name = updateReqDto.Name;
        await _repository.UpdatePlayer(player);
    }
}
