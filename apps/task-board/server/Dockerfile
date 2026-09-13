FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY Directory.Build.props ./
COPY apps/task-board/server/TaskBoard.Api.csproj apps/task-board/server/
RUN dotnet restore apps/task-board/server/TaskBoard.Api.csproj
COPY apps/task-board/server/ apps/task-board/server/
RUN dotnet publish apps/task-board/server/TaskBoard.Api.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app ./
EXPOSE 5080
ENTRYPOINT ["dotnet", "TaskBoard.Api.dll"]

