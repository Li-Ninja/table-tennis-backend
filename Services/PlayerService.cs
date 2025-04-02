
using table_tennis_backend.Services;
using table_tennis_backend.Const;
using table_tennis_backend.Dtos.Player;
using GetDoublePlayerDto = table_tennis_backend.Dtos.DoublePlayer.GetAllResDto;
using UpdateDoublePlayerDto = table_tennis_backend.Dtos.DoublePlayer.UpdateReqDto;
using table_tennis_backend.Database.MsSql.TableTennis.Model;
using table_tennis_backend.Database.MsSql.TableTennis.Repositories;


public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _repository;
    private readonly IDoublePlayerRepository _doublePlayerRepository;
    private readonly IResultRepository _resultRepository;
    private readonly IScoreService _scoreService;


    public PlayerService(IPlayerRepository repository, IDoublePlayerRepository doublePlayerRepository, IResultRepository resultRepository, IScoreService scoreService)
    {
        _repository = repository;
        _doublePlayerRepository = doublePlayerRepository;
        _resultRepository = resultRepository;
        _scoreService = scoreService;
    }

    public async Task AddPlayer(AddReqDto[] req)
    {

        var result = req.Select(r => new Player { Name = r.Name });

        await _repository.CreatePlayer(result);
        return;
    }

    public async Task<List<GetDoublePlayerDto>> GetAllDoublePlayer()
    {
        var playerList = await _repository.ReadAllPlayer();
        var doublePlayerList = await _doublePlayerRepository.ReadAllDoublePlayer();
        var result = await _resultRepository.ReadAllResult(null, EventTypeEnum.Score, SubEventTypeEnum.Double, null, null, null, null, null, null);
        var sortedDoublePlayerList = doublePlayerList
                                .Where(p => p.Status != DoublePlayerStatusEnum.Disabled)
                                .OrderByDescending(p => p.Score)
                                .ToList();


        return sortedDoublePlayerList.Select((r, index) =>
        {
            return new GetDoublePlayerDto
            {
                Id = r.Id,
                TeamName = r.TeamName,
                Player_Id_1 = r.Player_Id_1,
                Player_Id_2 = r.Player_Id_2,
                Player_Name_1 = playerList.FirstOrDefault(p => p.Id == r.Player_Id_1)?.Name ?? "",
                Player_Name_2 = playerList.FirstOrDefault(p => p.Id == r.Player_Id_2)?.Name ?? "",
                Score = r.Score,
                Rank = index + 1,
                Status = r.Status,
                ResultCount = result.Select(r => new
                {
                    IdA = r.DoublePlayer_Id_A,
                    IdB = r.DoublePlayer_Id_B
                }).Where(ri => ri.IdA == r.Id || ri.IdB == r.Id).Count(),
                ResultCountByYearly = result.Select(r => new
                {
                    IdA = r.DoublePlayer_Id_A,
                    IdB = r.DoublePlayer_Id_B,
                    ResultDateTime = r.ResultDateTime
                })
               .Where(ri => (ri.IdA == r.Id || ri.IdB == r.Id)
                 && (ri.ResultDateTime.Month == 12
                     ? ri.ResultDateTime.Year + 1
                     : ri.ResultDateTime.Year) == (DateTimeOffset.Now.Month == 12 ? DateTimeOffset.Now.Year + 1 : DateTimeOffset.Now.Year))
                .Count(),
                WinningCount = result.Select(r => new
                {
                    IdA = r.DoublePlayer_Id_A,
                    ScoreA = r.ScoreA,
                    ScoreB = r.ScoreB
                }).Where(ri => ri.IdA == r.Id && ri.ScoreA > ri.ScoreB).Count()
                + result.Select(r => new
                {
                    IdB = r.DoublePlayer_Id_B,
                    ScoreA = r.ScoreA,
                    ScoreB = r.ScoreB
                }).Where(ri => ri.IdB == r.Id && ri.ScoreB > ri.ScoreA).Count(),
                UpdateDateTime = r.UpdateDateTime,
                LatestResultDateTime = r.LatestResultDateTime
            };
        }).ToList();
    }

    public async Task<List<GetAllResDto>> GetAllPlayer()
    {
        var playerList = await _repository.ReadAllPlayer();
        var result = await _resultRepository.ReadAllResult(null, EventTypeEnum.Score, SubEventTypeEnum.Single, null, null, null, null, null, null);
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
                ResultCountByYearly = result.Select(r => new
                {
                    IdA = r.Player_Id_A_1,
                    IdB = r.Player_Id_B_1,
                    ResultDateTime = r.ResultDateTime
                })
               .Where(ri => (ri.IdA == r.Id || ri.IdB == r.Id)
                 && (ri.ResultDateTime.Month == 12
                     ? ri.ResultDateTime.Year + 1
                     : ri.ResultDateTime.Year) == (DateTimeOffset.Now.Month == 12 ? DateTimeOffset.Now.Year + 1 : DateTimeOffset.Now.Year))
                .Count(),
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
