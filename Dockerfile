FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY RecipeApp_Eboaguillaume/RecipeApp_Eboaguillaume.csproj RecipeApp_Eboaguillaume/
COPY SQLAccess/SQLAccess.csproj SQLAccess/
RUN dotnet restore RecipeApp_Eboaguillaume/RecipeApp_Eboaguillaume.csproj

COPY RecipeApp_Eboaguillaume/ RecipeApp_Eboaguillaume/
COPY SQLAccess/ SQLAccess/
RUN dotnet publish RecipeApp_Eboaguillaume/RecipeApp_Eboaguillaume.csproj -c Release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

RUN useradd --uid 1654 --user-group appuser
USER appuser

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RecipeApp_Eboaguillaume.dll"]
