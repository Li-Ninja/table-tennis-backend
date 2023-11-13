
using table_tennis_backend.Services;
using table_tennis_backend.Dtos.Player;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;


public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _repository;

    public PlayerService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task AddPlayer(AddReqDto[] req)
    {

        var result = req.Select(r => new Player { Name = r.Name });

        await _repository.CreatePlayer(result);
        return;
    }

    public async Task<List<Player>> GetAllPlayer()
    {
        return await _repository.ReadAllPlayer();
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
