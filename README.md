# Happy Wedding - Plan wedding and send invitations

## Summary

Happy Wedding is a wedding planning API that allows couples and families to:
- Track key milestones
- Plan expenses with flexible multi-category budget entries
- Manage guest lists for both bride and groom families
- Provide authentication flows and secured access

## Tech Stack

- .NET 10 (ASP.NET Core Web API)
- Entity Framework Core (Code First + Migrations)
- SQL Server (configured via connection string in `appsettings.json`)
- JWT authentication + refresh tokens


## Setup

1. Clone the repository:
   ```powershell
   git clone <repo-url>
   cd HappyWedding.Api
   ```
2. Configure your DB connection in `appsettings.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HappyWeddingDb;Trusted_Connection=True;MultipleActiveResultSets=true"
   }
   ```
3. Restore dependencies:
   ```powershell
   dotnet restore
   ```
4. Apply EF migrations and create the database:
   ```powershell
   dotnet ef database update
   ```
5. Run the API:
   ```powershell
   dotnet run
   ```
6. Open Scalar for API docs:
   `https://localhost:{port}/scalar`
   ![Screenshot 1](./public/Screenshot-api.png)
<!-- ### Auth
- `POST /api/auth/register` — register new user
- `POST /api/auth/login` — authenticate and receive JWT
- `POST /api/auth/refresh-token` — get refreshed token
- `GET /api/auth` — get currently authenticated user details
- `GET /api/auth/admin` — admin access test endpoint

### Weddings
- `GET /api/wedding` — list weddings
- `POST /api/wedding` — create wedding
- `PUT /api/wedding` — update wedding
- `DELETE /api/wedding` — delete wedding

### Guests
- `GET /api/wedding/guests` — list wedding guests
- `POST /api/wedding/guests` — create guest entry
- `PUT /api/wedding/guests/{id}` — update guest
- `DELETE /api/wedding/guests/{id}` — remove guest

### Milestones
- `GET /api/wedding/milestones` — list wedding milestones
- `POST /api/wedding/milestones` — create milestone
- `PUT /api/wedding/milestones/{id}` — update milestone
- `DELETE /api/wedding/milestones/{id}` — delete milestone

### Invitations
- `GET /api/invitation/{weddingId}` — get invitation info for wedding -->

## Links

<!-- - Deployment: `https://<your-api-deploy-url>` (replace with actual URL) -->
- Frontend repo:
`https://github.com/Gloxiniaaa/your-love-story-planner`
- API Documentation: 
`https://github.com/Gloxiniaaa/your-love-story-planner`
- Browse this source code:
`https://github.dev/Gloxiniaaa/HappyWedding.Api`