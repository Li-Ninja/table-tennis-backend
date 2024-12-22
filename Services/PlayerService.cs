
using table_tennis_backend.Services;
using table_tennis_backend.Const;
using table_tennis_backend.Dtos.Player;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;
using Microsoft.EntityFrameworkCore;



public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _repository;
    private readonly IResultRepository _resultRepository;
    private readonly IScoreService _scoreService;


    public PlayerService(IPlayerRepository repository, IResultRepository resultRepository, IScoreService scoreService)
    {
        _repository = repository;
        _resultRepository = resultRepository;
        _scoreService = scoreService;
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
        var result = await _resultRepository.ReadAllResult(null, 3, null, null, null, null);
        var sortedPlayerList = playerList
                                .Where(p => p.Status != PlayerStatusEnum.Freeze)
                                .OrderByDescending(p => p.Score)
                                .ToList();

        int? currentRank = 1;
        int previousScore = -1;
        int skipCount = 0;
        DateTimeOffset twoMonthsAgo = DateTimeOffset.Now.AddMonths(-2);

        return sortedPlayerList.Select(r =>
        {
            int? rank = null;

            if (r.Status == PlayerStatusEnum.OnLeave || (r.LatestResultDateTime != null && r.LatestResultDateTime >= twoMonthsAgo))
            {
                if (r.Score != previousScore)
                {
                    currentRank += skipCount;
                    skipCount = 1;
                }
                else
                {
                    skipCount++;
                }
                rank = currentRank;
            }

            previousScore = r.Score;

            return new GetAllResDto
            {
                Id = r.Id,
                Name = r.Name,
                Score = r.Score,
                IsMan = r.IsMan,
                IsRightHand = r.IsRightHand,
                RacketType = r.RacketType,
                ForehandRubberType = r.ForehandRubberType,
                BackhandRubberType = r.BackhandRubberType,
                IsOnLeave = r.Status == PlayerStatusEnum.OnLeave,
                Rank = rank,
                ResultCount = result.Select(r => new
                {
                    IdA = r.Player_Id_A_1,
                    IdB = r.Player_Id_B_1
                }).Where(ri => ri.IdA == r.Id || ri.IdB == r.Id).Count(),
                WinningCount = result.Select(r => new
                {
                    IdA = r.Player_Id_A_1,
                    ScoreA = r.ScoreA,
                    ScoreB = r.ScoreB
                }).Where(ri => ri.IdA == r.Id && ri.ScoreA > ri.ScoreB).Count()
                + result.Select(r => new
                {
                    IdB = r.Player_Id_B_1,
                    ScoreA = r.ScoreA,
                    ScoreB = r.ScoreB
                }).Where(ri => ri.IdB == r.Id && ri.ScoreB > ri.ScoreA).Count(),
                UpdateDateTime = r.UpdateDateTime,
                LatestResultDateTime = r.LatestResultDateTime
            };
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

    public async Task<GetComparisonResDto> GetComparison(GetComparisonReqDto req)
    {
        var playerAAll = await _scoreService.GetPlayerStats(req.IdA, req.IdB, PlayerComparisonTypeEnum.All);
        var playerARecent = await _scoreService.GetPlayerStats(req.IdA, req.IdB, PlayerComparisonTypeEnum.Recent);
        var playerBAll = await _scoreService.GetPlayerStats(req.IdB, req.IdA, PlayerComparisonTypeEnum.All);
        var playerBRecent = await _scoreService.GetPlayerStats(req.IdB, req.IdA, PlayerComparisonTypeEnum.Recent);

        var playerA = new PlayerComparison
        {
            All = playerAAll,
            Recent = playerARecent
        };
        var playerB = new PlayerComparison
        {
            All = playerBAll,
            Recent = playerBRecent
        };

        return new GetComparisonResDto
        {
            PlayerA = playerA,
            PlayerB = playerB
        };
    }
}
