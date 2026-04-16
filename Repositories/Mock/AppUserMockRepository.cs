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
                Username = "ivan.horvat",
                DisplayName = "Ivan Horvat",
                Email = "ivan.horvat@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 1, 15),
                ManagedCompetitionIds = new() { 1 }
            },
            new AppUser
            {
                Id = 2,
                Username = "marko.milic",
                DisplayName = "Marko Milic",
                Email = "marko.milic@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 2, 20),
                ManagedCompetitionIds = new() { 2 }
            },
            new AppUser
            {
                Id = 3,
                Username = "ante.antic",
                DisplayName = "Ante Antic",
                Email = "ante.antic@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 3, 10),
                ManagedCompetitionIds = new() { 1, 3 }
            },
            new AppUser
            {
                Id = 4,
                Username = "ana.kovacevic",
                DisplayName = "Ana Kovacevic",
                Email = "ana.kovacevic@handball.hr",
                Role = UserRole.Admin,
                CreatedAt = new DateTime(2022, 6, 1),
                ManagedCompetitionIds = new() { 1, 2, 3, 4 }
            },
            new AppUser
            {
                Id = 5,
                Username = "petar.novak",
                DisplayName = "Petar Novak",
                Email = "petar.novak@handball.hr",
                Role = UserRole.TournamentManager,
                CreatedAt = new DateTime(2023, 4, 5),
                ManagedCompetitionIds = new() { 2, 3 }
            },
            new AppUser
            {
                Id = 6,
                Username = "drago.horvat",
                DisplayName = "Drago Horvat",
                Email = "drago.horvat@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 5, 12),
                ManagedCompetitionIds = new() { 4 }
            },
            new AppUser
            {
                Id = 7,
                Username = "jure.horvat",
                DisplayName = "Jure Horvat",
                Email = "jure.horvat@handball.hr",
                Role = UserRole.Coach,
                CreatedAt = new DateTime(2023, 7, 8),
                ManagedCompetitionIds = new() { 3 }
            },
            new AppUser
            {
                Id = 8,
                Username = "vesna.sekulic",
                DisplayName = "Vesna Sekulic",
                Email = "vesna.sekulic@handball.hr",
                Role = UserRole.TournamentManager,
                CreatedAt = new DateTime(2023, 8, 22),
                ManagedCompetitionIds = new() { 1, 4 }
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
