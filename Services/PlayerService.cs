
using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Player;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;



public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _repository;
    private readonly IResultRepository _resultRepository;


    public PlayerService(IPlayerRepository repository, IResultRepository resultRepository)
    {
        _repository = repository;
        _resultRepository = resultRepository;
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
        // TODO create 3's enum
        var result = await _resultRepository.ReadAllResult(null, 3);

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
