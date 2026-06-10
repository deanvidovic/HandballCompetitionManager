namespace HandballCompetitionManager.Repositories.Mock;

using HandballCompetitionManager.Models;

public class AppUserMockRepository
{
    private List<AppUser> _users;

    public AppUserMockRepository()
    {
        InitializeUsers();
    }

    private void InitializeUsers()
    {
        _users = new()
        {
            new AppUser
            {
                Id = 1,
                UserName = "ivan.horvat",
                DisplayName = "Ivan Horvat",
                Email = "ivan.horvat@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 1, 15),
                ManagedCompetitions = new List<Competition>()
            },
            new AppUser
            {
                Id = 2,
                UserName = "marko.milic",
                DisplayName = "Marko Milic",
                Email = "marko.milic@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 2, 20),
                ManagedCompetitions = new List<Competition>()
            },
            new AppUser
            {
                Id = 3,
                UserName = "ante.antic",
                DisplayName = "Ante Antic",
                Email = "ante.antic@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 3, 10),
                ManagedCompetitions = new List<Competition>()
            },
            new AppUser
            {
                Id = 4,
                UserName = "ana.kovacevic",
                DisplayName = "Ana Kovacevic",
                Email = "ana.kovacevic@handball.hr",
                Role = UserRole.Admin,
                CreatedAt = new DateTime(2022, 6, 1),
                ManagedCompetitions = new List<Competition>()
            },
            new AppUser
            {
                Id = 5,
                UserName = "petar.novak",
                DisplayName = "Petar Novak",
                Email = "petar.novak@handball.hr",
                Role = UserRole.TournamentManager,
                CreatedAt = new DateTime(2023, 4, 5),
                ManagedCompetitions = new List<Competition>()
            },
            new AppUser
            {
                Id = 6,
                UserName = "drago.horvat",
                DisplayName = "Drago Horvat",
                Email = "drago.horvat@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 5, 12),
                ManagedCompetitions = new List<Competition>()
            },
            new AppUser
            {
                Id = 7,
                UserName = "jure.horvat",
                DisplayName = "Jure Horvat",
                Email = "jure.horvat@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 7, 8),
                ManagedCompetitions = new List<Competition>()
            },
            new AppUser
            {
                Id = 8,
                UserName = "vesna.sekulic",
                DisplayName = "Vesna Sekulic",
                Email = "vesna.sekulic@handball.hr",
                Role = UserRole.TournamentManager,
                CreatedAt = new DateTime(2023, 8, 22),
                ManagedCompetitions = new List<Competition>()
            }
        };
    }

    public List<AppUser> GetAll()
    {
        return _users ?? new List<AppUser>();
    }

    public AppUser? GetById(int id)
    {
        return GetAll().FirstOrDefault(u => u.Id == id);
    }

    public List<AppUser> GetByRole(UserRole role)
    {
        return GetAll().Where(u => u.Role == role).ToList();
    }
}
